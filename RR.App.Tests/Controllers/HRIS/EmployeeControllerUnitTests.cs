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
using Microsoft.Extensions.DependencyInjection;
using RR.Tests.Data;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeControllerUnitTests
{
    private readonly Mock<IChartService> _chartMockService;
    private readonly EmployeeController _controller;
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly EmployeeDto _employee;
    private readonly SimpleEmployeeProfileDto _simpleEmployee;
    private readonly Mock<IEmployeeService> _employeeMockService;
    private readonly EmployeeAddressDto _employeeAddressDto;
    private readonly EmployeeTypeDto _employeeTypeDto;
    private readonly List<Claim> _claims;
    private readonly ClaimsPrincipal _claimsPrincipal;
    private readonly ClaimsIdentity _claimsIdentity;
    private readonly SimpleEmployeeProfileDto _simpleEmployeeProfileDto;

    public EmployeeControllerUnitTests()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeMockService = new Mock<IEmployeeService>();

        _controller = new EmployeeController(new AuthorizeIdentityMock(), _employeeMockService.Object);

        _employee = new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "001",
            TaxNumber = "34434434",
            EngagementDate = new DateTime(),
            TerminationDate = new DateTime(),
            PeopleChampion = null,
            Disability = false,
            DisabilityNotes = "None",
            Level = 4,
            EmployeeType = _employeeTypeDto,
            Notes = "Notes",
            LeaveInterval = 1,
            SalaryDays = 28,
            PayRate = 128,
            Salary = 100000,
            Name = "Kamo",
            Initials = "K.G.",
            Surname = "Smith",
            DateOfBirth = new DateTime(),
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "1234457899",
            PassportNumber = " ",
            PassportExpirationDate = new DateTime(),
            PassportCountryIssue = "South Africa",
            Race = Race.Black,
            Gender = Gender.Female,
            Email = "ksmith@retrorabbit.co.za",
            PersonalEmail = "kmaosmith@gmail.com",
            CellphoneNo = "0123456789",
            PhysicalAddress = _employeeAddressDto,
            PostalAddress = _employeeAddressDto
        };

        _simpleEmployeeProfileDto = new SimpleEmployeeProfileDto{
            Id = 1,
            EmployeeNumber = "1",
            TaxNumber = "123123",
            EngagementDate = new DateTime(),
            Disability = false,
            DisabilityNotes = "",
            Level = 3,
            EmployeeType = _employeeTypeDto,
            Name = "John",
            Initials = "J",
            Surname = "Doe",
            DateOfBirth = new DateTime(),
            IdNumber = "123",
            Email = "ksmith@retrorabbit.co.za",
            PersonalEmail = "ba@gmail.com",
            CellphoneNo = "123",
            PhysicalAddress = _employeeAddressDto,
            PostalAddress = _employeeAddressDto
        };

        _claims = new List<Claim>
        {
            new(ClaimTypes.Email, "ksmith@retrorabbit.co.za")
        };

        _claimsIdentity = new ClaimsIdentity(_claims, "TestAuthType");
        _claimsPrincipal = new ClaimsPrincipal(_claimsIdentity);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = _claimsPrincipal }
        };

        _employeeAddressDto = new EmployeeAddressDto
        {
            Id = 1,
            UnitNumber = "2",
            ComplexName = "Complex",
            StreetNumber = "2",
            SuburbOrDistrict = "Suburb/District",
            City = "City",
            Country = "Country",
            Province = "Province",
            PostalCode = "1620"
        };

        _employeeTypeDto = new EmployeeTypeDto { Id = 1, Name = "Developer" };
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

    [Fact(Skip = "Current user needs to be set for validations on endpoint")]
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

    [Fact (Skip = "User data being accessed does not match user making the request.")]
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

    [Fact(Skip = "needs update")]
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

    [Fact(Skip = "needs update")]
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
        _employeeMockService.Setup(service => service.GetSimpleProfile(It.IsAny<string>())).ReturnsAsync(_simpleEmployeeProfileDto);

        var result = await _controller.GetSimpleEmployee(_simpleEmployeeProfileDto.Email!);

        var simpleEmployee = (ObjectResult)result;

        Assert.Equal(_simpleEmployeeProfileDto, simpleEmployee.Value);
    }

    [Fact]
    public async Task GetSimpleEmployeeFail()
    {
        _employeeMockService.Setup(service => service.GetSimpleProfile(It.IsAny<string>()))
                            .ThrowsAsync(new Exception("Not Found"));

        var result = await _controller.GetSimpleEmployee(_simpleEmployeeProfileDto.Email!);

        var simpleEmployee = (NotFoundObjectResult)result;

        Assert.Equal("Not Found", simpleEmployee.Value);
    }

    [Fact]
    public async Task FilterEmployeesSuccessTest()
    {
        _employeeMockService.Setup(service => service.FilterEmployees(1, 0,true))
                            .ReturnsAsync(new List<EmployeeDto> { _employee });

        var result = await _controller.FilterEmployees(1, 0);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(new List<EmployeeDto> { _employee }, (List<EmployeeDto>)okObjectResult.Value!);
    }

    [Fact]
    public async Task FilterEmployeesFailTest()
    {
        _employeeMockService.Setup(service => service.FilterEmployees(-1, -1, true))
                            .ThrowsAsync(new Exception("Not found"));

        var result = await _controller.FilterEmployees(-1, -1);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetChurnRateSuccessTest()
    {
        var expectedChurnRate = new ChurnRateDataCardDto { ChurnRate = 0.15 };
        _employeeMockService.Setup(service => service.CalculateEmployeeChurnRate())
                            .ReturnsAsync(expectedChurnRate);

        var result = await _controller.GetChurnRate();

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(expectedChurnRate.ChurnRate, ((ChurnRateDataCardDto)okObjectResult.Value!).ChurnRate);
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
