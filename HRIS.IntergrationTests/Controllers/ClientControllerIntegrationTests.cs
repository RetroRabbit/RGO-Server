using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RR.App.Controllers.HRIS;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers
{
    public class ClientControllerIntegrationTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public ClientControllerIntegrationTests()
        {
            var mockClientService = new Mock<IClientService>();
            //mockClientService.Setup(x => x.GetAllClients()).ReturnsAsync(new List<ClientDto>());
            mockClientService.Setup(x => x.GetAllClients()).ReturnsAsync(new List<ClientDto>
            {
                new ClientDto(1, "Client1"),
                new ClientDto(2, "Client2")
            });

            _server = new TestServer(new WebHostBuilder()
                .ConfigureTestServices(services => { services.AddSingleton(mockClientService.Object); })
                .UseStartup<TestStartup>());

            _client = _server.CreateClient();
        }

        [Fact]
        public async Task GetAllClients_ReturnsSuccess()
        {
            var response = await _client.GetAsync("/clients");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllClients_NoClientsFound_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/clients");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }

    public class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<IClientService, ClientService>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
