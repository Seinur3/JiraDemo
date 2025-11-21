using WebApplication3.Models;

namespace WebApplication3.DTO;


    public record IssueCreateDTO( string title, string? description, IssueType IssueType, IssuePriority Priority, int? AssigneeId);
    public record IssueUpdateDTO( string? title, string? description, IssueType? IssueType, IssuePriority? Priority, int? AssigneeId);
    public record IssueDto(int id, int projectId, string title, string? description, IssueType? IssueType, IssuePriority? Priority, int? AssigneeId, int reporterId, DateTime createAt, DateTime updateAt);
