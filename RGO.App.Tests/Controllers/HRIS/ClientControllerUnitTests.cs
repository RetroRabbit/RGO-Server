using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class ClientControllerUnitTests
{
    [Fact]
    public async Task GetAllClientsReturnsOkResultWithClients()
    {
        var expectedClients = new List<ClientDto>
        {
          new ClientDto{Id = 1,Name = "string"},
          new ClientDto { Id = 2, Name = "string" },
          new ClientDto { Id = 3, Name = "string" },
          new ClientDto { Id = 4, Name = "string" },
          new ClientDto { Id = 5, Name = "string" },
          new ClientDto { Id = 6, Name = "string" },
          new ClientDto { Id = 7, Name = "string" }
        };

        var mockClientService = new Mock<IClientService>();
        mockClientService.Setup(service => service.GetAllClients())
                         .ReturnsAsync(expectedClients);

        var controller = new ClientController(mockClientService.Object);

        var result = await controller.GetAllClients();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualClients = Assert.IsAssignableFrom<List<ClientDto>>(okResult.Value);
        Assert.Equal(expectedClients, actualClients);
    }

    [Fact]
    public async Task GetAllClientsReturnsNotFoundResultWhenNoClientsFound()
    {
        var mockClientService = new Mock<IClientService>();
        mockClientService.Setup(service => service.GetAllClients())
                         .ReturnsAsync((List<ClientDto>?)null);

        var controller = new ClientController(mockClientService.Object);

        var result = await controller.GetAllClients();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("No clients found", notFoundResult.Value);
    }

    [Fact]
    public async Task GetAllClientsReturnsNotFoundResultWhenExceptionThrown()
    {
        var mockClientService = new Mock<IClientService>();
        mockClientService.Setup(service => service.GetAllClients())
                         .ThrowsAsync(new Exception("An error occurred"));

        var controller = new ClientController(mockClientService.Object);

        var result = await controller.GetAllClients();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred", notFoundResult.Value);
    }
}
