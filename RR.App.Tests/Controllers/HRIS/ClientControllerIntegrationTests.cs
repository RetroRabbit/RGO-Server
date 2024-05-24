using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using HRIS.Services.Services;
using IClientService = HRIS.Services.Interfaces.IClientService;
using Microsoft.Extensions.Configuration;
using RR.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace RR.App.Tests.Controllers
{
    public class ClientControllerIntegrationTests : IClassFixture<WebApplicationFactory<RR.App.Program>>
    {
        private readonly WebApplicationFactory<RR.App.Program> _factory;
        private readonly HttpClient _client;

        public ClientControllerIntegrationTests(WebApplicationFactory<RR.App.Program> factory)
        {
            _factory = factory;

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<Program>()
                .AddEnvironmentVariables();

            var configuration = configBuilder.Build();
            var connectionString = configuration.GetConnectionString("Default");

            _client = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                });
                builder.ConfigureTestServices(services =>
                {
                    services.AddScoped<IUnitOfWork, RR.UnitOfWork.UnitOfWork>();
                    services.AddScoped<IClientService, ClientService>();
                });
            }).CreateClient();

            using (var scope = _factory.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<DatabaseContext>();
            }
        }

        [Fact]
        public async Task GetAllClients_ReturnsOkResult()
        {
            var response = await _client.GetAsync("/clients");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}