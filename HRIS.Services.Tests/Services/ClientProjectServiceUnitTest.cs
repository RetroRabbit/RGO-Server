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
    private readonly ClientProjectService _clientProjectService;
    private readonly ClientProject _clientProject;

    public ClientProjectServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _clientProjectService = new ClientProjectService(_dbMock.Object);
        _clientProject = new ClientProject
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
        var clientProjects = new List<ClientProject> { _clientProject };

        _dbMock.Setup(ex => ex.ClientProject.GetAll(null)).ReturnsAsync(clientProjects);
        var result = await _clientProjectService.GetAllClientProjects();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equivalent(clientProjects.Select(x => x.ToDto()), result);
    }

    [Fact]
    public async Task GetClientProjectById_ThrowsExceptionWhenNotFound()
    {
        _dbMock.Setup(ex => ex.ClientProject.GetById(1)).ReturnsAsync((ClientProject)null!);

        await Assert.ThrowsAsync<CustomException>(() => _clientProjectService.GetClientProjectById(1));
    }

    [Fact]
    public async Task GetClientProjectById_ReturnsProjectWhenFound()
    {
        _dbMock.Setup(ex => ex.ClientProject.GetById(1)).ReturnsAsync(_clientProject);

        var result = await _clientProjectService.GetClientProjectById(1);

        Assert.NotNull(result);
        Assert.Equal(_clientProject.Id, result.Id);
    }

    [Fact]
    public async Task CreateClientProject_SuccessfullyCreatesProject()
    {
        _dbMock.Setup(ex => ex.ClientProject.GetById(1)).ReturnsAsync((ClientProject)null!);
        _dbMock.Setup(ex => ex.ClientProject.Add(It.IsAny<ClientProject>())).ReturnsAsync(_clientProject);

        var result = await _clientProjectService.CreateClientProject(_clientProject.ToDto());

        Assert.NotNull(result);
        Assert.Equal(_clientProject.Id, result.Id);
        Assert.Equal(_clientProject.ClientName, result.ClientName);
    }

    [Fact]
    public async Task CreateClientProject_ThrowsExceptionWhenProjectExists()
    {
        _dbMock.Setup(ex => ex.ClientProject.GetById(1)).ReturnsAsync(_clientProject);

        await Assert.ThrowsAsync<CustomException>(() => _clientProjectService.CreateClientProject(_clientProject.ToDto()));
    }

    [Fact]
    public async Task DeleteClientProject_SuccessfullyDeletesProject()
    {
        _dbMock.Setup(ex => ex.ClientProject.Delete(1)).ReturnsAsync(_clientProject);

        var result = await _clientProjectService.DeleteClientProject(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task UpdateClientProject_ThrowsExceptionWhenNotFound()
    {
        _dbMock.Setup(ex => ex.ClientProject.FirstOrDefault(q => q.Id == _clientProject.Id)).ReturnsAsync((ClientProject)null!);

        await Assert.ThrowsAsync<CustomException>(() =>
            _clientProjectService.UpdateClientProject(_clientProject.ToDto()));
    }

    [Fact]
    public async Task UpdateClientProject_ThrowsArgumentNullException_WhenClientProjectsDtoIsNull()
    {
        await Assert.ThrowsAsync<CustomException>(() => _clientProjectService.UpdateClientProject(null!));
    }

    [Fact]
    public async Task UpdateClientProject_ReturnsUpdatedClientProjectsDto_WhenClientProjectIsFound()
    {
        _dbMock.Setup(ex => ex.ClientProject.FirstOrDefault(It.IsAny<Expression<Func<ClientProject, bool>>>()))
            .ReturnsAsync(_clientProject);

        _dbMock.Setup(ex => ex.ClientProject.Update(It.IsAny<ClientProject>()))
            .ReturnsAsync(_clientProject);

        var result = await _clientProjectService.UpdateClientProject(_clientProject.ToDto());

        Assert.NotNull(result);
        Assert.Equal(_clientProject.Id, result.Id);
    }

    [Fact]
    public async Task CheckIfExists_ReturnsTrue_WhenProjectExists()
    {
        _dbMock.Setup(ex => ex.ClientProject.GetById(1)).ReturnsAsync(_clientProject);

        var exists = await _clientProjectService.CheckIfExists(1);

        Assert.True(exists);
    }

    [Fact]
    public async Task CheckIfExists_ReturnsFalse_WhenProjectDoesNotExist()
    {
        _dbMock.Setup(ex => ex.ClientProject.GetById(1)).ReturnsAsync((ClientProject)null!);

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