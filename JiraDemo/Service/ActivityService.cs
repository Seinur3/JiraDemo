using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Service;

public class ActivityService : IActivityService
{
    private readonly ApplicationDbContext _context;
    public ActivityService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task LogAsync(int? userId, string entityType, string entityId, string action, string? oldValue = null,
        string? newValue = null)
    {
        _context.ActivityLog.Add(new ActivityLog
            {
                UserId = userId,
                EntityType = entityType,
                EntityId = entityId,
                Action = action,
                OldValue = oldValue,
                NewValue = newValue
            }
        );
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ActivityLog>> GetByIssue(int issueId)
    {
        return await _context.ActivityLog.Where(x => x.EntityType == "Issue" && x.EntityId == issueId.ToString()).ToListAsync();
    }

    public async Task<IEnumerable<ActivityLog>> GetByProject(int projectId)
    {
        return await _context.ActivityLog.Where(x => x.EntityType == "Project" && x.EntityId == projectId.ToString()).ToListAsync();
    }
}