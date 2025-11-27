using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models;

public class Project
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string? Description { get; set; } 
    [Required]
    public int OwnerId { get; set; }
    public User? Owner { get; set; }
    public DateTime CreatedAt { get; set; }  = DateTime.UtcNow;
    public ICollection<ProjectMember>? Members { get; set; }  
    public ICollection<Issue>? Issues { get; set; }
}
