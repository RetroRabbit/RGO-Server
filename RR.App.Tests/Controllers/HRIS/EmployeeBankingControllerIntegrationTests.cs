using HRIS.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using HRIS.Services.Services;
using IUnitOfWork = RR.UnitOfWork.IUnitOfWork;
using Microsoft.Extensions.Configuration;
using RR.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Net;
using RR.Tests.Data.Models.HRIS;
using Microsoft.AspNetCore.Authorization.Policy;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;
using static Google.Apis.Requests.BatchRequest;

namespace RR.App.Tests.Controllers;

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

public class EmployeeBankingControllerIntegrationTests : IClassFixture<WebApplicationFactory<RR.App.Program>>
{
    private readonly WebApplicationFactory<RR.App.Program> _factory;
    private readonly HttpClient _client;

    public EmployeeBankingControllerIntegrationTests(WebApplicationFactory<RR.App.Program> factory)
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
    public async Task Get_ReturnsOkResult()
    {
        var response = await _client.GetAsync("/employee-banking");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateReadUpdateDeleteEmployeeBanking_ReturnsOkResult()
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

        var bankingDto = EmployeeBankingTestData.GetModifiedEmployeeBankingDtoWithEmployeeId(employeeId);

        var jsonContentBanking = new StringContent(JsonConvert.SerializeObject(bankingDto), Encoding.UTF8, "application/json");

        response = await _client.PostAsync("/employee-banking", jsonContentBanking);

        content = await response.Content.ReadAsStringAsync();
        jsonDoc = JsonDocument.Parse(content);
        var bankingId = jsonDoc.RootElement.GetProperty("id").GetInt32();

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await _client.GetAsync($"/employee-banking/details?id={employeeId}");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updatedBankingDto = EmployeeBankingTestData.GetModifiedEmployeeBankingDtoWithEmployeeIdAndBankingId(bankingId, employeeId);

        var jsonContentUpdatedBanking = new StringContent(JsonConvert.SerializeObject(updatedBankingDto), Encoding.UTF8, "application/json");

        response = await _client.PutAsync("/employee-banking", jsonContentUpdatedBanking);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await _client.DeleteAsync($"/employee-banking?addressId={bankingId}");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        response = await _client.DeleteAsync($"/employees?email={employeeEmail}");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteEmployeeBanking_ReturnsOkResult()
    {
        var employeeEmail = "test@gmail.com";
        var response = await _client.DeleteAsync($"/employees?email={employeeEmail}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
