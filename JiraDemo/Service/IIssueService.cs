using WebApplication3.DTO;

namespace WebApplication3.Service;

public interface IIssueService
{
    Task<IssueDto> CreateIssueAsync(IssueCreateDTO issueCreateDTO, int projectId, int reporterId);
    Task<IEnumerable<IssueDto>> GetAllIssuesAsync(int projectId);
    Task<IssueDto> GetIssueAsync(int issueId);
    Task UpdateIssueAsync(IssueUpdateDTO issueUpdateDTO, int issueId);
    Task ChangeIssueStatusAsync(int issueId, string status);
    Task DeleteIssueAsync(int issueId);
    Task AsignAsigneeAsync(int issueId, int asigneeId);
}