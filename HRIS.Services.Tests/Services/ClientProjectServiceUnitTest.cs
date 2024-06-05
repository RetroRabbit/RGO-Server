using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using System.Linq.Expressions;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class ClientProjectServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;
    private readonly ClientProjectService _clientProjectService;
    private readonly ClientProjectsDto _clientProjectsDto;

    public ClientProjectServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _errorLoggingServiceMock = new Mock<IErrorLoggingService>();
        _clientProjectService = new ClientProjectService(_dbMock.Object, _errorLoggingServiceMock.Object);
        _clientProjectsDto = new ClientProjectsDto
        {
            Id = 1,
            EmployeeId = 1,
            ClientName = "ClientName",
            ProjectName = "ProjectName",
            EndDate = DateTime.Now,
            StartDate = DateTime.Now,
            ProjectURL = "ProjectURL"
        };
    }

    [Fact]
    public async Task GetAllClientProjectsTest()
    {
        var clientProject = new List<ClientProjectsDto> { _clientProjectsDto };

        _dbMock.Setup(ex => ex.ClientProject.GetAll(null)).Returns(Task.FromResult(clientProject));
        var result = await _clientProjectService.GetAllClientProjects();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(clientProject, result);
    }

    [Fact]
    public async Task GetClientProjectById_ThrowsExceptionWhenNotFound()
    {
        _dbMock.Setup(ex => ex.ClientProject.GetById(1)).ReturnsAsync((ClientProjectsDto)null);
        _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>()))
            .Returns(new Exception("Client project not found"));

        var exception = await Assert.ThrowsAsync<Exception>(() => _clientProjectService.GetClientProjectById(1));

        Assert.Equal("Client project not found", exception.Message);
    }

    [Fact]
    public async Task GetClientProjectById_ReturnsProjectWhenFound()
    {
        _dbMock.Setup(ex => ex.ClientProject.GetById(1)).ReturnsAsync(_clientProjectsDto);

        var result = await _clientProjectService.GetClientProjectById(1);

        Assert.NotNull(result);
        Assert.Equal(_clientProjectsDto.Id, result.Id);
    }

    [Fact]
    public async Task CreateClientProject_SuccessfullyCreatesProject()
    {
        _dbMock.Setup(ex => ex.ClientProject.GetById(1)).ReturnsAsync((ClientProjectsDto)null);
        _dbMock.Setup(ex => ex.ClientProject.Add(It.IsAny<ClientProject>())).ReturnsAsync(_clientProjectsDto);

        var result = await _clientProjectService.CreateClientProject(_clientProjectsDto);

        Assert.NotNull(result);
        Assert.Equal(_clientProjectsDto.Id, result.Id);
        Assert.Equal(_clientProjectsDto.ClientName, result.ClientName);
    }

    [Fact]
    public async Task CreateClientProject_ThrowsExceptionWhenProjectExists()
    {
        _dbMock.Setup(ex => ex.ClientProject.GetById(1)).ReturnsAsync(_clientProjectsDto);
        _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>()))
            .Returns(new Exception("Client Project already exists"));

        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _clientProjectService.CreateClientProject(_clientProjectsDto));

        Assert.Equal("Client Project already exists", exception.Message);
    }

    [Fact]
    public async Task DeleteClientProject_SuccessfullyDeletesProject()
    {
        _dbMock.Setup(ex => ex.ClientProject.Delete(1)).ReturnsAsync(_clientProjectsDto);

        var result = await _clientProjectService.DeleteClientProject(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task UpdateClientProject_ThrowsExceptionWhenNotFound()
    {
        _dbMock.Setup(ex => ex.ClientProject.FirstOrDefault(q => q.Id == _clientProjectsDto.Id)).ReturnsAsync((ClientProjectsDto)null);
        _errorLoggingServiceMock.Setup(x => x.LogException(It.IsAny<Exception>()))
            .Returns(new KeyNotFoundException($"No client Project found with ID {_clientProjectsDto.Id}."));

        var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _clientProjectService.UpdateClientProject(_clientProjectsDto));

        Assert.Equal($"No client Project found with ID {_clientProjectsDto.Id}.", exception.Message);
    }

    [Fact]
    public async Task UpdateClientProject_ThrowsArgumentNullException_WhenClientProjectsDtoIsNull()
    {
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() =>
         _clientProjectService.UpdateClientProject(null));

        Assert.Equal("Value cannot be null. (Parameter 'clientProjectsDto')", exception.Message);
    }

    [Fact]
    public async Task UpdateClientProject_ReturnsUpdatedClientProjectsDto_WhenClientProjectIsFound()
    {
        _dbMock.Setup(ex => ex.ClientProject.FirstOrDefault(It.IsAny<Expression<Func<ClientProject, bool>>>()))
            .ReturnsAsync(_clientProjectsDto);

        _dbMock.Setup(ex => ex.ClientProject.Update(It.IsAny<ClientProject>()))
            .ReturnsAsync(_clientProjectsDto);

        var result = await _clientProjectService.UpdateClientProject(_clientProjectsDto);

        Assert.NotNull(result);
        Assert.Equal(_clientProjectsDto.Id, result.Id);
    }

    [Fact]
    public async Task CheckIfExists_ReturnsTrue_WhenProjectExists()
    {
        _dbMock.Setup(ex => ex.ClientProject.GetById(1)).ReturnsAsync(_clientProjectsDto);

        var exists = await _clientProjectService.CheckIfExists(1);

        Assert.True(exists);
    }

    [Fact]
    public async Task CheckIfExists_ReturnsFalse_WhenProjectDoesNotExist()
    {
        _dbMock.Setup(ex => ex.ClientProject.GetById(1)).ReturnsAsync((ClientProjectsDto)null);

        var exists = await _clientProjectService.CheckIfExists(1);

        Assert.False(exists);
    }

    [Fact]
    public async Task CheckIfExists_ReturnsFalse_WhenIdIsZero()
    {
        var exists = await _clientProjectService.CheckIfExists(0);

        Assert.False(exists);
    }
}
