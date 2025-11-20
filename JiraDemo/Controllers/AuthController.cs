using Microsoft.AspNetCore.Mvc;
using WebApplication3.Service;
using WebApplication3.DTO;

namespace WebApplication3.Controllers;
[ApiController]
[Route("api/[controller]")]

public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Registr(RegisterDTO registerDto)
    {
        try
        {
            var res = await _authService.RegistrAsync(registerDto);
            return Ok(res);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO loginDto)
    {
        try
        {
            var res = await _authService.LoginAsync(loginDto);
            return Ok(res);
        }
        catch(Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
}