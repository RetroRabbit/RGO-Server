using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using HRIS.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS
{
    public class ChartControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ChartControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllCharts_ReturnsOk_WithCharts()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/charts");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var charts = JsonSerializer.Deserialize<List<ChartDto>>(content);
        }

        [Fact]
        public async Task CreateChart_ReturnsOk_OnSuccess()
        {
            var client = _factory.CreateClient();
            var requestContent = new StringContent(
                JsonSerializer.Serialize(new { /* chart data */ }),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("/charts", requestContent);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
