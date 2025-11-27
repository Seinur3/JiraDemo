using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Service;

public class AttachmentService : IAttachmentService
{
    private readonly ApplicationDbContext _context;
    private readonly IWebHostEnvironment _environment;
    public AttachmentService(ApplicationDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }
    
    public async Task<string> SaveFile(int issueId, string fileName, string contentType, byte[] data)
    {
        if( !_context.Issues.Any(x => x.Id == issueId)) throw new Exception("Issue Not Found");
        var folder = Path.Combine(_environment.WebRootPath?? "wwwroot", "Uploads", issueId.ToString());
        //wwwroot/Uploads/issueId/FileName
        Directory.CreateDirectory(folder);
        var filePath = Path.Combine(folder, fileName);
        await File.WriteAllBytesAsync(filePath, data);

        var a = new Attachment
        {
            IssueId = issueId,
            FilePath = filePath,
            FileName = fileName,
            ContentType = contentType
        };
        _context.Attachment.Add(a);
        await _context.SaveChangesAsync();
        return filePath;
    }

    public async Task Delete(int attachmentId)
    {
        var  attachment = await _context.Attachment.FindAsync(attachmentId);
        if(attachment == null) throw new Exception("Attachment Not Found");
        
        if(File.Exists(attachment.FilePath)) File.Delete(attachment.FilePath);
        
        _context.Attachment.Remove(attachment);
        await _context.SaveChangesAsync();
        
    }
}