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

        public ClientControllerIntegrationTests(WebApplicationFactory<RR.App.Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllClients_ReturnsOkResult()
        {
            var mockClientService = new Mock<IClientService>();
            mockClientService.Setup(service => service.GetAllClients()).ReturnsAsync(new List<ClientDto>());

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IClientService>(_ => mockClientService.Object);
                });
            }).CreateClient();

            var response = await client.GetAsync("/clients");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllClients_ReturnsNotFoundResultOnException()
        {
            var mockClientService = new Mock<IClientService>();
            mockClientService.Setup(service => service.GetAllClients()).ThrowsAsync(new Exception("An error occurred"));

            var client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IClientService>(_ => mockClientService.Object);
                });
            }).CreateClient();

            var response = await client.GetAsync("/clients");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
