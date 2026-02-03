using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.DTO;
using WebApplication3.Models;
using JiraDemo.Redis;
using System.Text.Json;

namespace WebApplication3.Service;

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _context;
    private readonly IRedis _redis;

    public ProjectService(ApplicationDbContext context, IRedis redis)
    {
        _context = context;
        _redis = redis;
    }

    public async Task<ProjectDTO> CreateProjectAsync(ProjectCreateDTO project, int ownerId)
    {
        var proj = new Project
        {
            Name = project.name,
            Description = project.description,
            OwnerId = ownerId,
        };
        await _context.Project.AddAsync(proj);
        await _context.ProjectMember.AddAsync(new ProjectMember { UserId = ownerId,  Project = proj });

        await _context.SaveChangesAsync();
        return new ProjectDTO(proj.Id, proj.Name, proj.Description, proj.OwnerId, proj.CreatedAt);
    }

    public async Task<IEnumerable<ProjectDTO>> GetAllAsync(int ownerId)
    {
        var project = await _context.Project.Include(x => x.Members).Where(x => x.OwnerId == ownerId || x.Members.Any(b => b.UserId==ownerId)).ToListAsync();
        return project.Select(x => new ProjectDTO(x.Id, x.Name, x.Description, x.OwnerId, x.CreatedAt));
    }

    public async Task<ProjectDTO> GetByIdAsync(int id, int ownerId)
    {
        var project = $"project:{id}:user:{ownerId}";
        
        var cache = await _redis.GetAsync(project);
        
        if (cache != null)
            return JsonSerializer.Deserialize<ProjectDTO>(cache)!;
        var prDB = await _context.Project.Include(x=>x.Members).SingleOrDefaultAsync(x => x.Id == id && (x.OwnerId == ownerId || x.Members.Any(m => m.UserId == ownerId)));
        if (prDB == null) throw new Exception("Project not found");
        var dto = new ProjectDTO(prDB.Id, prDB.Name, prDB.Description, prDB.OwnerId, prDB.CreatedAt);

        await _redis.SetAsync(project, JsonSerializer.Serialize(dto), TimeSpan.FromMinutes(60));

        return dto;
    }

    public async Task RemoveMemberAsync(int userId, int projectId, int ownerId)
    {
        var project = await _context.Project.FindAsync(projectId);
       var member = await _context.ProjectMember.Where(x => x.ProjectId == projectId && x.UserId == userId).FirstOrDefaultAsync();
       /*if (member == null)
      {
          throw new Exception("Member not found");
      }*/

        if (project.OwnerId == ownerId)
        {
            _context.ProjectMember.Remove(member);
        }
        else
        {
            throw new Exception("You are not the owner of this project");
        }
        await _context.SaveChangesAsync();
    }

    public async Task AddMemberAsync(int userId, int projectId, int ownerId)
    {
       var project = await _context.Project.FindAsync(projectId);
       if (project != null && project.OwnerId == ownerId)
       {
           if (await _context.ProjectMember.AnyAsync(p => p.UserId == userId && p.ProjectId == projectId))
           {
               return;
           }
           await _context.ProjectMember.AddAsync(new ProjectMember { UserId = userId, ProjectId = projectId });
           await _context.SaveChangesAsync();
           return;
       }
       throw new Exception("Project not found");
    }

    public async Task RemoveProjectAsync(int projectId, int userId)
    {
        var project = await _context.Project.FindAsync(projectId);
        if (project == null)
        {
            throw new Exception("Project not found");
        }

        if (project.OwnerId != userId)
        {
            throw new Exception("You are not owner of this project");
        }

         _context.Project.Remove(project);
        await _context.SaveChangesAsync();
    }
}