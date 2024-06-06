using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data.Models.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeSalaryDetailsControllerUnitTest
{
    public EmployeeSalaryDetailsControllerUnitTest()
    {}

    [Fact]
    public async Task SaveEmployeeSalaryValidInputReturnsOkResult()
    {
        var employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalarayDetailsService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeSalaryDetailsController(employeeSalaryDetailsServiceMock.Object);
        var employeeSalaryDetailsDto = EmployeeSalaryDetailsTestData.EmployeeSalaryTest1;

        employeeServiceMock.Setup(x => x.GetEmployeeById(employeeSalaryDetailsDto.EmployeeId))
                           .ReturnsAsync(EmployeeTestData.EmployeeDto);

        employeeSalaryDetailsServiceMock.Setup(x => x.SaveEmployeeSalary(It.IsAny<EmployeeSalaryDetailsDto>()))
                                        .ReturnsAsync(employeeSalaryDetailsDto);

        var result = await controller.AddEmployeeSalary(employeeSalaryDetailsDto);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(nameof(controller.AddEmployeeSalary), createdAtActionResult.ActionName);
        Assert.Equal(employeeSalaryDetailsDto.EmployeeId, createdAtActionResult.RouteValues["employeeId"]);
        Assert.Equal(employeeSalaryDetailsDto, createdAtActionResult.Value);
    }

    [Fact]
    public async Task SaveEmployeeSalaryExceptionThrownReturnsNotFoundWithMessage()
    {
        var employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalarayDetailsService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeSalaryDetailsController(employeeSalaryDetailsServiceMock.Object);

        employeeSalaryDetailsServiceMock.Setup(x => x.SaveEmployeeSalary(It.IsAny<EmployeeSalaryDetailsDto>()))
                               .ThrowsAsync(new Exception("An error occurred while saving employee salary information."));

        var result = await controller.AddEmployeeSalary(EmployeeSalaryDetailsTestData.EmployeeSalaryTest1);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while saving employee salary information.", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteEmployeeSalaryValidInputReturnsOkResult()
    {
        var employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalarayDetailsService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeSalaryDetailsController(employeeSalaryDetailsServiceMock.Object);
        var employeeSalaryDetailsDto = EmployeeSalaryDetailsTestData.EmployeeSalaryTest1;

        employeeServiceMock.Setup(x => x.GetEmployeeById(employeeSalaryDetailsDto.EmployeeId))
                           .ReturnsAsync(EmployeeTestData.EmployeeDto);

        employeeSalaryDetailsServiceMock.Setup(x => x.DeleteEmployeeSalary(employeeSalaryDetailsDto.Id))
                                        .Returns(Task.FromResult(employeeSalaryDetailsDto));

        var result = await controller.DeleteSalary(employeeSalaryDetailsDto.Id);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task DeleteEmployeeSalaryExceptionThrownReturnsNotFoundWithMessage()
    {
        var employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalarayDetailsService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeSalaryDetailsController(employeeSalaryDetailsServiceMock.Object);
        var employeeSalaryDetailsDto = EmployeeSalaryDetailsTestData.EmployeeSalaryTest1;

        employeeSalaryDetailsServiceMock.Setup(x => x.DeleteEmployeeSalary(employeeSalaryDetailsDto.Id))
                               .ThrowsAsync(new Exception("An error occurred while deleting employee salary information."));

        var result = await controller.DeleteSalary(employeeSalaryDetailsDto.Id);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while deleting employee salary information.", notFoundResult.Value);
    }

    [Fact]
    public async Task UpdateEmployeeSalaryValidInputReturnsOkResult()
    {
        var employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalarayDetailsService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeSalaryDetailsController(employeeSalaryDetailsServiceMock.Object);
        var employeeSalaryDetailsDto = EmployeeSalaryDetailsTestData.EmployeeSalaryTest1;

        employeeServiceMock.Setup(x => x.GetEmployeeById(employeeSalaryDetailsDto.EmployeeId))
                           .ReturnsAsync(EmployeeTestData.EmployeeDto);

        employeeSalaryDetailsServiceMock.Setup(x => x.UpdateEmployeeSalary(employeeSalaryDetailsDto))
                                        .ReturnsAsync(employeeSalaryDetailsDto);

        var result = await controller.UpdateSalary(employeeSalaryDetailsDto);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task UpdateEmployeeSalaryExceptionThrownReturnsNotFoundWithMessage()
    {
        var employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalarayDetailsService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeSalaryDetailsController(employeeSalaryDetailsServiceMock.Object);

        employeeSalaryDetailsServiceMock.Setup(x => x.UpdateEmployeeSalary(It.IsAny<EmployeeSalaryDetailsDto>()))
                               .ThrowsAsync(new Exception("An error occurred while updating employee salary information."));

        var result = await controller.UpdateSalary(EmployeeSalaryDetailsTestData.EmployeeSalaryTest1);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while updating employee salary information.", notFoundResult.Value);
    }

    [Fact]
    public async Task GetSalariesByEmployeePass()
    {
        var employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalarayDetailsService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeSalaryDetailsController(employeeSalaryDetailsServiceMock.Object);
        var employeeId = 1;
        var expectedSalaryDetailsDto = EmployeeSalaryDetailsTestData.EmployeeSalaryTest1;

        employeeSalaryDetailsServiceMock.Setup(x => x.GetEmployeeSalary(employeeId))
                                        .ReturnsAsync(expectedSalaryDetailsDto);

        var result = await controller.GetEmployeeSalary(employeeId);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualSalaryDetailsDto = Assert.IsType<EmployeeSalaryDetailsDto>(okResult.Value);
        Assert.Equal(expectedSalaryDetailsDto, actualSalaryDetailsDto);
    }

    [Fact]
    public async Task GetAllEmployeeSalariesNoFiltersReturnsOkResultWithList()
    {
        var employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalarayDetailsService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeSalaryDetailsController(employeeSalaryDetailsServiceMock.Object);
        var expectedSalaryDetailsList = new List<EmployeeSalaryDetailsDto> { EmployeeSalaryDetailsTestData.EmployeeSalaryTest1 };
        employeeSalaryDetailsServiceMock.Setup(x => x.GetAllEmployeeSalaries()).ReturnsAsync(expectedSalaryDetailsList);
        var result = await controller.GetAllEmployeeSalaries();
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualSalaryDetailsDto = Assert.IsType<List<EmployeeSalaryDetailsDto>>(okResult.Value);
        Assert.Equal(expectedSalaryDetailsList, actualSalaryDetailsDto);
    }
}