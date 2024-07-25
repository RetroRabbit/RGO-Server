using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services
{
    public class ClientProjectService : IClientProjectService
    {
        private readonly IUnitOfWork _db;
        private readonly AuthorizeIdentity _identity;

        public ClientProjectService(IUnitOfWork db, AuthorizeIdentity identity)
        {
            _db = db;
            _identity = identity;
        }

        public async Task<ClientProjectsDto> CreateClientProject(ClientProjectsDto clientProjectsDto)
        {
            var exists = await CheckIfExists(clientProjectsDto.Id);

            if (exists)
                throw new CustomException("Client Project Already Exists");

            if(_identity.IsSupport == false && clientProjectsDto.EmployeeId == _identity.EmployeeId)
                throw new CustomException("Unauthorized Access.");

            var createdProject = await _db.ClientProject.Add(new ClientProject(clientProjectsDto));
            return createdProject.ToDto();
        }

        public async Task<ClientProjectsDto> DeleteClientProject(int id)
        {
            var exists = await CheckIfExists(id);

            if (!exists)
                throw new CustomException("Client Project Does not Exist");

            if (_identity.IsSupport == false)
                throw new CustomException("Unauthorized Access.");

            return (await _db.ClientProject.Delete(id)).ToDto();
        }

        public async Task<ClientProjectsDto?> GetClientProjectById(int id)
        {
            if (_identity.IsSupport == false)
                throw new CustomException("Unauthorized Access.");

            var existingClientProject = await _db.ClientProject.GetById(id);

            return existingClientProject?.ToDto();
        }

        public async Task<List<ClientProjectsDto>> GetAllClientProjects()
        {
            return (await _db.ClientProject.GetAll()).Select(x => x.ToDto()).ToList();
        }

        public async Task<ClientProjectsDto> UpdateClientProject(ClientProjectsDto clientProjectsDto)
        {
            var exists = await CheckIfExists(clientProjectsDto.Id);

            if (!exists)
                throw new CustomException($"No client Project found with ID {clientProjectsDto.Id}.");

            if (_identity.IsSupport == false && clientProjectsDto.EmployeeId == _identity.EmployeeId)
                throw new CustomException("Unauthorized Access.");

            var clientProject = new ClientProject(clientProjectsDto);
            var updatedClientProjectDto = await _db.ClientProject.Update(clientProject);

            return updatedClientProjectDto.ToDto();
        }
   
        public async Task<bool> CheckIfExists(int id)
        {
            return await _db.WorkExperience.Any(x => x.Id == id);
        }
    }
}
