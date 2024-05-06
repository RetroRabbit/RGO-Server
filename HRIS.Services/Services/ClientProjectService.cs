using HRIS.Models;
using HRIS.Services.Interfaces;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace HRIS.Services.Services
{
    public class ClientProjectService : IClientProjectService
    {
        private readonly IClientProjectRepository _clientProjectRepository;

        public ClientProjectService(IClientProjectRepository clientProjectRepository)
        {
            _clientProjectRepository = clientProjectRepository;
        }

        public Task<ClientProject> CreateClientProject(ClientProject clientProject)
        {
            throw new NotImplementedException();
        }

        public Task DeleteClientProject(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ClientProject>> GetAllClientProject()
        {
            throw new NotImplementedException();
        }

        public Task<ClientProject?> GetClientProject(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ClientProjectsDto>> GetClientProjectsDtos()
        {
            var dtos = await _clientProjectRepository.GetAll();
            var allClientProjects = dtos
                             .Select(dto => new ClientProjectsDto
                             {
                                 Id = dto.Id,
                                 NameOfClient = dto.NameOfClient,
                                 ProjectName = dto.ProjectName,
                                 StartDate = dto.StartDate,
                                 EndDate = dto.EndDate,
                                 UploadProjectUrl = dto.UploadProjectUrl
                             })
                             .ToList();
            return allClientProjects;
        }

        public Task UpdateClientProject(ClientProject clientProject)
        {
            throw new NotImplementedException();
        }
    }
}

