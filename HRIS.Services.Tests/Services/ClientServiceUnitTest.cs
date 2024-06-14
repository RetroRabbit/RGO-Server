using HRIS.Models;
using HRIS.Services.Services;
using Moq;
using RR.UnitOfWork;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class ClientServiceUnitTest
{
    private readonly ClientDto _clientDto;
    private readonly ClientService _clientService;
    private readonly Mock<IUnitOfWork> _dbMock;

    public ClientServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _clientService = new ClientService(_dbMock.Object);
        _clientDto = new ClientDto
        {
            Id = 1,
            Name = "string"
        };
    }

    [Fact]
    public async Task GetAllClientsTest()
    {
        var clients = new List<ClientDto> { _clientDto };

        _dbMock.Setup(x => x.Client.GetAll(null)).Returns(Task.FromResult(clients));
        var result = await _clientService.GetAllClients();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(clients, result);
    }
}
