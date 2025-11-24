namespace WebApplication3.Service;

public interface IAttachmentService
{
    Task<string> SaveFile(int issueId, string fileName, string contentType, byte[] data);
    Task Delete ( int attachmentId);
}