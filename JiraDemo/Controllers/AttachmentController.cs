using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Service;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/issue/{issueId:int}/[controller]")]
[Authorize]

public class AttachmentController : ControllerBase
{
    private readonly ActivityService _activity;
    private readonly AttachmentService _attachment;
    
    public  AttachmentController(ActivityService activityService, AttachmentService attachmentService)
    {
        _activity = activityService;
        _attachment = attachmentService;
    }

    [HttpPost]
    public async Task<IActionResult> UploadAttachment(int issueId)
    {
        var file = Request.Form.Files.FirstOrDefault();
        if (file == null) return BadRequest(" null file");
        if (file.Length > 10*1024*1024) return BadRequest("file too big");
        var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var path = await _attachment.SaveFile(issueId, file.FileName, file.ContentType, ms.ToArray());
        await _activity.LogAsync(null, "Attachment", issueId.ToString(), "Upload", newValue: file.FileName);
        return Ok(path);
    }

    [HttpDelete("{attachmentId}")]
    public async Task<IActionResult> DeleteAttachment(int attachmentId, int issueId)
    {
        await _attachment.Delete(attachmentId);
        await _activity.LogAsync(null, "Attachment", issueId.ToString(), "Delete", newValue: "Deleted");
        return NoContent();
    }
}