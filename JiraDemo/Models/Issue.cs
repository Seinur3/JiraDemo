using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models;

public enum IssueType
{
    Task,
    Bug,
    Story
}

public enum IssuePriority
{
    Low,
    Medium,
    High,
    Critical
}

public enum IssueStatus
{
    Open,
    InProgress,
    Review,
    Testing,
    Done
}
public class Issue
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public Project? Project { get; set; }
    [Required] public string Title { get; set; } = default!;
    public string Description { get; set; }
    public IssueType IssueType { get; set; } = IssueType.Task;
    public IssuePriority IssuePriority { get; set; } = IssuePriority.Medium;
    public IssueStatus IssueStatus { get; set; } = IssueStatus.Open;
    public int? AssigneeId { get; set; }
    public User? Assignee { get; set; }
    public int ReporterId { get; set; }
    public User? Reporter { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }
    public ICollection<Comment> Comments { get; set; }
    public ICollection<Attachment> Attachments { get; set; }
    
}