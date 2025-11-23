using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.DTO;
using WebApplication3.Service;

namespace WebApplication3.Controllers;
[ApiController]
[Route("api/projects/{projectId}/[controller]")]
[Authorize]

public class IssueController : ControllerBase
{
    private readonly IIssueService _issueService;
    public IssueController(IIssueService issueService)
    {
        _issueService = issueService;
    }
    private int reporterId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

    [HttpPost]
    public async Task<IActionResult> CreateIssueAsync( [FromBody] IssueCreateDTO issueCreateDto, int projectId)
    {
        try
        {
            var res = await _issueService.CreateIssueAsync(issueCreateDto, projectId, reporterId);
            return CreatedAtAction(nameof(CreateIssueAsync), res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }

    [HttpGet]
    public async Task<IActionResult> GetAllIssuesAsync(int projectId)
    {
        var res = await _issueService.GetAllIssuesAsync(projectId);
        return Ok(res);
    }

    [HttpGet("{issueId}")]
    public async Task<IActionResult> GetIssueById(int issueId)
    {
        var res = await _issueService.GetIssueAsync(issueId);
        return Ok(res);
    }

    [HttpPut("{issueId}")]
    public async Task<IActionResult> UpdateIssueAsync(int issueId, [FromBody] IssueUpdateDTO issueUpdateDto)
    {
        await _issueService.UpdateIssueAsync(issueUpdateDto, issueId);
        return NoContent();
    }

    [HttpPatch("{issueId}")]
    public async Task<IActionResult> ChangeIssueStatusAsync(int issueId, [FromBody] string newStatus)
    {
        await _issueService.ChangeIssueStatusAsync(issueId, newStatus);
        return NoContent();
    }

    [HttpDelete("{issueId}")]
    public async Task<IActionResult> DeleteIssueAsync(int issueId)
    {
        await _issueService.DeleteIssueAsync(issueId);
        return NoContent();
        
    }

    [HttpPatch("{issueId}/assignee/{asigneeId}")]
    public async Task<IActionResult> AssigneeToIssue(int asigneeId, int issueId)
    {
        await _issueService.AsignAsigneeAsync(issueId, asigneeId);
        return NoContent();
    }
    

}