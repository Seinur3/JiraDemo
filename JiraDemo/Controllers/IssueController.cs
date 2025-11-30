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
    private readonly IActivityService _activity;
    public IssueController(IIssueService issueService, IActivityService activity)
    {
        _issueService = issueService;
        _activity = activity;
    }
    private int reporterId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

    [HttpPost]
    public async Task<IActionResult> CreateIssueAsync( [FromBody] IssueCreateDTO issueCreateDto, int projectId)
    {
        try
        {
            var res = await _issueService.CreateIssueAsync(issueCreateDto, projectId, reporterId);
            await _activity.LogAsync(reporterId, "Issue", res.id.ToString(), "Created");
            //return CreatedAtAction(nameof(CreateIssueAsync), res);
            return CreatedAtAction(
                nameof(GetIssueById),
                new { projectId = projectId, issueId = res.id },
                res);
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

    [HttpGet("{issueId:int}")]
    public async Task<IActionResult> GetIssueById(int projectId, int issueId)
    {
        var res = await _issueService.GetIssueAsync(issueId);
        return Ok(res);
    }

    [HttpPut("{issueId}")]
    public async Task<IActionResult> UpdateIssueAsync(int projectId, int issueId, [FromBody] IssueUpdateDTO issueUpdateDto)
    {
        await _issueService.UpdateIssueAsync(issueUpdateDto, issueId);
        await _activity.LogAsync(reporterId, "Issue", issueId.ToString(), "Updated");
        return NoContent();
    }

    [HttpPatch("{issueId}")]
    public async Task<IActionResult> ChangeIssueStatusAsync(int projectId, int issueId, [FromBody] string newStatus)
    {
        await _issueService.ChangeIssueStatusAsync(issueId, newStatus);
        await _activity.LogAsync(reporterId, "Issue", issueId.ToString(), "StatusUpdated", newValue:newStatus);
        return NoContent();
    }

    [HttpDelete("{issueId}")]
    public async Task<IActionResult> DeleteIssueAsync(int issueId, int projectId)
    {
        await _issueService.DeleteIssueAsync(issueId);
        await _activity.LogAsync(reporterId, "Issue", issueId.ToString(), "Deleted");
        return NoContent();
        
    }

    [HttpPatch("{issueId}/assignee/{asigneeId}")]
    public async Task<IActionResult> AssigneeToIssue(int issueId, int asigneeId, int projectId)
    {
        await _issueService.AsignAsigneeAsync(issueId, asigneeId);
        await _activity.LogAsync(reporterId, "Issue", issueId.ToString(), "NewAsignee");
        return NoContent();
    }
    

}