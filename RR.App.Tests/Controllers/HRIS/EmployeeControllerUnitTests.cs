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
using AutoMapper;


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
    private readonly EmployeeProfileDto _employeeProfileDto;
    private readonly Mock<IEmployeeService> _employeeMockService;
    private readonly EmployeeAddressDto _employeeAddressDto;
    private readonly EmployeeTypeDto _employeeTypeDto;
    private readonly List<Claim> _claims;
    private readonly ClaimsPrincipal _claimsPrincipal;
    private readonly ClaimsIdentity _claimsIdentity;
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

        _employeeProfileDto = new EmployeeProfileDto
        { 
            CellphoneNo = _employeeDto.CellphoneNo,
            ClientAllocatedId = _employeeDto.ClientAllocated,
            CountryOfBirth = _employeeDto.CountryOfBirth,
            DateOfBirth = _employeeDto.DateOfBirth,
            Disability = _employeeDto.Disability,
            DisabilityNotes = _employeeDto.DisabilityNotes,
            Email = _employeeDto.Email,
            EmergencyContactName = _employeeDto.EmergencyContactName,
            EmergencyContactNo = _employeeDto.EmergencyContactNo,
            EmployeeNumber = _employeeDto.EmployeeNumber,
            EmployeeType = _employeeDto.EmployeeType,
            EngagementDate = _employeeDto.EngagementDate,
            Gender = _employeeDto.Gender,
            HouseNo = _employeeDto.HouseNo,
            Id = _employeeDto.Id,
            IdNumber = _employeeDto.IdNumber,
            Initials = _employeeDto.Initials,
            LeaveInterval = _employeeDto.LeaveInterval,
            Level = _employeeDto.Level,
            Name = _employeeDto.Name,
            Nationality = _employeeDto.Nationality,
            Notes = _employeeDto.Notes,
            PassportCountryIssue = _employeeDto.PassportCountryIssue,
            PassportExpirationDate = _employeeDto.PassportExpirationDate,
            PassportNumber = _employeeDto.PassportNumber,
            PayRate = _employeeDto.PayRate,
            PeopleChampionId = _employeeDto.PeopleChampion,
            PersonalEmail = _employeeDto.PersonalEmail,
            Photo = _employeeDto.Photo,
            Race = _employeeDto.Race,
            Salary = _employeeDto.Salary,
            SalaryDays = _employeeDto.SalaryDays,
            Surname = _employeeDto.Surname,
            TaxNumber = _employeeDto.TaxNumber,
            TeamLeadId = _employeeDto.TeamLead,
            TerminationDate = _employeeDto.TerminationDate,
             
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
    public async Task UpdateEmployeeSuccessTest()
    {
        _identity.SetupGet(i => i.Role).Returns("SuperAdmin");
        _identity.SetupGet(i => i.EmployeeId).Returns(2);
        _employeeMockService.Setup(x => x.UpdateEmployee(_employeeProfileDto))
                               .ReturnsAsync(_employeeDto);

        var result = await _controller.UpdateEmployee(_employeeProfileDto);
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
        _employeeMockService.Setup(service => service.UpdateEmployee(_employeeProfileDto))
                            .ThrowsAsync(new CustomException("Unauthorized action."));

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await _controller.UpdateEmployee(_employeeProfileDto));
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal("Unauthorized action.", notFoundResult.Value);
    }

    [Fact]
    public async Task GetAllEmployeeProfilesSuccessTest()
    {
        _identity.SetupGet(i => i.Email).Returns("test@retrorabbit.co.za");
        _employeeMockService.Setup(service => service.GetAll(_employeeDto.Email))
                            .ReturnsAsync(_employeeDtoList);

        var result = await _controller.GetAllEmployeeProfiles();
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
    public async Task GetEmployeeProfileSuccess()
    {
        _identity.SetupGet(i => i.Role).Returns("SuperAdmin");
        _identity.SetupGet(i => i.EmployeeId).Returns(2);
        _employeeMockService.Setup(service => service.GetEmployeeProfile(It.IsAny<string>())).ReturnsAsync(_employeeProfileDto);

        var result = await _controller.GetEmployeeProfile(_employeeProfileDto.Email!);
        var simpleEmployee = (ObjectResult)result;

        Assert.Equal(_employeeProfileDto, simpleEmployee.Value);
    }

    [Fact]
    public async Task GetEmployeeProfileFail()
    {

        _identity.Setup(identity => identity.Role).Returns("Developer");
        _identity.Setup(identity => identity.EmployeeId).Returns(5);

        _employeeMockService.Setup(service => service.GetEmployeeProfile(It.IsAny<string>()))
                            .ThrowsAsync(new CustomException("User data being accessed does not match user making the request."));

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await _controller.GetEmployeeProfile(_employeeProfileDto.Email!));

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
    public async Task CheckIdNumberSuccessTest()
    {
        _identity.SetupGet(i => i.Role).Returns("SuperAdmin");
        _identity.SetupGet(i => i.EmployeeId).Returns(2);
        _employeeMockService.Setup(service => service.CheckDuplicateIdNumber("0000080000000", 1, true))
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

        _employeeMockService.Setup(service => service.CheckDuplicateIdNumber("0000080000000", 1, true))
            .ThrowsAsync(new CustomException("No permission or user id already exists."));

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await _controller.CheckIdNumber("0000080000000", 1));

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("No permission or user id already exists.", notFoundResult.Value);
    }
}
