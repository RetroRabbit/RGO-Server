using RGO.Models.Enums;
using RGO.Models;
using RGO.UnitOfWork.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RGO.UnitOfWork.Tests.Entities
{
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
        public async Task ClientToDtoTest()
        {
            var clientDto = new ClientDto(1, "Name");
            var client = new Client(clientDto);
            var dto = client.ToDto();
            Assert.Equal(dto.Id, client.Id);
            Assert.Equal(dto.Name, client.Name);
        }
    }
}
