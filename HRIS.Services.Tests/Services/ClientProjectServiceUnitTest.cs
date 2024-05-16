using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services
{
    public class ClientProjectServiceUnitTest
    {
        private Mock<IClientProjectService> _mockClientProjectService;
        private readonly ClientProjectService _clientProjectService;
        private readonly ClientProjectsDto _clientProjectsDto;
        private readonly Mock<IUnitOfWork> _dbMock;
        private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;

        public ClientProjectServiceUnitTest()
        {
          _dbMock = new Mock<IUnitOfWork> ();
            _mockClientProjectService = new Mock<IClientProjectService> ();
           _errorLoggingServiceMock = new Mock<IErrorLoggingService> ();
            _clientProjectService = new ClientProjectService(_dbMock.Object, _errorLoggingServiceMock.Object);
            _clientProjectsDto = new ClientProjectsDto
            {
                Id = 1,
                NameOfClient = "string",
            };
        }

        [Fact]
        public async Task GetAllClientProjectTest_ReturnsListOfClientProjects()
        {
            var clientProject = new List<ClientProjectsDto> { _clientProjectsDto };
            _dbMock.Setup(ex => ex.ClientProject.GetAll(null)).ReturnsAsync(clientProject);
            var result = await _clientProjectService.GetAllClientProject();

            Assert.NotNull(result);
            Assert.IsType<List<ClientProjectsDto>>(result);
            Assert.Equal(clientProject.Count, result.Count);

            for (int i = 0; i < clientProject.Count; i++)
            {
                Assert.Equal(clientProject[i].Id, result[i].Id);
                Assert.Equal(clientProject[i].NameOfClient, result[i].NameOfClient);
            }
        }
        [Fact]
        public async Task GetClientProject_ReturnsCorrectProject()
        {
            int projectId = 1;
            var clientProject = new ClientProjectsDto
            {
                Id = projectId,
            };
            _dbMock.Setup(ex => ex.ClientProject.GetById(projectId)).ReturnsAsync(clientProject);

            var result = await _clientProjectService.GetClientProject(projectId);

            Assert.NotNull(result);
            Assert.Equal(projectId, result.Id);
        }

        [Fact]
        public async Task UpdateClientProject_SuccessfullyUpdatesProject()
        {
            var clientProjectToUpdate = new ClientProject
            {
                Id = 1,
            };
            var updatedClientProject = new ClientProjectsDto
            {
                Id = 1,
            };
            _dbMock.Setup(ex => ex.ClientProject.Update(clientProjectToUpdate)).ReturnsAsync(updatedClientProject);
            var result = await _clientProjectService.UpdateClientProject(clientProjectToUpdate);

            Assert.NotNull(result);
            Assert.Equal(clientProjectToUpdate.Id, result.Id);
        }

        [Fact]
        public async Task UpdateClientProject_ThrowsException_WhenProjectNotFound()
        {
            var clientProjectToUpdate = new ClientProject
            {
                Id = 1,
            };

            _dbMock.Setup(ex => ex.ClientProject.Update(clientProjectToUpdate)).ReturnsAsync((ClientProjectsDto)null);

            var exception = await Assert.ThrowsAsync<NullReferenceException>(() => _clientProjectService.UpdateClientProject(clientProjectToUpdate));
            Assert.Contains("Object reference not set to an instance", exception.Message);
        }

        [Fact]
        public async Task CreateClientProject_ThrowsExceptionWhenProjectAlreadyExists()
        {
            var clientProject = new ClientProject
            {
                Id = 1,
                NameOfClient = "Client1",
                ProjectName = "Project1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                UploadProjectUrl = "string"
            };

            var existingProjects = new List<ClientProjectsDto> { _clientProjectsDto };
            _dbMock.Setup(ex => ex.ClientProject.GetAll(null)).ReturnsAsync(existingProjects);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _clientProjectService.CreateClientProject(clientProject);
            });
            Assert.Equal("Client project already exists.", exception.Message);
        }

        [Fact]
        public async Task DeleteClientProject_SuccessfullyDeletesProject()
        {
            int projectIdToDelete = 1;
            var deletedClientProject = new ClientProjectsDto
            {
                Id = projectIdToDelete,
            };
            _dbMock.Setup(db => db.ClientProject.Delete(projectIdToDelete)).ReturnsAsync(deletedClientProject);

            var result = await _clientProjectService.DeleteClientProject(projectIdToDelete);

            Assert.NotNull(result);
            Assert.Equal(projectIdToDelete, result.Id);
        }

        [Fact]
        public async Task GetClientProject_ThrowsExceptionWhenProjectNotFound()
        {
            int nonExistentProjectId = 999;
            _dbMock.Setup(db => db.ClientProject.GetById(nonExistentProjectId)).ReturnsAsync((ClientProjectsDto)null);

            var exception = await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _clientProjectService.GetClientProject(nonExistentProjectId);
            });
            Assert.Contains("Object reference not set to an instance", exception.Message);
         
        }

        [Fact]
        public async Task CreateClientProject_AddsProjectSuccessfully_WhenNoExistingProject()
        {
            var clientProject = new ClientProject
            {
                Id = 2,
                NameOfClient = "Client2",
                ProjectName = "Project2",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(7),
                UploadProjectUrl = "url2"
            };

            var emptyProjects = new List<ClientProjectsDto>();
            _dbMock.Setup(ex => ex.ClientProject.GetAll(null)).ReturnsAsync(emptyProjects);

            var expectedDto = new ClientProjectsDto
            {
                Id = clientProject.Id,
                NameOfClient = clientProject.NameOfClient,
                ProjectName = clientProject.ProjectName,
                StartDate = clientProject.StartDate,
                EndDate = clientProject.EndDate,
                UploadProjectUrl = clientProject.UploadProjectUrl
            };

            _dbMock.Setup(ex => ex.ClientProject.Add(It.IsAny<ClientProject>()))
                   .ReturnsAsync(expectedDto);

            var result = await _clientProjectService.CreateClientProject(clientProject);

            Assert.NotNull(result);
            Assert.Equal(expectedDto.Id, result.Id);
            Assert.Equal(expectedDto.NameOfClient, result.NameOfClient);
            Assert.Equal(expectedDto.ProjectName, result.ProjectName);
            _dbMock.Verify(db => db.ClientProject.Add(It.IsAny<ClientProject>()), Times.Once);
        }

    }
}
