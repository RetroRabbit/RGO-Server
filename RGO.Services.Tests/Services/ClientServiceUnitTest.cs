using Moq;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using RGO.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RGO.Services.Tests.Services
{
    public class ClientServiceUnitTest
    {
        private readonly Mock<IUnitOfWork> _dbMock;
        private readonly ClientService _clientService;
        private readonly ClientDto _clientDto;

        public ClientServiceUnitTest()
        {
            _dbMock = new Mock<IUnitOfWork>();
            _clientService = new ClientService(_dbMock.Object);
            _clientDto = new ClientDto(
                Id: 1, 
                Name: "string"
                );
        }

        [Fact]
        public async Task GetAllClientsTest()
        {
            List<ClientDto> clients = new List<ClientDto>() { _clientDto };

            _dbMock.Setup(x => x.Client.GetAll(null)).Returns(Task.FromResult(clients));
            var result = await _clientService.GetAllClients();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(clients, result);
        }
    }
}
