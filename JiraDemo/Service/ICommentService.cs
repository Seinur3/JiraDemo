using WebApplication3.DTO;
using WebApplication3.Models;

namespace WebApplication3.Service;

public interface ICommentService
{
    Task<CommentDto> CreateCommentAsync(CommentCreateDTO dto);
    Task RemoveCommentAsync(int id, int authorId, Role role);
    Task<IEnumerable<CommentDto>> GetAllCommentsAsync(int issueId);
}