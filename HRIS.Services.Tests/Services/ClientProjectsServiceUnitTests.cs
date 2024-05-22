using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;
    public class ClientProjectsServiceUnitTests
    {
        private Mock<IClientProjectService> _mockClientProjectService;
        private readonly ClientProjectService _clientProjectService;
        private readonly ClientProjectsDto _clientProjectsDto;
        private readonly Mock<IUnitOfWork> _dbMock;
        private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;

        public ClientProjectsServiceUnitTests()
        {
          _dbMock = new Mock<IUnitOfWork> ();
          _mockClientProjectService = new Mock<IClientProjectService> ();
          _errorLoggingServiceMock = new Mock<IErrorLoggingService> ();
          _clientProjectService = new ClientProjectService(_dbMock.Object, _errorLoggingServiceMock.Object);
          _clientProjectsDto = new ClientProjectsDto
          {
                Id = 1,
                EmployeeId = 1,
                ClientName = "string",
                ProjectName = "string",
                EndDate = DateTime.Now,
                StartDate = DateTime.Now,
                ProjectURL = "string",
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
                Assert.Equal(clientProject[i].ClientName, result[i].ClientName);
            }
        }

        [Fact]
        public async Task GetClientProject_ReturnsCorrectProject()
        {
            var clientProjectsDto = new ClientProjectsDto
            {
                Id = 1
            };
            _dbMock.Setup(ex => ex.ClientProject.GetById(1)).ReturnsAsync(clientProjectsDto);
            var result = await _clientProjectService.GetClientProject(1);
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
        }

        [Fact]
        public async Task UpdateClientProject_SuccessfullyUpdatesProject()
        {
            var clientProjectsDto = new ClientProjectsDto { };
            var clientProject = new ClientProject(clientProjectsDto);

            _dbMock.Setup(x => x.ClientProject.Update(It.IsAny<ClientProject>()))
                   .ReturnsAsync(clientProjectsDto);

            var result = await _clientProjectService.UpdateClientProject(clientProjectsDto);

            Assert.NotNull(result);
            Assert.Equal(clientProjectsDto.Id, result.Id);
            Assert.Equal(clientProjectsDto.ProjectName, result.ProjectName);
            _dbMock.Verify(x => x.ClientProject.Update(It.IsAny<ClientProject>()), Times.Once);
        }

        [Fact]
        public async Task CreateClientProject_ThrowsExceptionWhenProjectAlreadyExists()
        {
            var clientProject = new ClientProject(_clientProjectsDto);
            var existingProjects = new List<ClientProjectsDto> { _clientProjectsDto };
            _dbMock.Setup(ex => ex.ClientProject.GetAll(null)).ReturnsAsync(existingProjects);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await _clientProjectService.CreateClientProject(_clientProjectsDto);
            });
            Assert.Equal("Client project already exists.", exception.Message);
        }

        [Fact]
        public async Task DeleteClientProject_SuccessfullyDeletesProject()
        {
            var deletedClientProject = new ClientProjectsDto { Id = 1};

            _dbMock.Setup(db => db.ClientProject.Delete(1)).ReturnsAsync(deletedClientProject);

            var result = await _clientProjectService.DeleteClientProject(1);
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            _dbMock.Verify(db => db.ClientProject.Delete( 1), Times.Once);
        }

        [Fact]
        public async Task GetClientProject_ThrowsExceptionWhenProjectNotFound()
        {
            _dbMock.Setup(db => db.ClientProject.GetById(99)).ReturnsAsync((ClientProjectsDto)null);

            var exception = await Assert.ThrowsAsync<NullReferenceException>(async () =>
            {
                await _clientProjectService.GetClientProject(99);
            });
            Assert.Contains("Object reference not set to an instance", exception.Message);
        }

        [Fact]
        public async Task CreateClientProject_AddsProjectSuccessfully_WhenNoExistingProject()
        {
            var clientProject = new ClientProject { };
            var emptyProjects = new List<ClientProjectsDto>();
            _dbMock.Setup(ex => ex.ClientProject.GetAll(null)).ReturnsAsync(emptyProjects);
            _dbMock.Setup(ex => ex.ClientProject.Add(It.IsAny<ClientProject>()))
                   .ReturnsAsync(_clientProjectsDto);

            var result = await _clientProjectService.CreateClientProject(_clientProjectsDto);

            Assert.NotNull(result);
            Assert.Equal(_clientProjectsDto.Id, result.Id);
            Assert.Equal(_clientProjectsDto.ClientName, result.ClientName);
            Assert.Equal(_clientProjectsDto.ProjectName, result.ProjectName);
            _dbMock.Verify(db => db.ClientProject.Add(It.IsAny<ClientProject>()), Times.Once);
        }
    }
