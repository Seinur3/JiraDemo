namespace WebApplication3.DTO;

public record ProjectCreateDTO( string name, string description);
public record ProjectDTO (int id, string name, string description, int ownerId, DateTime createAt);
public record MemberProjectDTO (int userId);