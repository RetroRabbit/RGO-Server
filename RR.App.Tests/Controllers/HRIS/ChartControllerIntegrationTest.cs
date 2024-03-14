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
                "dataType=Type1&dataType=Type2&roles=Role1&chartName=SampleChart&chartType=Bar",
                Encoding.UTF8,
                "application/x-www-form-urlencoded");

            var response = await client.PostAsync("/charts", requestContent);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task DeleteChart_ReturnsOk_WithDeletedChart()
        {
        
            var client = _factory.CreateClient();
            var url = "/charts?chartId=1";
            var response = await client.DeleteAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task UpdateChartData_ReturnsOk_WithUpdatedData()
        {
   
            var client = _factory.CreateClient();
            var chartDto = new ChartDto(); // Create a chart DTO object with necessary data
            var requestContent = new StringContent(
                JsonSerializer.Serialize(chartDto),
                Encoding.UTF8,
                "application/json");

            var response = await client.PutAsync("/charts", requestContent);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
