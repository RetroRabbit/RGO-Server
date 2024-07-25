using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
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
    private readonly Mock<AuthorizeIdentityMock> _identity;

    public ClientProjectServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _identity = new Mock<AuthorizeIdentityMock>();
        _clientProjectService = new ClientProjectService(_dbMock.Object, _identity.Object );
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
    public async Task CreateClientProject_Success()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

        _dbMock.Setup(ex => ex.ClientProject.GetById(1)).ReturnsAsync(_clientProject);
        _dbMock.Setup(ex => ex.ClientProject.Add(It.IsAny<ClientProject>())).ReturnsAsync(_clientProject);

        var result = await _clientProjectService.CreateClientProject(_clientProject.ToDto());

        Assert.NotNull(result);
        Assert.Equal(_clientProject.Id, result.Id);
        Assert.Equal(_clientProject.ClientName, result.ClientName);
    }

    [Fact]
    public async Task CreateClientProject_DoesExist()
    {
        _dbMock.Setup(x => x.ClientProject.Any(It.IsAny<Expression<Func<ClientProject, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(ex => ex.ClientProject.GetById(1)).ReturnsAsync(_clientProject);
        _dbMock.Setup(ex => ex.ClientProject.Add(It.IsAny<ClientProject>())).ReturnsAsync(_clientProject);

        await Assert.ThrowsAsync<CustomException>(() => _clientProjectService.CreateClientProject(_clientProject.ToDto()));
        _dbMock.Verify(x => x.ClientProject.FirstOrDefault(It.IsAny<Expression<Func<ClientProject, bool>>>()), Times.Never);
    }

    [Fact]
    public async Task CreateClientProject_Unauthorized()
    {
        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.SetupGet(i => i.EmployeeId).Returns(2);

        _dbMock.Setup(x => x.ClientProject.Any(It.IsAny<Expression<Func<ClientProject, bool>>>()))
            .ReturnsAsync(false);

        _dbMock.Setup(ex => ex.ClientProject.Add(It.IsAny<ClientProject>())).ReturnsAsync(_clientProject);

        await Assert.ThrowsAsync<CustomException>(() => _clientProjectService.CreateClientProject(_clientProject.ToDto()));
    }

    [Fact]
    public async Task DeleteClientProject_Success()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

        _dbMock.Setup(x => x.ClientProject.Any(It.IsAny<Expression<Func<ClientProject, bool>>>()))
          .ReturnsAsync(true);

        _dbMock.Setup(ex => ex.ClientProject.Delete(1)).ReturnsAsync(_clientProject);

        var result = await _clientProjectService.DeleteClientProject(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task DeleteClientProject_DoesNotExist()
    {
        _dbMock.Setup(x => x.ClientProject.Any(It.IsAny<Expression<Func<ClientProject, bool>>>()))
          .ReturnsAsync(false);

        _dbMock.Setup(ex => ex.ClientProject.Delete(_clientProject.Id)).ReturnsAsync(_clientProject);

        await Assert.ThrowsAsync<CustomException>(() => _clientProjectService.DeleteClientProject(_clientProject.Id));
    }

    [Fact]
    public async Task DeleteClientProject_Unauthorized()
    {
        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.SetupGet(i => i.EmployeeId).Returns(2);

        _dbMock.Setup(x => x.ClientProject.Any(It.IsAny<Expression<Func<ClientProject, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(ex => ex.ClientProject.Delete(_clientProject.Id)).ReturnsAsync(_clientProject);

        await Assert.ThrowsAsync<CustomException>(() => _clientProjectService.DeleteClientProject(_clientProject.Id));
    }

    [Fact]
    public async Task UpdateClientProject_Success()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

        _dbMock.Setup(x => x.ClientProject.Any(It.IsAny<Expression<Func<ClientProject, bool>>>()))
          .ReturnsAsync(true);

        _dbMock.Setup(x => x.ClientProject.FirstOrDefault(It.IsAny<Expression<Func<ClientProject, bool>>>()))
          .ReturnsAsync(_clientProject);

        _dbMock.Setup(ex => ex.ClientProject.Update(_clientProject)).ReturnsAsync(_clientProject);

        var result = await _clientProjectService.UpdateClientProject(_clientProject.ToDto());

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task UpdateClientProject_DoesNotExist()
    {
        _dbMock.Setup(x => x.ClientProject.Any(It.IsAny<Expression<Func<ClientProject, bool>>>()))
         .ReturnsAsync(false);

        _dbMock.Setup(ex => ex.ClientProject.FirstOrDefault(q => q.Id == _clientProject.Id)).ReturnsAsync((ClientProject)null!);

        await Assert.ThrowsAsync<CustomException>(() => _clientProjectService.UpdateClientProject(_clientProject.ToDto()));
    }

    [Fact]
    public async Task UpdateClientProject_Unauthorized()
    {
        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.SetupGet(i => i.EmployeeId).Returns(2);

        _dbMock.Setup(x => x.ClientProject.Any(It.IsAny<Expression<Func<ClientProject, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(ex => ex.ClientProject.Update(_clientProject)).ReturnsAsync(_clientProject);

        await Assert.ThrowsAsync<CustomException>(() => _clientProjectService.UpdateClientProject(_clientProject.ToDto()));
    }

    [Fact]
    public async Task GetClientProjectById_Success()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

        _dbMock.Setup(x => x.ClientProject.Any(It.IsAny<Expression<Func<ClientProject, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(ex => ex.ClientProject.GetById(_clientProject.Id)).ReturnsAsync(_clientProject);

        var result = await _clientProjectService.GetClientProjectById(_clientProject.Id);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetClientProjectById_Unauthorized()
    {
        _identity.Setup(i => i.Role).Returns("Employee");

        _dbMock.Setup(x => x.ClientProject.Any(It.IsAny<Expression<Func<ClientProject, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(ex => ex.ClientProject.GetById(_clientProject.Id)).ReturnsAsync(_clientProject);

        await Assert.ThrowsAsync<CustomException>(() => _clientProjectService.GetClientProjectById(_clientProject.Id));
    }
}