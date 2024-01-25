using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RGO.App.Controllers;
using RGO.Models;
using RGO.Services.Interfaces;
using System.Security.Claims;
using Xunit;
using Xunit.Sdk;

namespace RGO.App.Tests.Controllers;

public class EmployeeControllerUnitTests
{
    private readonly Mock<IEmployeeService> _employeeMockService;
    private readonly EmployeeController _controller;
    private readonly EmployeeDto _employee;
    List<Claim> claims;
    ClaimsPrincipal claimsPrincipal;
    ClaimsIdentity identity;
    private readonly EmployeeTypeDto employeeTypeDto = new(1, "Developer");
    private readonly EmployeeAddressDto employeeAddressDto = new(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

    public EmployeeControllerUnitTests()
    {
        _employeeMockService = new Mock<IEmployeeService>();
        _controller = new EmployeeController(_employeeMockService.Object);

        _employee = new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
                null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Kamo", "K.G.",
                "Smith", new DateTime(), "South Africa", "South African", "1234457899", " ",
                new DateTime(), null, Models.Enums.Race.Black, Models.Enums.Gender.Female, null!,
                "ksmith@retrorabbit.co.za", "kmaosmith@gmail.com", "0123456789", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, "ksmith@retrorabbit.co.za"),
        };
        identity = new ClaimsIdentity(claims, "TestAuthType");
        claimsPrincipal = new ClaimsPrincipal(identity);
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = claimsPrincipal }
        };
    }

    private ClaimsPrincipal SetupClaimsProncipal(string email)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, email)
        };

        var identity = new ClaimsIdentity(claims, "Test");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        return claimsPrincipal;
    }

    private void SetupControllerContext(EmployeeController controller, ClaimsPrincipal principal)
    {
        var context = new DefaultHttpContext { User = principal };
        controller.ControllerContext = new ControllerContext { HttpContext = context };
    }

    [Fact]
    public async Task AddEmployeeSuccessTest()
    {
        _employeeMockService.Setup(service => service.SaveEmployee(_employee))
            .ReturnsAsync(_employee);

        var result = await _controller.AddEmployee(_employee);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("AddEmployee", createdAtActionResult.ActionName);
        Assert.Equal(201, createdAtActionResult.StatusCode);
    }

    [Fact]
    public async Task AddEmployeeEmployeeExistsFailTest()
    {
        _employeeMockService.Setup(service => service.SaveEmployee(_employee))
            .ThrowsAsync(new Exception("Employee exists"));

        var result = await _controller.AddEmployee(_employee);

        var problemDetails = Assert.IsType<ObjectResult>(result);
        Assert.Equal(406, problemDetails.StatusCode);
        Assert.Equal("User Exists", ((ProblemDetails)problemDetails.Value!).Title);
    }

    [Fact]
    public async Task AddEmployeeNotFoundFailTest()
    {
        _employeeMockService.Setup(service => service.SaveEmployee(_employee))
            .ThrowsAsync(new Exception("Not found"));

        var result = await _controller.AddEmployee(_employee);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetEmployeeWithClaimTest()
    {
        var principal = SetupClaimsProncipal(_employee.Email);
        SetupControllerContext(_controller, principal);

        _employeeMockService.Setup(service => service.GetEmployee(It.IsAny<string>()))
            .ReturnsAsync(_employee);

        var result = await _controller.GetEmployee(null);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
    }

    [Fact]
    public async Task GetEmployeeSuccessTest()
    {
        var principal = SetupClaimsProncipal(_employee.Email);
        SetupControllerContext(_controller, principal);

        _employeeMockService.Setup(service => service.GetEmployee(It.IsAny<string>()))
            .ReturnsAsync(_employee);

        var result = await _controller.GetEmployee(_employee.Email);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(_employee, (EmployeeDto)okObjectResult.Value!);
    }

    [Fact]
    public async Task GetEmployeeFailTest()
    {
        var principal = SetupClaimsProncipal(_employee.Email);
        SetupControllerContext(_controller, principal);

        _employeeMockService.Setup(service => service.GetEmployee(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Not found"));

        var result = await _controller.GetEmployee(_employee.Email);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task UpdateEmployeeSuccessTest()
    {
        _employeeMockService.Setup(service => service.UpdateEmployee(_employee, _employee.Email))
            .ReturnsAsync(_employee);

        var result = await _controller.UpdateEmployee(_employee, "ksmith@retrorabbit.co.za");

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("UpdateEmployee", createdAtActionResult.ActionName);
        Assert.Equal(201, createdAtActionResult.StatusCode);
    }

    [Fact]
    public async Task UpdateEmployeeFailTest()
    {
        _employeeMockService.Setup(service => service.UpdateEmployee(_employee, _employee.Email))
            .ThrowsAsync(new Exception("Not found"));

        var result = await _controller.UpdateEmployee(_employee, "ksmith@retrorabbit.co.za");

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task UpdateEmployeeUnauthorized()
    {
        _employeeMockService.Setup(service => service.UpdateEmployee(_employee, _employee.Email))
            .ThrowsAsync(new Exception("Unauthorized action"));
        var result = await _controller.UpdateEmployee(_employee, "ksmith@retrorabbit.co.za");
        var statusCodeResult = (ObjectResult)result;
        Assert.Equal(403, statusCodeResult.StatusCode);
    }

    [Fact]
    public async Task GetAllEmployeesSuccessTest()
    {
        _employeeMockService.Setup(service => service.GetAll("ksmith@retrorabbit.co.za"))
            .ReturnsAsync(new List<EmployeeDto> { _employee });

        var result = await _controller.GetAllEmployees();

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(new List<EmployeeDto> { _employee }, (List<EmployeeDto>)okObjectResult.Value!);
    }

    [Fact]
    public async Task GetAllEmployeesFailTest()
    {
        _employeeMockService.Setup(service => service.GetAll("ksmith@retrorabbit.co.za"))
            .ThrowsAsync(new Exception("Not found"));

        var result = await _controller.GetAllEmployees();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task CountAllEmployeesSuccessTest()
    {
        _employeeMockService.Setup(service => service.GetAll("ksmith@retrorabbit.co.za"))
            .ReturnsAsync(new List<EmployeeDto> { _employee });

        var result = await _controller.CountAllEmployees();

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(1, (int)okObjectResult.Value!);
    }

    [Fact]
    public async Task CountAllEmployeesFailTest()
    {
        _employeeMockService.Setup(service => service.GetAll(""))
            .ThrowsAsync(new Exception("Not found"));

        var result = await _controller.CountAllEmployees();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetEmployeeByIdSuccess()
    {
        _employeeMockService.Setup(service => service.GetEmployeeById(It.IsAny<int>()))
            .ReturnsAsync(_employee);

        var result = await _controller.GetEmployeeById(_employee.Id);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(_employee, (EmployeeDto)okObjectResult.Value!);
    }

    [Fact]
    public async Task GetEmployeeByIdFail()
    {
        var principal = SetupClaimsProncipal(_employee.Email);
        SetupControllerContext(_controller, principal);

        _employeeMockService.Setup(service => service.GetEmployeeById(It.IsAny<int>()))
            .ThrowsAsync(new Exception("Not found"));

        var result = await _controller.GetEmployeeById(2);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetSimpleEmployeeSuccess()
    {
        SimpleEmployeeProfileDto employee = new SimpleEmployeeProfileDto(1, "1", "123123",
            new DateTime(), null, null, null, false, "", 3, employeeTypeDto, "", null,
            null, null, null, "John", "J", "Doe", new DateTime(), null, null, "123", "123", null,
            null, Models.Enums.Race.Coloured, Models.Enums.Gender.Male, null, "ksmith@retrorabbit.co.za",
            "ba@gmail.com", "123", null, null, null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        _employeeMockService.Setup(service => service.GetSimpleProfile(It.IsAny<string>())).ReturnsAsync(employee);

        var result = await _controller.GetSimpleEmployee(employee.Email);

        var simpleEmployee = (ObjectResult)result;

        Assert.Equal(employee, simpleEmployee.Value);
    }

    [Fact]
    public async Task GetSimpleEmployeeFail()
    {
        SimpleEmployeeProfileDto employee = new SimpleEmployeeProfileDto(1, "1", "123123",
            new DateTime(), null, null, null, false, "", 3, employeeTypeDto, "", null,
            null, null, null, "John", "J", "Doe", new DateTime(), null, null, "123", "123", null,
            null, Models.Enums.Race.Coloured, Models.Enums.Gender.Male, null, "ksmith@retrorabbit.co.za",
            "ba@gmail.com", "123", null, null, null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        _employeeMockService.Setup(service => service.GetSimpleProfile(It.IsAny<string>())).ThrowsAsync(new Exception("Not Found"));

        var result = await _controller.GetSimpleEmployee(employee.Email);

        var simpleEmployee = (NotFoundObjectResult)result;

        Assert.Equal("Not Found", simpleEmployee.Value);
    }

    [Fact]
    public async Task FilterByTypeSuccess()
    {
        List<EmployeeDto> employeesDtos = new List<EmployeeDto>
        {
            _employee
        };
        _employeeMockService.Setup(service => service.GetEmployeesByType("Developer")).ReturnsAsync(employeesDtos);

        var result = await _controller.FilterByType("Developer");

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(employeesDtos, okObjectResult.Value!);
    }

    [Fact]
    public async Task FilterByTypeFail()
    { 
        _employeeMockService.Setup(service => service.GetEmployeesByType("HR"))
            .ThrowsAsync(new Exception());
        var result = await _controller.FilterByType("HR");
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }
}
