using System.Security.Claims;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.UnitOfWork;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeControllerUnitTests
{
    private readonly Mock<IChartService> _chartMockService;
    private readonly EmployeeController _controller;
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly EmployeeDto _employee;
    private readonly Mock<IEmployeeService> _employeeMockService;

    private readonly EmployeeAddressDto employeeAddressDto =
        new(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");

    private readonly EmployeeTypeDto employeeTypeDto = new EmployeeTypeDto { Id = 1, Name = "Developer" };

    private readonly List<Claim> claims;
    private readonly ClaimsPrincipal claimsPrincipal;
    private readonly ClaimsIdentity identity;

    public EmployeeControllerUnitTests()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeMockService = new Mock<IEmployeeService>();
        _chartMockService = new Mock<IChartService>();
        _controller = new EmployeeController(_employeeMockService.Object, _chartMockService.Object);

        _employee = new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
                                    null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Kamo",
                                    "K.G.",
                                    "Smith", new DateTime(), "South Africa", "South African", "1234457899", " ",
                                    new DateTime(), null, Race.Black, Gender.Female, null!,
                                    "ksmith@retrorabbit.co.za", "kmaosmith@gmail.com", "0123456789", null, null,
                                    employeeAddressDto, employeeAddressDto, null, null, null);

        claims = new List<Claim>
        {
            new(ClaimTypes.Email, "ksmith@retrorabbit.co.za")
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
            new(ClaimTypes.Email, email)
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
        var principal = SetupClaimsProncipal(_employee.Email!);
        SetupControllerContext(_controller, principal);

        _employeeMockService.Setup(service => service.GetEmployee(It.IsAny<string>()))
                            .ReturnsAsync(_employee);

        var result = await _controller.GetEmployeeByEmail(null);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
    }

    [Fact]
    public async Task GetEmployeeSuccessTest()
    {
        var principal = SetupClaimsProncipal(_employee.Email!);
        SetupControllerContext(_controller, principal);

        _employeeMockService.Setup(service => service.GetEmployee(It.IsAny<string>()))
                            .ReturnsAsync(_employee);

        var result = await _controller.GetEmployeeByEmail(_employee.Email);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(_employee, (EmployeeDto)okObjectResult.Value!);
    }

    [Fact]
    public async Task GetEmployeeFailTest()
    {
        var principal = SetupClaimsProncipal(_employee.Email!);
        SetupControllerContext(_controller, principal);

        _employeeMockService.Setup(service => service.GetEmployee(It.IsAny<string>()))
                            .ThrowsAsync(new Exception("Not found"));

        var result = await _controller.GetEmployeeByEmail(_employee.Email);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task UpdateEmployeeSuccessTest()
    {
        _employeeMockService.Setup(service => service.UpdateEmployee(_employee, _employee.Email!))
                            .ReturnsAsync(_employee);

        var result = await _controller.UpdateEmployee(_employee, "ksmith@retrorabbit.co.za");

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("UpdateEmployee", createdAtActionResult.ActionName);
        Assert.Equal(201, createdAtActionResult.StatusCode);
    }

    [Fact]
    public async Task UpdateEmployeeFailTest()
    {
        _employeeMockService.Setup(service => service.UpdateEmployee(_employee, _employee.Email!))
                            .ThrowsAsync(new Exception("Not found"));

        var result = await _controller.UpdateEmployee(_employee, "ksmith@retrorabbit.co.za");

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task UpdateEmployeeUnauthorized()
    {
        _employeeMockService.Setup(service => service.UpdateEmployee(_employee, _employee.Email!))
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
    public async Task GetEmployeeCountSuccessTest()
    {
        var expectedCount = new EmployeeCountDataCard { EmployeeTotalDifference = 42 };
        _employeeMockService.Setup(service => service.GenerateDataCardInformation())
                            .ReturnsAsync(expectedCount);

        var result = await _controller.GetEmployeesCount();

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(expectedCount.EmployeeTotalDifference, ((EmployeeCountDataCard)okObjectResult.Value!).EmployeeTotalDifference);
    }

    [Fact]
    public async Task GetEmployeesCountFailTest()
    {
        _employeeMockService.Setup(service => service.GenerateDataCardInformation())
                            .ThrowsAsync(new Exception("Failed to generate data card"));

        var result = await _controller.GetEmployeesCount();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.Equal("Failed to generate data card", notFoundResult.Value);
    }

    [Fact]
    public async Task GetEmployeeByIdSuccessTest()
    {
        var expectedDetails = _employee;
        _employeeMockService.Setup(x => x.GetEmployeeById(_employee.Id)).ReturnsAsync(expectedDetails);

        var result = await _controller.GetEmployeeById(_employee.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDetails = Assert.IsType<EmployeeDto>(okResult.Value);

        Assert.Equal(expectedDetails, actualDetails);
    }

    [Fact]
    public async Task GetEmployeeByIdFailTest()
    {
        var expectedDetails = _employee;
        _employeeMockService.Setup(s => s.GetEmployeeById(_employee.Id))
                            .ThrowsAsync(new Exception("Not found"));

        var result = await _controller.GetEmployeeById(_employee.Id);

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
        var principal = SetupClaimsProncipal(_employee.Email!);
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
        SimpleEmployeeProfileDto employee = new SimpleEmployeeProfileDto
        {
            Id = 1,
            EmployeeNumber = "1",
            TaxNumber = "123123",
            EngagementDate = new DateTime(),
            Disability = false,
            DisabilityNotes = "",
            Level = 3,
            EmployeeType = employeeTypeDto,
            Name = "John",
            Initials = "J",
            Surname = "Doe",
            DateOfBirth = new DateTime(),
            IdNumber = "123",
            Email = "ksmith@retrorabbit.co.za",
            PersonalEmail = "ba@gmail.com",
            CellphoneNo = "123",
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto
        };

        _employeeMockService.Setup(service => service.GetSimpleProfile(It.IsAny<string>())).ReturnsAsync(employee);

        var result = await _controller.GetSimpleEmployee(employee.Email!);

        var simpleEmployee = (ObjectResult)result;

        Assert.Equal(employee, simpleEmployee.Value);
    }

    [Fact]
    public async Task GetSimpleEmployeeFail()
    {
        SimpleEmployeeProfileDto employee = new SimpleEmployeeProfileDto
        {
            Id = 1,
            EmployeeNumber = "1",
            TaxNumber = "123123",
            EngagementDate = new DateTime(),
            Disability = false,
            DisabilityNotes = "",
            Level = 3,
            EmployeeType = employeeTypeDto,
            Name = "John",
            Initials = "J",
            Surname = "Doe",
            DateOfBirth = new DateTime(),
            IdNumber = "123",
            Email = "ksmith@retrorabbit.co.za",
            PersonalEmail = "ba@gmail.com",
            CellphoneNo = "123",
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto
        };

        _employeeMockService.Setup(service => service.GetSimpleProfile(It.IsAny<string>()))
                            .ThrowsAsync(new Exception("Not Found"));

        var result = await _controller.GetSimpleEmployee(employee.Email!);

        var simpleEmployee = (NotFoundObjectResult)result;

        Assert.Equal("Not Found", simpleEmployee.Value);
    }

    [Fact]
    public async Task FilterEmployeesSuccessTest()
    {
        _employeeMockService.Setup(service => service.FillerEmployees(1, 0))
                            .ReturnsAsync(new List<EmployeeDto> { _employee });

        var result = await _controller.FilterEmployees(1, 0);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(new List<EmployeeDto> { _employee }, (List<EmployeeDto>)okObjectResult.Value!);
    }

    [Fact]
    public async Task FilterEmployeesFailTest()
    {
        _employeeMockService.Setup(service => service.FillerEmployees(-1, -1))
                            .ThrowsAsync(new Exception("Not found"));

        var result = await _controller.FilterEmployees(-1, -1);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetChurnRateSuccessTest()
    {
        var expectedChurnRate = new ChurnRateDataCard { ChurnRate = 0.15 };
        _employeeMockService.Setup(service => service.CalculateEmployeeChurnRate())
                            .ReturnsAsync(expectedChurnRate);

        var result = await _controller.GetChurnRate();

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(expectedChurnRate.ChurnRate, ((ChurnRateDataCard)okObjectResult.Value!).ChurnRate);
    }

    [Fact]
    public async Task GetChurnRateFailTest()
    {
        _employeeMockService.Setup(service => service.CalculateEmployeeChurnRate())
                            .ThrowsAsync(new Exception("Failed to calculate churn rate"));

        var result = await _controller.GetChurnRate();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.Equal("Failed to calculate churn rate", notFoundResult.Value);
    }
}
