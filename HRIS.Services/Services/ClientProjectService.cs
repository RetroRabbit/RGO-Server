using HRIS.Models;
using HRIS.Services.Interfaces;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services
{
    public class ClientProjectService : IClientProjectService
    {
        private readonly IUnitOfWork _db;
        private readonly IErrorLoggingService _errorLoggingService;

        public ClientProjectService(IUnitOfWork db, IErrorLoggingService errorLoggingService)
        {
            _db = db;
            _errorLoggingService = errorLoggingService;
        }

        public async Task<ClientProjectsDto> CreateClientProject(ClientProjectsDto clientProjectsDto)
        {
            var exists = await CheckIfExists(clientProjectsDto.Id);

            if (exists)
            {
                var exception = new Exception("Client Project already exists");
                throw _errorLoggingService.LogException(exception);
            }

            var createdProject = await _db.ClientProject.Add(new ClientProject(clientProjectsDto));
            return createdProject;
        }

        public async Task<ClientProjectsDto> DeleteClientProject(int id)
        {
            return await _db.ClientProject.Delete(id);
        }

        public async Task<ClientProjectsDto?> GetClientProjectById(int id)
        {
            var existingClientProject = await _db.ClientProject.GetById(id);

            if (existingClientProject == null)
            {
                var exception = new Exception("Client project not found");
                throw _errorLoggingService.LogException(exception);
            }

            return existingClientProject;
        }

        public async Task<List<ClientProjectsDto>> GetAllClientProjects()
        {
            return await _db.ClientProject.GetAll();
        }

        public async Task<ClientProjectsDto> UpdateClientProject(ClientProjectsDto clientProjectsDto)
        {
            if (clientProjectsDto == null)
                throw new ArgumentNullException(nameof(clientProjectsDto));

            try
            {
                var clientProjectFound = await _db.ClientProject.FirstOrDefault(q => q.Id == clientProjectsDto.Id);

                if (clientProjectFound == null)
                {
                    throw new KeyNotFoundException($"No client Project found with ID {clientProjectsDto.Id}.");
                }

                ClientProject clientProject = new ClientProject(clientProjectsDto);
                var updatedClientProjectDto = await _db.ClientProject.Update(clientProject);

                return updatedClientProjectDto;

            }
            catch (Exception ex)
            {
                throw _errorLoggingService.LogException(ex);
            }
        }

        public async Task<bool> CheckIfExists(int id)
        {
            if (id == 0)
            {
                return false;
            }

            var exists = await _db.ClientProject.GetById(id);
            return exists != null;
        }
    }
}
