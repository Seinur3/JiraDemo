namespace WebApplication3.Models;

public class ActivityLog
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string EntityType { get; set; } = default!;
    public string EntityId { get; set; } = default!;
    public string Action { get; set; } = default!;
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;
    
}