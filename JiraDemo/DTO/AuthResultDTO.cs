namespace WebApplication3.DTO;

public record AuthResultDto(string token, string tokenType = "Bearer");
public record LoginDTO(string email, string password);
public record RegisterDTO(string fullname, string email, string password);