using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Interfaces
{
    public interface IClientProjectService
    {
        Task<List<ClientProjectsDto>> GetAllClientProject();
        Task<ClientProjectsDto?> GetClientProject(int id);
        Task<ClientProjectsDto> CreateClientProject(ClientProject clientProject);
        Task <ClientProjectsDto>UpdateClientProject(ClientProject clientProject);
        Task <ClientProjectsDto>DeleteClientProject(int id);
    }
}
