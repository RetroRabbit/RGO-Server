/*using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeCertificationControllerIntergrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public class FakePolicyEvaluatorAllRolesPolicy : IPolicyEvaluator
    {
        public virtual async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            GlobalVariables.SetRunningTests(true);
            var principal = new ClaimsPrincipal();

            principal.AddIdentity(new ClaimsIdentity(new[]
            {
            new Claim("Name", "AdminOrEmployeePolicy"),
            new Claim(ClaimTypes.Role, "SuperAdmin"),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(ClaimTypes.Role, "Employee"),
            new Claim(ClaimTypes.Email,"testintegration2@retrorabbit.co.za"),
            new Claim("Permissions", "ViewEmployee", "EditEmployee" ),
        }, "FakeScheme"));

            principal.AddIdentity(new ClaimsIdentity(new[]
            {
            new Claim("Permission", "ViewEmployee"),
            new Claim("Permission", "EditEmployee"),
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

    public class FakePolicyEvaluatorAdminSuperAdminPolicy : IPolicyEvaluator
    {
        public virtual async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            var principal = new ClaimsPrincipal();

            principal.AddIdentity(new ClaimsIdentity(new[]
            {
            new Claim("Name", "AdminOrSuperAdminPolicy"),
            new Claim(ClaimTypes.Role,"Admin"),
            new Claim(ClaimTypes.Role, "SuperAdmin")
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

    public class FakePolicyEvaluatorAdminOrTalentOrSuperAdminPolicy : IPolicyEvaluator
    {
        public virtual async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            var principal = new ClaimsPrincipal();

            principal.AddIdentity(new ClaimsIdentity(new[]
            {
            new Claim("Name", "AdminOrTalentOrSuperAdminPolicy"),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(ClaimTypes.Role, "SuperAdmin"),
            new Claim(ClaimTypes.Role, "Talent"),
        }, "FakeScheme"));

            principal.AddIdentity(new ClaimsIdentity(new[]
    {
            new Claim("Permission", "ViewChart"),
            new Claim("Permission", "AddChart"),
            new Claim("Permission", "EditChart"),
            new Claim("Permission", "DeleteChart"),
            new Claim("Permission", "ViewEmployee"),
            new Claim("Permission", "EditEmployee"),
            new Claim("Permission", "ViewOwnInfo"),
            new Claim("Permission", "EditOwnInfo"),
            new Claim("Permission", "GetAllFieldCodes")
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

    public EmployeeCertificationControllerIntergrationTest(WebApplicationFactory<Program> factory)
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
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluatorAdminSuperAdminPolicy>();
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluatorAdminOrTalentOrSuperAdminPolicy>();
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluatorAllRolesPolicy>();
                services.AddScoped<IEmployeeBankingService, EmployeeBankingService>();
            });
        }).CreateClient();

        using (var scope = _factory.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<DatabaseContext>();
        }
    }

    [Fact]
    public async Task GetAllCertificatesReturnsOk()
    {
        var response = await _client.GetAsync($"/certification?employeeId={EmployeeTestData.EmployeeDto.Id}");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateCertificateOnSuccess()
    {
        var employeeDto = EmployeeTestData.EmployeeDtoNew;
        var jsonContentEmployee = new StringContent(JsonConvert.SerializeObject(employeeDto), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/employees", jsonContentEmployee);

        var content = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(content);
        var employeeId = jsonDoc.RootElement.GetProperty("id").GetInt32();
        var employeeEmail = jsonDoc.RootElement.GetProperty("email").GetString();

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        EmployeeCertificationDto certificate = new EmployeeCertificationDto
        {
            Id = 0,
            IssueDate = DateTime.UtcNow,
            IssueOrganization = "as",
            CertificateDocument = "as",
            CertificateName = "Test",
            //EmployeeId = employeeId,
            EmployeeId = EmployeeTestData.EmployeeDto.Id,
        };
        var jsonContentCertificate = new StringContent(JsonConvert.SerializeObject(certificate), Encoding.UTF8, "application/json");

        response = await _client.PostAsync("/employee-certification", jsonContentCertificate);

        content = await response.Content.ReadAsStringAsync();
        jsonDoc = JsonDocument.Parse(content);
        var certificateId = jsonDoc.RootElement.GetProperty("Id").GetInt32();

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await _client.GetAsync($"/employee-certification?id={employeeId}");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updatedBankingDto = EmployeeBankingTestData.GetModifiedEmployeeBankingDtoWithEmployeeIdAndBankingId(certificateId, employeeId);

        var jsonContentUpdatedBanking = new StringContent(JsonConvert.SerializeObject(updatedBankingDto), Encoding.UTF8, "application/json");

        response = await _client.PutAsync("/employee-banking", jsonContentUpdatedBanking);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await _client.DeleteAsync($"/employee-certification?id={certificateId}");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await _client.DeleteAsync($"/employees?email={employeeEmail}");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetEmployeeCertificate()
    {
        var employeeCertificate = new EmployeeCertificationDto
        {
            IssueOrganization = "Amazon",
            IssueDate = DateTime.Now,
            CertificateName = "Test",
            CertificateDocument = "asd",
            Id = 0,
            EmployeeId = 1,
        };

        // var response = await _client.GetAsync($"/employee-certificate?employeeId={1}&certificationId={newCertficate}");
        // response.EnsureSuccessStatusCode();
        // Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
*/