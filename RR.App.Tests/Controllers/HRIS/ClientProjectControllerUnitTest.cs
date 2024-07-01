using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;
public class ClientProjectControllerUnitTest
{
    private readonly ClientProjectsController _controller;
    private readonly Mock<IClientProjectService> _mockClientProjectService;
    public List<ClientProjectsDto> ClientProjectsList;
    public ClientProjectsDto ClientProjectDto;
    public ClientProjectControllerUnitTest()
    {
        _mockClientProjectService = new Mock<IClientProjectService>();
        _controller = new ClientProjectsController(_mockClientProjectService.Object);

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
    public async Task GetAllClientProjects_ReturnsNotFound_WhenExceptionIsThrown()
    {
        _mockClientProjectService.Setup(exception => exception.GetAllClientProjects())
          .ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.GetAllClientProjects();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Test exception", notFoundResult.Value);
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
    public async Task GetClientProjectById_ReturnsNotFound_WhenExceptionIsThrown()
    {
        _mockClientProjectService.Setup(ex => ex.GetClientProjectById(1))
        .ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.GetClientProjectById(1);

        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.NotNull(notFoundResult);
        Assert.Equal("Test exception", notFoundResult.Value);
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
    public async Task SaveClientProject_ReturnsNotFound_WhenExceptionIsThrown()
    {
        _mockClientProjectService.Setup(ex => ex.CreateClientProject(ClientProjectDto))
        .ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.SaveClientProject(ClientProjectDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Test exception", notFoundResult.Value);
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
    public async Task UpdateClientProject_ReturnsNotFound_WhenExceptionIsThrown()
    {
        _mockClientProjectService.Setup(ex => ex.UpdateClientProject(ClientProjectDto))
         .ThrowsAsync(new Exception("Test exception"));

        var result = await _controller.UpdateClientProject(ClientProjectDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Test exception", notFoundResult.Value);
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
    public async Task DeleteClientProject_ReturnsNotFound_WhenProjectDoesNotExist()
    {
        _mockClientProjectService.Setup(ex => ex.GetClientProjectById(1))
          .ReturnsAsync((ClientProjectsDto)null);

        var result = await _controller.DeleteClientProject(1);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Client Project not found", notFoundResult.Value);
    }
}