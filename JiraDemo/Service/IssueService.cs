using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.DTO;
using WebApplication3.Models;
using JiraDemo.Redis;

namespace WebApplication3.Service;

public class IssueService : IIssueService
{
    private readonly ApplicationDbContext _context;
    private readonly IRedis _redis;
    public IssueService(ApplicationDbContext context, IRedis redis)
    {
        _context = context;
        _redis = redis;
    }
    public async Task<IssueDto> CreateIssueAsync(IssueCreateDTO issueCreateDTO, int projectId, int reporterId)
    {
        if( await _context.Project.FindAsync(projectId) == null) throw new Exception("Project not found");
        var issue = new Issue
        {
            ProjectId = projectId, 
            ReporterId = reporterId,
            Title = issueCreateDTO.title,
            Description = issueCreateDTO.description,
            IssueType = issueCreateDTO.IssueType,
            IssuePriority = issueCreateDTO.Priority,
            AssigneeId = (int)issueCreateDTO.AssigneeId
        };
        await _context.Issues.AddAsync(issue);
        await _context.SaveChangesAsync();
        return Map(issue);
    }

    public async Task<IEnumerable<IssueDto>> GetAllIssuesAsync(int projectId)
    {
        var issues = await _context.Issues.Where(x => x.ProjectId == projectId).ToListAsync();
        return issues.Select(Map);
    }
    
    public async Task<IssueDto> GetIssueAsync(int issueId)
    {
        var issue = $"issue:{issueId}";
        
        var cache = await _redis.GetAsync(issue);
        
        if (cache != null)
            return JsonSerializer.Deserialize<IssueDto>(cache)!;
        var isDB = await _context.Issues.FindAsync(issueId);
        if (isDB == null) throw new Exception("Issue not found");
        var dto = Map(isDB);
        await _redis.SetAsync(issue, JsonSerializer.Serialize(dto), TimeSpan.FromMinutes(60));

        return dto;
    }

    public async Task UpdateIssueAsync(IssueUpdateDTO DTO, int issueId)
    {
        var res = await _context.Issues.FindAsync(issueId);
        if(res==null) throw new Exception("Issue not found");
        if(DTO.title!=null) res.Title = DTO.title;
        if(DTO.description!=null) res.Description = DTO.description;
        if(DTO.IssueType.HasValue) res.IssueType = DTO.IssueType.Value;
        if(DTO.Priority.HasValue) res.IssuePriority = DTO.Priority.Value;
        if(DTO.AssigneeId!=null) res.AssigneeId = DTO.AssigneeId;
        res.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task ChangeIssueStatusAsync(int issueId, string status)
    {
        if( await _context.Issues.FindAsync(issueId) == null ) throw new Exception("Issue not found");
        var res = await _context.Issues.FindAsync(issueId);
        res.IssueStatus = (IssueStatus)Enum.Parse(typeof(IssueStatus), status);
        res.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteIssueAsync(int issueId)
    {
        if( await _context.Issues.FindAsync(issueId) == null ) throw new Exception("Issue not found");
        var issue = await _context.Issues.FindAsync(issueId);
        _context.Issues.Remove(issue);
        await _context.SaveChangesAsync();
    }

    public async Task AsignAsigneeAsync(int issueId, int asigneeId)
    {
        if( await _context.Issues.FindAsync(issueId) == null ) throw new Exception("Issue not found");
        var issue = await _context.Issues.FindAsync(issueId);
        issue.AssigneeId = asigneeId;
        await _context.SaveChangesAsync();
    }

    private IssueDto Map(Issue issue)
    {
        return new IssueDto(issue.Id,  issue.ProjectId,issue.Title, issue.Description, issue.IssueType, issue.IssuePriority, issue.AssigneeId, issue.ReporterId,issue.CreatedAt, issue.UpdatedAt );
        
    }
}