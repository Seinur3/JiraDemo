using WebApplication3.Models;

namespace WebApplication3.Service;

public interface IActivityService
{
    Task LogAsync(int? userId, string entityType, string entityId, string action, string? oldValue = null, string? newValue = null);
    Task<IEnumerable<ActivityLog>> GetByIssue(int issueId);
    Task<IEnumerable<ActivityLog>> GetByProject(int projectId);
}