using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models;

public class Comment
{
    public int Id { get; set; }
    public int IssuedId { get; set; }
    public Issue? Issue { get; set; }
    public int AuthorId { get; set; }
    public User? Author { get; set; }
    [Required] public string Text { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}