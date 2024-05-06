using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Interfaces
{
    public interface IClientProjectService
    {
        Task<List<ClientProject>> GetAllClientProject();
        Task<ClientProject?> GetClientProject(int id);
        Task<ClientProject> CreateClientProject(ClientProject clientProject);
        Task UpdateClientProject(ClientProject clientProject);
        Task DeleteClientProject(int id);
    }
}
