using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models;

public enum Role
{
    Admin,
    User
}
public class User
{
    public int Id { get; set; }
    [Required]
    public string FullName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string PasswordHash { get; set; }

    public string Role { get; set; } = "User";
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  
}