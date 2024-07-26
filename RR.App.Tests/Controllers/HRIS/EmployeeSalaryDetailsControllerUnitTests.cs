using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeSalaryDetailsControllerUnitTest
{
    private readonly Mock<IEmployeeSalarayDetailsService> _employeeSalaryDetailsServiceMock;
    private readonly Mock<IEmployeeService> _employeeServiceMock;
    private readonly EmployeeSalaryDetailsController _controller;
    private readonly EmployeeSalaryDetailsDto _employeeSalaryDetailsDto;
    private readonly EmployeeDto _employeeDto;

    public EmployeeSalaryDetailsControllerUnitTest()
    {
        _employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalarayDetailsService>();
        _employeeServiceMock = new Mock<IEmployeeService>();
        _controller = new EmployeeSalaryDetailsController(new AuthorizeIdentityMock(), _employeeSalaryDetailsServiceMock.Object);
        _employeeSalaryDetailsDto = EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto();
        _employeeDto = EmployeeTestData.EmployeeOne.ToDto();
    }

    [Fact(Skip = "Current user needs to be set for validations on endpoint")]
    public async Task SaveEmployeeSalaryValidInputReturnsOkResult()
    {
        _employeeServiceMock.Setup(x => x.GetEmployeeById(_employeeSalaryDetailsDto.EmployeeId))
                           .ReturnsAsync(_employeeDto);

        _employeeSalaryDetailsServiceMock.Setup(x => x.CreateEmployeeSalary(It.IsAny<EmployeeSalaryDetailsDto>()))
                           .ReturnsAsync(_employeeSalaryDetailsDto);

        var result = await _controller.AddEmployeeSalary(_employeeSalaryDetailsDto);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(_controller.AddEmployeeSalary), createdAtActionResult.ActionName);
        Assert.Equal(_employeeSalaryDetailsDto.EmployeeId, createdAtActionResult.RouteValues["employeeId"]);
        Assert.Equal(_employeeSalaryDetailsDto, createdAtActionResult.Value);
    }

    [Fact(Skip = "Current user needs to be set for validations on endpoint")]
    public async Task SaveEmployeeSalaryExceptionThrownReturnsNotFoundWithMessage()
    {
        _employeeSalaryDetailsServiceMock.Setup(x => x.CreateEmployeeSalary(It.IsAny<EmployeeSalaryDetailsDto>()))
                               .ThrowsAsync(new Exception("An error occurred while saving employee salary information."));

        var result = await _controller.AddEmployeeSalary(EmployeeSalaryDetailsTestData.EmployeeSalaryDetailsOne.ToDto());

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while saving employee salary information.", notFoundResult.Value);
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
    public async Task DeleteEmployeeSalaryExceptionThrownReturnsNotFoundWithMessage()
    {
        _employeeSalaryDetailsServiceMock.Setup(x => x.DeleteEmployeeSalary(_employeeSalaryDetailsDto.Id))
                               .ThrowsAsync(new Exception("An error occurred while deleting employee salary information."));

        var result = await _controller.DeleteSalary(_employeeSalaryDetailsDto.Id);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while deleting employee salary information.", notFoundResult.Value);
    }

    [Fact(Skip = "Current user needs to be set for validations on endpoint")]
    public async Task UpdateEmployeeSalaryValidInputReturnsOkResult()
    {
        _employeeServiceMock.Setup(x => x.GetEmployeeById(_employeeSalaryDetailsDto.EmployeeId))
                           .ReturnsAsync(_employeeDto);

        _employeeSalaryDetailsServiceMock.Setup(x => x.UpdateEmployeeSalary(_employeeSalaryDetailsDto))
                                        .ReturnsAsync(_employeeSalaryDetailsDto);

        var result = await _controller.UpdateSalary(_employeeSalaryDetailsDto);

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact(Skip = "Current user needs to be set for validations on endpoint")]
    public async Task UpdateEmployeeSalaryExceptionThrownReturnsNotFoundWithMessage()
    {
        _employeeSalaryDetailsServiceMock.Setup(x => x.UpdateEmployeeSalary(It.IsAny<EmployeeSalaryDetailsDto>()))
                               .ThrowsAsync(new Exception("An error occurred while updating employee salary information."));

        var result = await _controller.UpdateSalary(_employeeSalaryDetailsDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while updating employee salary information.", notFoundResult.Value);
    }

    [Fact(Skip = "Current user needs to be set for validations on endpoint")]
    public async Task GetSalariesByEmployeePass()
    {
        _employeeSalaryDetailsServiceMock.Setup(x => x.GetEmployeeSalaryById(_employeeSalaryDetailsDto.Id))
                                        .ReturnsAsync(_employeeSalaryDetailsDto);

        var result = await _controller.GetEmployeeSalary(_employeeSalaryDetailsDto.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualSalaryDetailsDto = Assert.IsType<EmployeeSalaryDetailsDto>(okResult.Value);
        Assert.Equal(_employeeSalaryDetailsDto, actualSalaryDetailsDto);
    }

    [Fact]
    public async Task GetAllEmployeeSalariesNoFiltersReturnsOkResultWithList()
    {
        var expectedSalaryDetailsList = new List<EmployeeSalaryDetailsDto> 
        { 
            _employeeSalaryDetailsDto,
            _employeeSalaryDetailsDto
        };

        _employeeSalaryDetailsServiceMock.Setup(x => x.GetAllEmployeeSalaries()).ReturnsAsync(expectedSalaryDetailsList);

        var result = await _controller.GetAllEmployeeSalaries();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualSalaryDetailsDto = Assert.IsType<List<EmployeeSalaryDetailsDto>>(okResult.Value);
        Assert.Equal(expectedSalaryDetailsList, actualSalaryDetailsDto);
    }
}