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


namespace RR.App.Tests.Controllers;

public class FakePolicyEvaluatorAllRolesPolicy : IPolicyEvaluator
{
    public virtual async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
    {
        var principal = new ClaimsPrincipal();

        principal.AddIdentity(new ClaimsIdentity(new[]
        {
            new Claim("Name", "AdminOrEmployeePolicy"),
            new Claim("Permissions", "ViewEmployee", "EditEmployee" ),
            new Claim(ClaimTypes.Role, "SuperAdmin", "Admin", "Employee" ),
            new Claim(ClaimTypes.Email,"testintegration2@retrorabbit.co.za")
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
            new Claim(ClaimTypes.Role,"Admin", "SuperAdmin"),
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
            new Claim("Permissions", "Admin", "SuperAdmin", "Talent" ),
            new Claim(ClaimTypes.Role, "ViewEmployee", "EditEmployee", "ViewOwnInfo", "EditOwnInfo" ),
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
    public async Task GetBankingDetails_ReturnsOkResult()
    {
        var idhere = 1;
        var response = await _client.GetAsync($"/employee-banking/details?id={idhere}");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task SaveUpdateDeleteEmployeeBanking_ReturnsOkResult()
    {
        var employeeDto = EmployeeTestData.EmployeeDtoNew;
        var jsonContentEmployee = new StringContent(JsonConvert.SerializeObject(employeeDto), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("/employees", jsonContentEmployee);

        var content = await response.Content.ReadAsStringAsync();
        var jsonDoc = JsonDocument.Parse(content);
        var employeeId = jsonDoc.RootElement.GetProperty("id").GetInt32();

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var bankingDto = new EmployeeBankingDto
        {
            Id = 0,
            EmployeeId = employeeId,
            BankName = "FNB",
            Branch = "Not Sure",
            AccountNo = "120",
            AccountType = EmployeeBankingAccountType.Savings,
            AccountHolderName = "Name1",
            Status = BankApprovalStatus.PendingApproval,
            DeclineReason = "",
            File = "asd",
            LastUpdateDate = new DateOnly(),
            PendingUpdateDate = new DateOnly()
        };

        var jsonContentBanking = new StringContent(JsonConvert.SerializeObject(bankingDto), Encoding.UTF8, "application/json");

        response = await _client.PostAsync("/employee-banking", jsonContentBanking);

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        content = await response.Content.ReadAsStringAsync();
        jsonDoc = JsonDocument.Parse(content);
        var bankingId = jsonDoc.RootElement.GetProperty("id").GetInt32();

        response = await _client.GetAsync($"/employee-banking/details?id={bankingId}");

        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        //var updatedDto = new EmployeeAddressDto
        //{
        //    Id = addressId,
        //    UnitNumber = "56",
        //    ComplexName = "Complex72",
        //    StreetNumber = "8",
        //    SuburbOrDistrict = "Suburb/District",
        //    City = "City",
        //    Country = "Country",
        //    Province = "Province",
        //    PostalCode = "1620"
        //};

        //var updatedJsonContent = new StringContent(JsonConvert.SerializeObject(updatedDto), Encoding.UTF8, "application/json");

        //response = await _client.PutAsync("/employee-address", updatedJsonContent);

        //response.EnsureSuccessStatusCode();
        //Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        //response = await _client.DeleteAsync($"/employee-address?addressId={addressId}");

        //response.EnsureSuccessStatusCode();
        //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
