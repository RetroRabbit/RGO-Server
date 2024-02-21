using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class ClientUnitTest
{
    [Fact]
    public void ClientTest()
    {
        var client = new Client();
        Assert.IsType<Client>(client);
        Assert.NotNull(client);
    }

    [Fact]
    public void ClientToDtoTest()
    {
        var clientDto = new ClientDto(1, "Name");
        var client = new Client(clientDto);
        var dto = client.ToDto();
        Assert.Equal(dto.Id, client.Id);
        Assert.Equal(dto.Name, client.Name);
    }
}