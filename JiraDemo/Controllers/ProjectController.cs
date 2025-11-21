using Microsoft.AspNetCore.Mvc;
using WebApplication3.DTO;
using WebApplication3.Service;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication3.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }
    private int OwnerId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

    [HttpPost("create")]
    public async Task<IActionResult> CreateProjectAsync(ProjectCreateDTO projectCreateDto)
    {
        try
        {
            var res = await _projectService.CreateProjectAsync(projectCreateDto, OwnerId);
            return Ok(res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
        
    }
    [HttpGet("all")]
    
    public async Task<IActionResult> GetAllProjectsAsync()
    {
        try
        {
            var res = await _projectService.GetAllAsync(OwnerId);
            return Ok(res);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("id")]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        try
        {
            var res = await _projectService.GetByIdAsync(id);
            return Ok(res);
        }
        catch  (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("remove")]
    public async Task<IActionResult> RemoveMemberAsync(int userId, int projectId)
    {
        try
        {
            await _projectService.RemoveMemberAsync(userId, projectId);
            return Ok("removed");
        }
        catch  (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> AddMemberAsync(int userId, int projectId)
    {
        try
        {
            await _projectService.AddMemberAsync(userId, projectId);
            return Ok("added");
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("delete/{projectId:int}")]
    public async Task<IActionResult> RemoveProjectAsync(int projectId)
    {
        try
        {
            await _projectService.RemoveProjectAsync(projectId, OwnerId);
            return Ok("removed");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
}