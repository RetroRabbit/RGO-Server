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
using IRoleAccessLinkService = HRIS.Services.Interfaces.IRoleAccessLinkService;
using IEmployeeRoleService = HRIS.Services.Interfaces.IEmployeeRoleService;
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
using RR.Tests.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Org.BouncyCastle.Utilities.Net;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using System.Net.Http.Json;

namespace RR.App.Tests.Controllers
{
    public class FakePolicyEvaluator : IPolicyEvaluator
    {
        public virtual async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            var principal = new ClaimsPrincipal();

            principal.AddIdentity(new ClaimsIdentity(new[]
            {
                new Claim("Name", "AdminOrEmployeePolicy"),
                new Claim("Permissions", "ViewEmployee", "EditEmployee" ),
                new Claim(ClaimTypes.Role, "SuperAdmin", "Admin", "Employee" ),
            }, "FakeScheme"));

            return await Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(principal,
             new AuthenticationProperties(), "FakeScheme")));
        }

        public virtual async Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy,
         AuthenticateResult authenticationResult, HttpContext context, object resource)
        {
            return await Task.FromResult(PolicyAuthorizationResult.Success());
        }
    }

    public class EmployeeAddressControllerIntegrationTests : IClassFixture<WebApplicationFactory<RR.App.Program>>
    {
        private readonly WebApplicationFactory<RR.App.Program> _factory;
        private readonly HttpClient _client;
        private readonly IEmployeeAddressService _employeeAddressService;
        private readonly IEmployeeService _employeeService;
        private readonly IAuthService _authService;
        private readonly IEmployeeTypeService _employeeTypeService;
        private readonly IRoleAccessLinkService _roleAccessLinkService;
        private readonly IEmployeeRoleService _employeeRoleService;
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
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.AddScoped<IEmployeeAddressService, EmployeeAddressService>();
                });
            }).CreateClient();


            using (var scope = _factory.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<DatabaseContext>();
            }
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult()
        {

            var response = await _client.GetAsync("/employee-address");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task SaveUpdateDeleteEmployeeAddress_ReturnsOkResult()
        {
            var addressDto = EmployeeAddressTestData.EmployeeAddressDtoNew;
            var jsonContent = new StringContent(JsonConvert.SerializeObject(addressDto), Encoding.UTF8, "application/json");
            
            var response = await _client.PostAsync("/employee-address", jsonContent);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(content);
            var addressId = jsonDoc.RootElement.GetProperty("id").GetInt32();

            var updatedDto = new EmployeeAddressDto
            {
                Id = addressId,
                UnitNumber = "56",
                ComplexName = "Complex72",
                StreetNumber = "8",
                SuburbOrDistrict = "Suburb/District",
                City = "City",
                Country = "Country",
                Province = "Province",
                PostalCode = "1620"
            };

            var updatedJsonContent = new StringContent(JsonConvert.SerializeObject(updatedDto), Encoding.UTF8, "application/json");

            response = await _client.PutAsync("/employee-address", updatedJsonContent);

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            response = await _client.DeleteAsync($"/employee-address?addressId={addressId}");

            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
