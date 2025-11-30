using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.DTO;
using WebApplication3.Models;

namespace WebApplication3.Service;

public class CommentService : ICommentService
{
    
    private readonly ApplicationDbContext _context;

    public CommentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CommentDto> CreateCommentAsync(CommentCreateDTO dto)
    {
        var comment = new Comment
        {
            AuthorId = dto.AuthorId,
            IssueId = dto.IssueId,
            Text = dto.Text,
        };
        await _context.Comment.AddAsync(comment);
        await _context.SaveChangesAsync();
        return new CommentDto(comment.Id, comment.IssueId, comment.AuthorId,  comment.Text, comment.CreatedAt);
    }

    public async Task RemoveCommentAsync(int id, int authorId, Role role)
    {
        var comment = await _context.Comment.FindAsync(id);
        if (comment == null) throw new Exception("Comment not found");
        if (comment.AuthorId == authorId || role == Role.Admin)
        {
            _context.Comment.Remove(comment);
        }
        else
        {
            throw new Exception("Only admin or author can delete comments");
        }
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CommentDto>> GetAllCommentsAsync(int issueId)
    {
        var comment = await _context.Comment.Include(c => c.Author).Where(c => c.IssueId == issueId).ToListAsync();
        return comment.Select(c=> new CommentDto(c.Id,c.IssueId, c.AuthorId, c.Text, c.CreatedAt));
    }
}