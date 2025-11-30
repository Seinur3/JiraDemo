namespace WebApplication3.DTO;

public record CommentDto(int Id, int IssueId, int AuthorId, string Text, DateTime CreatedAt);

public record CommentCreateDTO(int IssueId, int AuthorId, string Text, DateTime CreatedAt);
