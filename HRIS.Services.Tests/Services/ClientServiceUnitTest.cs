using HRIS.Services.Services;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class ClientServiceUnitTest
{
    private readonly Client _client;
    private readonly ClientService _clientService;
    private readonly Mock<IUnitOfWork> _dbMock;

    public ClientServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _clientService = new ClientService(_dbMock.Object);
        _client = new Client
        {
            Id = 2,
            Name = "string"
        };
    }

    [Fact]
    public async Task GetAllClientsTest()
    {
        var clients = new List<Client> { _client };

        _dbMock.Setup(x => x.Client.GetAll(null)).ReturnsAsync(clients);
        var result = await _clientService.GetAllClients();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equivalent(clients.Select(x => x.ToDto()).ToList(), result);
    }
}
