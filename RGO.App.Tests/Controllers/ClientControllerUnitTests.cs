using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.App.Controllers;
using Xunit;

namespace RGO.App.Tests.Controllers;

public class ClientControllerUnitTests
{
    [Fact]
    public async Task GetAllClientsReturnsOkResultWithClients()
    {
        var expectedClients = new List<ClientDto>
        {
            new ClientDto (1, "Discovery"),
            new ClientDto (2, "FNB"),
            new ClientDto (3, "Capitec"),
            new ClientDto (4, "Telesure"),
            new ClientDto (5, "Nedbank"),
            new ClientDto (6, "Standard Bank"),
            new ClientDto (7, "Bench"),
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


