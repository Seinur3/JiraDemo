using WebApplication3.DTO;

namespace WebApplication3.Service;

public interface IProjectService
{
   Task <ProjectDTO> CreateProjectAsync(ProjectCreateDTO project, int ownerId);
   Task <IEnumerable<ProjectDTO>> GetAllAsync(int ownerId);
   Task<ProjectDTO> GetByIdAsync(int id, int ownerId);
   Task RemoveMemberAsync(int userId, int projectId, int ownerId);
   Task AddMemberAsync(int userId, int projectId, int ownerId);
   Task RemoveProjectAsync(int projectId,  int userId);
}