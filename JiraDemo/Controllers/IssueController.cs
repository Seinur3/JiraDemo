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
    private int OwnerId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

    [HttpPost]
    public async Task<IActionResult> CreateIssueAsync(IssueCreateDTO issueCreateDto, int projectId)
    {
        try
        {
            var res = await _issueService.CreateIssueAsync(issueCreateDto, projectId, OwnerId);
            return CreatedAtAction(nameof(CreateIssueAsync), res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }

    [HttpGet]
    public async Task<IActionResult> Get(int projectId)
    {
        var res = await _issueService.GetAllIssuesAsync(projectId);
        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var res = await _issueService.GetIssueAsync(id);
        return Ok(res);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIssue(int id, [FromBody] IssueUpdateDTO issueUpdateDto)
    {
        await _issueService.UpdateIssueAsync(issueUpdateDto, id);
        return NoContent();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> ChangeStatus(int id, [FromBody] string status)
    {
        await _issueService.ChangeIssueStatusAsync(id, status);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _issueService.DeleteIssueAsync(id);
        return NoContent();
        
    }

    [HttpPatch("{issueid}/assignee/{id}")]
    public async Task<IActionResult> AssigneeToIssue(int id, int issueid)
    {
        await _issueService.AsignAsigneeAsync(issueid, id);
        return NoContent();
    }
    

}