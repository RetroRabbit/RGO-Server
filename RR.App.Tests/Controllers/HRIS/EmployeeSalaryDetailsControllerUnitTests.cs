using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeSalaryDetailsControllerUnitTest
{
    private readonly Mock<IEmployeeSalaryDetailsService> _employeeSalaryDetailsServiceMock;
    private readonly EmployeeSalaryDetailsController _controller;
    private readonly Mock<IEmployeeService> _employeeServiceMock;

    private readonly EmployeeSalaryDetailsDto _employeeSalaryDetailsDto;
    private readonly EmployeeDto _employeeDto;

    public EmployeeSalaryDetailsControllerUnitTest()
    {
        _employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalaryDetailsService>();
        _employeeServiceMock = new Mock<IEmployeeService>();
        _controller = new EmployeeSalaryDetailsController(new AuthorizeIdentityMock("test@example.com", "TestUser", "SuperAdmin", 1), _employeeSalaryDetailsServiceMock.Object);
        _employeeSalaryDetailsDto = EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto();
        _employeeDto = EmployeeTestData.EmployeeOne.ToDto();

    }

    [Fact]
    public async Task SaveEmployeeSalaryValidInputReturnsOkResult()
    {
        _employeeServiceMock.Setup(x => x.GetEmployeeById(_employeeSalaryDetailsDto.EmployeeId))
                            .ReturnsAsync(_employeeDto);

        _employeeSalaryDetailsServiceMock.Setup(x => x.CreateEmployeeSalary(It.IsAny<EmployeeSalaryDetailsDto>()))
                                         .ReturnsAsync(_employeeSalaryDetailsDto);

        var result = await _controller.AddEmployeeSalary(_employeeSalaryDetailsDto);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("AddEmployeeSalary", createdAtActionResult.ActionName);
        Assert.Equal(_employeeSalaryDetailsDto.EmployeeId, createdAtActionResult.RouteValues["employeeId"]);
        Assert.Equal(_employeeSalaryDetailsDto, createdAtActionResult.Value);
    }

    [Fact]
    public async Task AddEmployeeSalary_UnauthorizedRoleOrIdMismatch_ThrowsCustomException()
    {
        var unauthorizedIdentity = new AuthorizeIdentityMock("unauthorized@example.com", "UnauthorizedUser", "User", 2);
        var controller = new EmployeeSalaryDetailsController(unauthorizedIdentity, _employeeSalaryDetailsServiceMock.Object);

        var exception = await Assert.ThrowsAsync<CustomException>(() => controller.AddEmployeeSalary(_employeeSalaryDetailsDto));

        Assert.Equal("User data being accessed does not match user making the request.", exception.Message);
    }

    [Fact]
    public async Task AddEmployeeSalary_ValidRoleAndMatchingId_ReturnsCreatedAtActionResult()
    {
        _employeeSalaryDetailsServiceMock.Setup(x => x.CreateEmployeeSalary(It.IsAny<EmployeeSalaryDetailsDto>()))
                                         .ReturnsAsync(_employeeSalaryDetailsDto);

        var result = await _controller.AddEmployeeSalary(_employeeSalaryDetailsDto);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("AddEmployeeSalary", createdAtActionResult.ActionName);
        Assert.Equal(_employeeSalaryDetailsDto.EmployeeId, createdAtActionResult.RouteValues["employeeId"]);
        Assert.Equal(_employeeSalaryDetailsDto, createdAtActionResult.Value);
    }

    [Fact]
    public async Task DeleteEmployeeSalaryValidInputReturnsOkResult()
    {
        _employeeServiceMock.Setup(x => x.GetEmployeeById(_employeeSalaryDetailsDto.EmployeeId))
                           .ReturnsAsync(_employeeDto);

        _employeeSalaryDetailsServiceMock.Setup(x => x.DeleteEmployeeSalary(_employeeSalaryDetailsDto.Id))
                                        .ReturnsAsync(_employeeSalaryDetailsDto);

        var result = await _controller.DeleteSalary(_employeeSalaryDetailsDto.Id);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task UpdateEmployeeSalaryValidInputReturnsOkResult()
    {
        _employeeServiceMock.Setup(x => x.GetEmployeeById(_employeeSalaryDetailsDto.EmployeeId))
                            .ReturnsAsync(_employeeDto);

        _employeeSalaryDetailsServiceMock.Setup(x => x.UpdateEmployeeSalary(_employeeSalaryDetailsDto))
                                         .ReturnsAsync(_employeeSalaryDetailsDto);

        var result = await _controller.UpdateSalary(_employeeSalaryDetailsDto);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task UpdateEmployeeSalary_UnauthorizedRoleOrIdMismatch_ThrowsCustomException()
    {
        var unauthorizedIdentity = new AuthorizeIdentityMock("unauthorized@example.com", "UnauthorizedUser", "User", 2);
        var controller = new EmployeeSalaryDetailsController(unauthorizedIdentity, _employeeSalaryDetailsServiceMock.Object);

        var exception = await Assert.ThrowsAsync<CustomException>(() => controller.UpdateSalary(_employeeSalaryDetailsDto));
        Assert.Equal("User data being accessed does not match user making the request.", exception.Message);
    }

    [Fact]
    public async Task GetSalariesByEmployeePass()
    {
        _employeeSalaryDetailsServiceMock.Setup(x => x.GetEmployeeSalaryById(_employeeSalaryDetailsDto.EmployeeId))
                                         .ReturnsAsync(_employeeSalaryDetailsDto);

        var result = await _controller.GetEmployeeSalary(_employeeSalaryDetailsDto.EmployeeId);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualSalaryDetailsDto = Assert.IsType<EmployeeSalaryDetailsDto>(okResult.Value);
        Assert.Equal(_employeeSalaryDetailsDto, actualSalaryDetailsDto);
    }

    [Fact]
    public async Task GetEmployeeSalary_ValidRoleAndMatchingId_ReturnsOkObjectResult()
    {
        _employeeSalaryDetailsServiceMock.Setup(x => x.GetEmployeeSalaryById(_employeeSalaryDetailsDto.EmployeeId))
                                         .ReturnsAsync(_employeeSalaryDetailsDto);

        var result = await _controller.GetEmployeeSalary(_employeeSalaryDetailsDto.EmployeeId);
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(_employeeSalaryDetailsDto, okResult.Value);
    }

    [Fact]
    public async Task GetEmployeeSalary_UnauthorizedRoleOrIdMismatch_ThrowsCustomException()
    {
        var unauthorizedIdentity = new AuthorizeIdentityMock("unauthorized@example.com", "UnauthorizedUser", "User", 2);
        var controller = new EmployeeSalaryDetailsController(unauthorizedIdentity, _employeeSalaryDetailsServiceMock.Object);

        var exception = await Assert.ThrowsAsync<CustomException>(() => controller.GetEmployeeSalary(_employeeSalaryDetailsDto.EmployeeId));
        Assert.Equal("User data being accessed does not match user making the request.", exception.Message);
    }

    [Fact]
    public async Task GetAllEmployeeSalariesNoFiltersReturnsOkResultWithList()
    {
        var expectedSalaryDetailsList = new List<EmployeeSalaryDetailsDto>
    {
        _employeeSalaryDetailsDto,
        _employeeSalaryDetailsDto
    };

        _employeeSalaryDetailsServiceMock.Setup(x => x.GetAllEmployeeSalaries())
                                         .ReturnsAsync(expectedSalaryDetailsList);

        var result = await _controller.GetAllEmployeeSalaries();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualSalaryDetailsDto = Assert.IsType<List<EmployeeSalaryDetailsDto>>(okResult.Value);
        Assert.Equal(expectedSalaryDetailsList, actualSalaryDetailsDto);
    }
}