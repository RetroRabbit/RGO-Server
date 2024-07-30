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
using RR.Tests.Data;
using HRIS.Services.Services;
using RR.Tests.Data.Models.HRIS;
using RR.App.Tests.Helper;


namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeControllerUnitTests
{
    private readonly Mock<IChartService> _chartMockService;
    private readonly EmployeeController _controller;
    private readonly EmployeeController _controllers;

    private readonly List<EmployeeDto> _employeeDtoList;
    private readonly EmployeeDto _employeeDto;
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly EmployeeDto _employee;
    private readonly EmployeeFilterResponse _employeeFilter;
    private readonly SimpleEmployeeProfileDto _simpleEmployee;
    private readonly Mock<IEmployeeService> _employeeMockService;
    private readonly EmployeeAddressDto _employeeAddressDto;
    private readonly EmployeeTypeDto _employeeTypeDto;
    private readonly List<Claim> _claims;
    private readonly ClaimsPrincipal _claimsPrincipal;
    private readonly ClaimsIdentity _claimsIdentity;
    private readonly SimpleEmployeeProfileDto _simpleEmployeeProfileDto;
    private readonly Mock<AuthorizeIdentityMock> _identity;

    public EmployeeControllerUnitTests()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeMockService = new Mock<IEmployeeService>();

        _identity = new Mock<AuthorizeIdentityMock>();

        _controller = new EmployeeController(_identity.Object, _employeeMockService.Object);

        _employeeDtoList = new List<EmployeeDto>
        {
            EmployeeTestData.EmployeeOne.ToDto(),
            EmployeeTestData.EmployeeTwo.ToDto(),
            EmployeeTestData.EmployeeThree.ToDto(),
            EmployeeTestData.EmployeeFour.ToDto(),
            EmployeeTestData.EmployeeNew.ToDto(),
            EmployeeTestData.EmployeeSix.ToDto(),
        };
        _employeeDto = EmployeeTestData.EmployeeOne.ToDto();

        _employeeFilter = new EmployeeFilterResponse
        {
            Email = _employeeDto.Email,
            EngagementDate = _employeeDto.EngagementDate,
            InactiveReason = _employeeDto.InactiveReason,
            TerminationDate = _employeeDto.TerminationDate,
            ClientAllocated = "Test",
            Id = 1,
            RoleId = 1,
            RoleDescription = "Test Role",
            Level = _employeeDto.Level,
            Name = _employeeDto.Name,
            Position = "Test Position",
            Surname = _employeeDto.Surname
        };

        _simpleEmployeeProfileDto = new SimpleEmployeeProfileDto(_employeeDto);

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
        _employeeMockService.Setup(service => service.CreateEmployee(_employeeDto))
                            .ReturnsAsync(_employeeDto);

        var result = await _controller.AddEmployee(_employeeDto);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("AddEmployee", createdAtActionResult.ActionName);
        Assert.Equal(201, createdAtActionResult.StatusCode);
    }

    [Fact]
    public async Task GetEmployeeByEmailSuccessTest()
    {
        var principal = SetupClaimsProncipal(_employeeDto.Email!);
        SetupControllerContext(_controller, principal);

        _employeeMockService.Setup(service => service.GetEmployeeByEmail(It.IsAny<string>()))
                            .ReturnsAsync(_employeeDto);

        var result = await _controller.GetEmployeeByEmail(null);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
    }

    [Fact]
    public async Task GetEmployeeSuccessTest()
    {
        var principal = SetupClaimsProncipal(_employeeDto.Email!);
        SetupControllerContext(_controller, principal);

        _employeeMockService.Setup(service => service.GetEmployeeByEmail(It.IsAny<string>()))
                            .ReturnsAsync(_employeeDto);

        var result = await _controller.GetEmployeeByEmail(_employeeDto.Email);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(_employeeDto, (EmployeeDto)okObjectResult.Value!);
    }

    [Fact]
    public async Task UpdateEmployeeSuccessTest()
    {
        _identity.SetupGet(i => i.Role).Returns("SuperAdmin");
        _identity.SetupGet(i => i.EmployeeId).Returns(2);
        _employeeMockService.Setup(x => x.UpdateEmployee(_employeeDto))
                               .ReturnsAsync(_employeeDto);

        var result = await _controller.UpdateEmployee(_employeeDto);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);

        Assert.Equal(nameof(EmployeeController.UpdateEmployee), createdAtActionResult.ActionName);
        Assert.Equal(201, createdAtActionResult.StatusCode);
        Assert.Equal(_employeeDto.Email, createdAtActionResult.RouteValues["email"]);
        Assert.Equal(_employeeDto, createdAtActionResult.Value);
    }

    [Fact]
    public async Task UpdateEmployeeUnauthorized()
    {
        _identity.SetupGet(i => i.Role).Returns("Developer");
        _identity.SetupGet(i => i.EmployeeId).Returns(5);
        _employeeMockService.Setup(service => service.UpdateEmployee(_employeeDto))
                            .ThrowsAsync(new CustomException("Unauthorized action."));

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await _controller.UpdateEmployee(_employeeDto));
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal("Unauthorized action.", notFoundResult.Value);
    }

    [Fact]
    public async Task GetAllEmployeesSuccessTest()
    {
        _identity.SetupGet(i => i.Email).Returns("test@retrorabbit.co.za");
        _employeeMockService.Setup(service => service.GetAll(_employeeDto.Email))
                            .ReturnsAsync(_employeeDtoList);

        var result = await _controller.GetAllEmployees();
        var okObjectResult = Assert.IsType<OkObjectResult>(result);

        Assert.Equal(200, okObjectResult.StatusCode);
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
    public async Task GetEmployeeByIdSuccessTest()
    {
        var expectedDetails = _employeeDto;
        _employeeMockService.Setup(x => x.GetEmployeeById(_employeeDto.Id)).ReturnsAsync(expectedDetails);

        var result = await _controller.GetEmployeeById(_employeeDto.Id);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualDetails = Assert.IsType<EmployeeDto>(okResult.Value);

        Assert.Equal(expectedDetails, actualDetails);
    }

    [Fact]
    public async Task GetSimpleEmployeeSuccess()
    {
        _identity.SetupGet(i => i.Role).Returns("SuperAdmin");
        _identity.SetupGet(i => i.EmployeeId).Returns(2);
        _employeeMockService.Setup(service => service.GetSimpleProfile(It.IsAny<string>())).ReturnsAsync(_simpleEmployeeProfileDto);

        var result = await _controller.GetSimpleEmployee(_simpleEmployeeProfileDto.Email!);
        var simpleEmployee = (ObjectResult)result;

        Assert.Equal(_simpleEmployeeProfileDto, simpleEmployee.Value);
    }

    [Fact]
    public async Task GetSimpleEmployeeFail()
    {

        _identity.Setup(identity => identity.Role).Returns("Developer");
        _identity.Setup(identity => identity.EmployeeId).Returns(5);

        _employeeMockService.Setup(service => service.GetSimpleProfile(It.IsAny<string>()))
                            .ThrowsAsync(new CustomException("User data being accessed does not match user making the request."));

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await _controller.GetSimpleEmployee(_simpleEmployeeProfileDto.Email!));

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("User data being accessed does not match user making the request.", notFoundResult.Value);
    }

    [Fact]
    public async Task FilterEmployeesSuccessTest()
    {
        _employeeMockService.Setup(service => service.FilterEmployees(1, 0, true))
                            .ReturnsAsync(new List<EmployeeFilterResponse> { _employeeFilter });

        var result = await _controller.FilterEmployees(1, 0);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(new List<EmployeeFilterResponse> { _employeeFilter }, (List<EmployeeFilterResponse>)okObjectResult.Value!);
    }

    [Fact]
    public async Task FilterEmployeesFailTest()
    {
        _employeeMockService.Setup(service => service.FilterEmployees(-1, -1, true))
                            .ThrowsAsync(new CustomException("An error occured while filtering employees"));

        var exception = await Assert.ThrowsAsync<CustomException>(async () =>
            await _controller.FilterEmployees(-1, -1));

        Assert.Equal("An error occured while filtering employees", exception.Message);
    }

    [Fact]
    public async Task DeleteEmployeeSuccessTest()
    {
        var principal = SetupClaimsProncipal(_employeeDto.Email!);
        SetupControllerContext(_controller, principal);

        _employeeMockService.Setup(service => service.DeleteEmployee(It.IsAny<string>()))
                            .ReturnsAsync(_employeeDto);

        var result = await _controller.DeleteEmployee(_employeeDto.Email);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(_employeeDto, (EmployeeDto)okObjectResult.Value!);
    }

    [Fact]
    public async Task CheckIdNumberSuccessTest()
    {
        _identity.SetupGet(i => i.Role).Returns("SuperAdmin");
        _identity.SetupGet(i => i.EmployeeId).Returns(2);
        _employeeMockService.Setup(service => service.CheckDuplicateIdNumber("0000080000000", 1))
                                .ReturnsAsync(true);

        var result = await _controller.CheckIdNumber("0000080000000", 1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.True((bool)okResult.Value!);
    }

    [Fact]
    public async Task CheckIdNumberUserRoleNotAuthorized()
    {
        _identity.Setup(identity => identity.Role).Returns("Developer");
        _identity.Setup(identity => identity.EmployeeId).Returns(5);

        _employeeMockService.Setup(service => service.CheckDuplicateIdNumber("0000080000000", 1))
                            .ThrowsAsync(new CustomException("User data being accessed does not match user making the request."));

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await _controller.CheckIdNumber("0000080000000", 1));

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("User data being accessed does not match user making the request.", notFoundResult.Value);
    }
}
