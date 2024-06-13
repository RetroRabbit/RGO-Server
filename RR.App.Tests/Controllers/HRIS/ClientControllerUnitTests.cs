using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class ClientControllerUnitTests
{
    private readonly Mock<IClientService> _clientServiceMock;
    private readonly ClientController _controller;
    private readonly List<ClientDto> _clientDtoList;
    public ClientControllerUnitTests() 
    { 
        _clientServiceMock = new Mock<IClientService>();
        _controller = new ClientController(_clientServiceMock.Object);

        _clientDtoList = new List<ClientDto>
        {
          new ClientDto{Id = 1,Name = "string"},
          new ClientDto { Id = 2, Name = "string" },
          new ClientDto { Id = 3, Name = "string" },
          new ClientDto { Id = 4, Name = "string" },
          new ClientDto { Id = 5, Name = "string" },
          new ClientDto { Id = 6, Name = "string" },
          new ClientDto { Id = 7, Name = "string" }
        };
    }

    [Fact]
    public async Task GetAllClientsReturnsOkResultWithClients()
    {
        _clientServiceMock.Setup(service => service.GetAllClients())
                         .ReturnsAsync(_clientDtoList);

        var result = await _controller.GetAllClients();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualClients = Assert.IsAssignableFrom<List<ClientDto>>(okResult.Value);
        Assert.Equal(_clientDtoList, actualClients);
    }

    [Fact]
    public async Task GetAllClientsReturnsNotFoundResultWhenNoClientsFound()
    {
        _clientServiceMock.Setup(service => service.GetAllClients())
                         .ReturnsAsync((List<ClientDto>?)null);

        var result = await _controller.GetAllClients();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("No clients found", notFoundResult.Value);
    }

    [Fact]
    public async Task GetAllClientsReturnsNotFoundResultWhenExceptionThrown()
    {
        _clientServiceMock.Setup(service => service.GetAllClients())
                         .ThrowsAsync(new Exception("An error occurred"));

        var result = await _controller.GetAllClients();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred", notFoundResult.Value);
    }
}