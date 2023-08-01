using RGO.Domain.Models;


namespace RGO.Domain.Interfaces.Repository
{
    public interface IProjectsRepository
    {
        Task<bool> HasProjects(int userid);
        Task<List<ProjectsDto>> GetProjectsByUserId(int userid);
    }
}
