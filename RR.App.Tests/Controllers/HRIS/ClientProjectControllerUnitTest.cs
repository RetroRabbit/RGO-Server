using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;
public class ClientProjectControllerUnitTest
{
    private readonly ClientProjectsController _controller;
    private readonly ClientProjectsController _unauthorizedController;
    private readonly Mock<IClientProjectService> _mockClientProjectService;
    public List<ClientProjectsDto> ClientProjectsList;
    public ClientProjectsDto ClientProjectDto;
    public ClientProjectControllerUnitTest()
    {
        _mockClientProjectService = new Mock<IClientProjectService>();
        _unauthorizedController = new ClientProjectsController(new AuthorizeIdentityMock("unauthorized@example.com", "UnauthorizedUser", "User", 2), _mockClientProjectService.Object);
        _controller = new ClientProjectsController(new AuthorizeIdentityMock("test@example.com", "TestUser", "SuperAdmin", 1), _mockClientProjectService.Object);

        ClientProjectsList = new List<ClientProjectsDto>
            { new ClientProjectsDto
               {
                  Id = 1,
                  EmployeeId = 1,
                  ClientName = "ClientName1",
                  ProjectName = "ProjectName1",
                  StartDate = DateTime.Now,
                  EndDate = DateTime.Now,
                  ProjectURL = "URL"
               }
            };

        ClientProjectDto = new ClientProjectsDto
        {
            Id = 1,
            EmployeeId = 1,
            ClientName = "ClientName1",
            ProjectName = "ProjectName1",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now,
            ProjectURL = "URL"
        };
    }

    [Fact]
    public async Task GetAllClientProjects_ReturnsOkResult_WithListOfClientProjects()
    {
        _mockClientProjectService.Setup(ex => ex.GetAllClientProjects())
          .ReturnsAsync(ClientProjectsList);

        var result = await _controller.GetAllClientProjects();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(ClientProjectsList, okResult.Value);
    }

    [Fact]
    public async Task GetClientProjectById_ReturnsOkResult_WithClientProjectsDto()
    {
        _mockClientProjectService.Setup(ex => ex.GetClientProjectById(1))
          .ReturnsAsync(ClientProjectDto);

        var result = await _controller.GetClientProjectById(1);

        var okResult = result.Result as OkObjectResult;
        Assert.NotNull(okResult);
        var returnValue = Assert.IsType<ClientProjectsDto>(okResult.Value);
        Assert.Equal(ClientProjectDto, returnValue);
    }

    [Fact]
    public async Task GetClientProjectById_Unauthorized()
    {
        _mockClientProjectService.Setup(x => x.GetClientProjectById(1))
            .ThrowsAsync(new CustomException("Unauthorized Access."));

        var exception = await Assert.ThrowsAsync<CustomException>(async () =>
            await _unauthorizedController.GetClientProjectById(1));

        Assert.Equal("Unauthorized Access.", exception.Message);
    }

    [Fact]
    public async Task SaveClientProject_ReturnsOkResult_WithCreatedClientProject()
    {
        _mockClientProjectService.Setup(ex => ex.CreateClientProject(ClientProjectDto))
         .ReturnsAsync(ClientProjectDto);

        var result = await _controller.SaveClientProject(ClientProjectDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<ClientProjectsDto>(okResult.Value);
        Assert.Equal(ClientProjectDto, returnValue);
    }

    [Fact]
    public async Task SaveClientProject_Unauthorized_WhenExceptionIsThrown()
    {
        _mockClientProjectService.Setup(x => x.CreateClientProject(ClientProjectDto))
            .ThrowsAsync(new CustomException("Unauthorized Access."));

        var exception = await Assert.ThrowsAsync<CustomException>(async () =>
            await _unauthorizedController.SaveClientProject(ClientProjectDto));

        Assert.Equal("Unauthorized Access.", exception.Message);
    }

    [Fact]
    public async Task UpdateClientProject_ReturnsOkResult_WithUpdatedClientProject()
    {
        _mockClientProjectService.Setup(ex => ex.UpdateClientProject(ClientProjectDto))
         .ReturnsAsync(ClientProjectDto);

        var result = await _controller.UpdateClientProject(ClientProjectDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<ClientProjectsDto>(okResult.Value);
        Assert.Equal(ClientProjectDto, returnValue);
    }

    [Fact]
    public async Task UpdateClientProject_Unauthorized_WhenExceptionIsThrown()
    {
        _mockClientProjectService.Setup(x => x.UpdateClientProject(ClientProjectDto))
        .ThrowsAsync(new CustomException("Unauthorized Access."));

        var exception = await Assert.ThrowsAsync<CustomException>(async () =>
            await _unauthorizedController.UpdateClientProject(ClientProjectDto));

        Assert.Equal("Unauthorized Access.", exception.Message);
    }

    [Fact]
    public async Task DeleteClientProject_ReturnsOkResult_WhenProjectIsDeleted()
    {
        _mockClientProjectService.Setup(service => service.GetClientProjectById(1))
         .ReturnsAsync(ClientProjectDto);
        _mockClientProjectService.Setup(ex => ex.DeleteClientProject(1))
         .ReturnsAsync(ClientProjectDto);

        var result = await _controller.DeleteClientProject(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<ClientProjectsDto>(okResult.Value);
        Assert.Equal(ClientProjectDto, returnValue);
    }

    [Fact]
    public async Task DeleteClientProject_Unauthorized_WhenProjectDoesNotExist()
    {
        _mockClientProjectService.Setup(x => x.GetClientProjectById(1))
       .ThrowsAsync(new CustomException("Unauthorized Access."));

        var exception = await Assert.ThrowsAsync<CustomException>(async () =>
           await _unauthorizedController.DeleteClientProject(1));

        Assert.Equal("Unauthorized Access.", exception.Message);
    }
}