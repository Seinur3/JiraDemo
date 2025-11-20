using WebApplication3.DTO;
using WebApplication3.Models;

namespace WebApplication3.Service;

public interface IAuthService
{
    Task<AuthResultDto> RegistrAsync(RegisterDTO register);
    Task<AuthResultDto> LoginAsync(LoginDTO login);
}