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

        public async Task<ClientProjectsDto> CreateClientProject(ClientProject clientProject)
        {
            var clientProjectDto = new ClientProjectsDto
            {
                Id = clientProject.Id,
                NameOfClient = clientProject.NameOfClient,
                ProjectName = clientProject.ProjectName,
                StartDate = clientProject.StartDate,
                EndDate = clientProject.EndDate,
                UploadProjectUrl = clientProject.UploadProjectUrl
            };

            var existingProjects = await _db.ClientProject.GetAll();
            bool projectExists = existingProjects.Exists(p => p.Id == clientProjectDto.Id);

            if (projectExists)
            {
                throw new InvalidOperationException("Client project already exists.");
            }

            var createdProject = await _db.ClientProject.Add(clientProject);
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
                var exception = new Exception("client project not found");
                throw _errorLoggingService.LogException(exception);
            }
            return clientProjectExist;
        }

        public async Task<List<ClientProjectsDto>> GetAllClientProject()
        {
            var dtos = await _db.ClientProject.GetAll();
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

        public async Task<ClientProjectsDto> UpdateClientProject(ClientProject clientProject)
        {
            var existingClientProject = await _db.ClientProject.Update(clientProject);
            if (existingClientProject == null)
            {
                var exception = new Exception("ClientProject not found");
                throw _errorLoggingService.LogException(exception);
            }

            return existingClientProject;
        }
    }
}
