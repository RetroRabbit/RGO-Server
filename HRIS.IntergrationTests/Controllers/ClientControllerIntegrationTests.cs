using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Google.Apis.Services;
using HRIS.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;
using RR.App.Controllers.HRIS;
using RR.UnitOfWork.Entities.HRIS;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using IClientService = HRIS.Services.Interfaces.IClientService;

namespace RR.App.Tests.Controllers
{
    public class ClientControllerIntegrationTests : IClassFixture<WebApplicationFactory<RR.App.Program>>
    {
        private readonly WebApplicationFactory<RR.App.Program> _factory;
        private readonly HttpClient _client;
        private readonly Mock<IClientService> _mockClientService;

        public ClientControllerIntegrationTests(WebApplicationFactory<RR.App.Program> factory)
        {
            _factory = factory;
            _mockClientService = new Mock<IClientService>();
            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IClientService>(_ => _mockClientService.Object);
                });
            }).CreateClient();
        }

        [Fact]
        public async Task GetAllClients_ReturnsOkResult()
        {
            var clients = new List<ClientDto>
            { 
                new ClientDto { Id = 1, Name = "Client1"},
                new ClientDto { Id = 2, Name = "Client2"}
            };

            _mockClientService.Setup(service => service.GetAllClients()).ReturnsAsync(clients);

            var response = await _client.GetAsync("/clients");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllClients_ReturnsNotFoundResultOnException()
        {
            _mockClientService.Setup(service => service.GetAllClients()).ThrowsAsync(new Exception("An error occurred"));

            var response = await _client.GetAsync("/clients");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
