using Microsoft.AspNetCore.Mvc;
using WebApplication3.DTO;
using WebApplication3.Models;
using WebApplication3.Service;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CommentController : ControllerBase
{
    private readonly ICommentService _comment;

    public CommentController(ICommentService comment)
    {
        _comment = comment;
    }

    [HttpPost("createComment")]
    public async Task<IActionResult> CreateCommentAsync([FromBody] CommentCreateDTO newComment)
    {
        if (newComment == null) return BadRequest("Comment is null");
        try
        {
            var comment = await _comment.CreateCommentAsync(newComment);
            return Ok(comment);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(int id, int authorId, Role role)
    {
        try
        {
            await _comment.RemoveCommentAsync(id, authorId, role);
            return Ok("removed");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Get(int issueId)
    {
        var comments = await _comment.GetAllCommentsAsync(issueId);
        return Ok(comments);
    }

}