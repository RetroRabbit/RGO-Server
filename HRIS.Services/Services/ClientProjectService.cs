using HRIS.Models;
using HRIS.Services.Interfaces;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services
{
    public class ClientProjectService : IClientProjectService
    {
        private readonly IUnitOfWork _db;

        public ClientProjectService(IUnitOfWork db)
        {
            _db = db;
        }

        public async Task<ClientProjectsDto> CreateClientProject(ClientProjectsDto clientProjectsDto)
        {
            var exists = await CheckIfExists(clientProjectsDto.Id);

            if (exists)
            {
                throw new CustomException("Client Project Already Exists");
            }

            var createdProject = await _db.ClientProject.Add(new ClientProject(clientProjectsDto));
            return createdProject.ToDto();
        }

        public async Task<ClientProjectsDto> DeleteClientProject(int id)
        {
            return (await _db.ClientProject.Delete(id)).ToDto();
        }

        public async Task<ClientProjectsDto?> GetClientProjectById(int id)
        {
            var existingClientProject = await _db.ClientProject.GetById(id);

            return existingClientProject == null
                ? throw new CustomException("Client Project Not Found")
                : existingClientProject.ToDto();
        }

        public async Task<List<ClientProjectsDto>> GetAllClientProjects()
        {
            return (await _db.ClientProject.GetAll()).Select(x => x.ToDto()).ToList();
        }

        public async Task<ClientProjectsDto> UpdateClientProject(ClientProjectsDto clientProjectsDto)
        {
            if (clientProjectsDto == null)
                throw new CustomException(nameof(clientProjectsDto));

            var clientProjectFound = await _db.ClientProject.FirstOrDefault(q => q.Id == clientProjectsDto.Id);

            if (clientProjectFound == null)
                throw new CustomException($"No client Project found with ID {clientProjectsDto.Id}.");

            var clientProject = new ClientProject(clientProjectsDto);
            var updatedClientProjectDto = await _db.ClientProject.Update(clientProject);

            return updatedClientProjectDto.ToDto();
        }

        public async Task<bool> CheckIfExists(int id)
        {
            if (id == 0)
                return false;

            var exists = await _db.ClientProject.GetById(id);
            return exists != null;
        }
    }
}
