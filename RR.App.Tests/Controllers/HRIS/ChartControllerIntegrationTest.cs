using System.Net;
using System.Text;
using System.Text.Json;
using HRIS.Models;
using IChartService = HRIS.Services.Interfaces.IChartService;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RR.UnitOfWork;
using Xunit;
using HRIS.Services.Services;

namespace RR.App.Tests.Controllers.HRIS
{
    public class ChartControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public ChartControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            GlobalVariables.SetRunningTests(true);
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
                    services.AddScoped<IChartService, ChartService>();
                });
            }).CreateClient();

            using (var scope = _factory.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<DatabaseContext>();
            }
        }

        [Fact(Skip = "Unauthorised error")]
        public async Task GetAllCharts_ReturnsOk_WithCharts()
        {
            var response = await _client.GetAsync("/charts");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var charts = JsonSerializer.Deserialize<List<ChartDto>>(content);
        }

        [Fact(Skip = "Unauthorised error")]
        public async Task Create_Update_Delte_Chart_ReturnsOK_OnSuccess()
        {
            var dataType = new List<string> { "Type1", "Type2", "Type3" };
            var roles = new List<string> { "Role1", "Role2" };
            var labels = new List<string> { "Label1", "Label2" };
            var chartName = "SampleChart";
            var chartType = "Bar";
            var response = await _client.PostAsync($"/charts?dataType={string.Join("&dataType=", dataType)}" +
                                                   $"&roles={string.Join("&roles=", roles)}" +
                                                   $"&chartName={chartName}&chartType={chartType}",
                                                   content: null);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);
            var chartId = jsonDoc.RootElement.GetProperty("id").GetInt32();
            var chartDto = new ChartDto
            {
                Id = chartId,
                Name = chartName,
                Type = chartType,
                DataTypes = dataType,
                Roles = roles,
                Labels = labels,
            };
            var jsonContent = new StringContent(JsonSerializer.Serialize(chartDto), Encoding.UTF8, "application/json");

            response = await _client.PutAsync($"/charts", jsonContent);

            response.EnsureSuccessStatusCode();

            response = await _client.DeleteAsync($"/charts?chartId={chartId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact(Skip = "Unauthorised error")]
        public async Task GetColumns_ReturnsOk_WithColumns()
        {
            var response = await _client.GetAsync("/charts/column");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
        }

        [Fact(Skip = "Unauthorised error")]
        public async Task ExportCsv_ReturnsFileResult_WithData()
        {
            var response = await _client.GetAsync("/charts/report/export");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var csvData = await response.Content.ReadAsStringAsync();
            Assert.NotEmpty(csvData);

            Assert.Equal("text/csv", response.Content.Headers.ContentType.MediaType);
            Assert.Equal("Report.csv", response.Content.Headers.ContentDisposition.FileNameStar);
        }

        [Fact(Skip = "Unauthorised error")]
        public async Task GetChartData_ReturnsOk_WithData()
        {
            var dataTypes = new List<string> { "Type1", "Type2", "Type3" };
            var queryString = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "dataTypes", JsonSerializer.Serialize(dataTypes) }
            });

            var response = await _client.GetAsync("/charts/data?" + queryString.ReadAsStringAsync().Result);
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseBody = await response.Content.ReadAsStringAsync();
            var chartData = JsonSerializer.Deserialize<ChartDataDto>(responseBody);

            Assert.NotNull(chartData);
        }
    }
}
