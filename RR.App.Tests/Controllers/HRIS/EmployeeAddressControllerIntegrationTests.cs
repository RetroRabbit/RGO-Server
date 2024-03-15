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
using IEmployeeAddressService = HRIS.Services.Interfaces.IEmployeeAddressService;
using IEmployeeService = HRIS.Services.Interfaces.IEmployeeService;
using IAuthService = HRIS.Services.Interfaces.IAuthService;
using IEmployeeTypeService = HRIS.Services.Interfaces.IEmployeeTypeService;
using IUnitOfWork = RR.UnitOfWork.IUnitOfWork;
using HRIS.Services.Services;
using Microsoft.Extensions.Configuration;
using RR.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using System.Net.Http.Headers;
using RR.Tests.Data.Models.HRIS;

namespace RR.App.Tests.Controllers
{

    public class EmployeeAddressControllerIntegrationTests : IClassFixture<WebApplicationFactory<RR.App.Program>>
    {
        private readonly WebApplicationFactory<RR.App.Program> _factory;
        private readonly HttpClient _client;
        private readonly IEmployeeAddressService _employeeAddressService;
        private readonly IEmployeeService _employeeService;
        private readonly IAuthService _authService;
        private readonly IEmployeeTypeService _employeeTypeService;
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeAddressControllerIntegrationTests(WebApplicationFactory<RR.App.Program> factory)
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
                    services.AddScoped<IEmployeeAddressService,EmployeeAddressService>();
                    services.AddScoped<IEmployeeService, EmployeeService>();
                    services.AddScoped<IEmployeeTypeService, EmployeeTypeService>();
                    services.AddScoped<IAuthService, AuthService>();
                });
            }).CreateClient();


            using (var scope = _factory.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<DatabaseContext>();
                _unitOfWork = services.GetRequiredService<IUnitOfWork>();
                _employeeAddressService = services.GetRequiredService<IEmployeeAddressService>();
                _employeeService = services.GetRequiredService<IEmployeeService>();
                _employeeTypeService = services.GetRequiredService<IEmployeeTypeService>();
                _authService = services.GetRequiredService<IAuthService>();
            }

            _employeeAddressService.Save(EmployeeAddressTestData.EmployeeAddressDto);

            _employeeTypeService.SaveEmployeeType(EmployeeTypeTestData.DeveloperType);

            _employeeService.SaveEmployee(EmployeeTestData.EmployeeDto);

            var token = _authService.GenerateToken(EmployeeTestData.EmployeeDto);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer","token here");
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult()
        {

            var response = await _client.GetAsync("/employee-address");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
