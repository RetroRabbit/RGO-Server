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

            var existingProjects = await _db.ClientProject.GetAll();
            bool projectExists = existingProjects.Exists(p => p.Id == clientProjectsDto.Id);

            if (projectExists)
            {
                throw new InvalidOperationException("Client project already exists.");
            }

            var createdProject = await _db.ClientProject.Add(new ClientProject(clientProjectsDto));
            return createdProject;
        }

        public async Task<ClientProjectsDto> DeleteClientProject(int id)
        {
            return await _db.ClientProject.Delete(id);
        }

        public async Task<ClientProjectsDto?> GetClientProject(int id)
        {
            var clientProjectExist = await _db.ClientProject.GetById(id);
            if (clientProjectExist == null)
            {
                var exception = new Exception("Client project not found");
                throw _errorLoggingService.LogException(exception);
            }
            return clientProjectExist;
        }

        public async Task<List<ClientProjectsDto>> GetAllClientProject()
        {
            return await _db.ClientProject.GetAll();
        }

        public async Task<ClientProjectsDto> UpdateClientProject(ClientProjectsDto clientProjectsDto)
        {
            var existingClientProject = await _db.ClientProject.Update(new ClientProject(clientProjectsDto));
            if (existingClientProject == null)
            {
                var exception = new Exception("InvalidOperationException");
                throw _errorLoggingService.LogException(exception);
            }
            return existingClientProject;
        }
    }
}
