using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models;

public class Attachment
{
    public int Id { get; set; }
    public int IssueId { get; set; }
    public Issue? Issue { get; set; }
    [Required] public string FileName { get; set; } = default!;
    [Required] public string ContentType { get; set; }= default!;
    [Required] public string FilePath { get; set; }= default!;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}