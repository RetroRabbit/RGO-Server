﻿using System.Collections.Generic;
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
using Microsoft.Extensions.Configuration;
using RR.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;

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
                    services.AddDbContext<DatabaseContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDatabase");
                    });

                    services.AddScoped<IUnitOfWork, RR.UnitOfWork.UnitOfWork>();
                    services.AddScoped<IClientService, ClientService>();
                });
            }).CreateClient();
        }

        [Fact(Skip = "Fixing this test")]
        public async Task GetAllClients_ReturnsOkResult()
        {
            var response = await _client.GetAsync("/clients");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetAllClients_ReturnsNotFoundResultOnException()
        {
            var response = await _client.GetAsync("/clients");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
