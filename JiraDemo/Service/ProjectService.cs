using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.DTO;
using WebApplication3.Models;

namespace WebApplication3.Service;

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _context;

    public ProjectService(ApplicationDbContext context)
    {
        _context = context;
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
        var projects = await _context.Project.Include(a => a.Members).Where(p => p.OwnerId == ownerId || p.Members.Any(b => b.UserId == ownerId)).ToListAsync();
        return projects.Select(p => new ProjectDTO(p.Id, p.Name, p.Description, p.OwnerId, p.CreatedAt));
    }

    public async Task<ProjectDTO> GetByIdAsync(int id)
    {
        var p = await _context.Project.FindAsync(id);
        return new ProjectDTO(p.Id, p.Name, p.Description, p.OwnerId, p.CreatedAt);
    }

    public async Task RemoveMemberAsync(int userId, int projectId)
    {
        var member = await _context.ProjectMember.FindAsync(userId, projectId);
        if (member == null)
        {
            throw new Exception("Member not found");
        }
        _context.ProjectMember.Remove(member);
        await _context.SaveChangesAsync();
    }

    public async Task AddMemberAsync(int userId, int projectId)
    {
       var project = await _context.Project.FindAsync(projectId);
       if (project != null)
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