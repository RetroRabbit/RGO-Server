﻿//TODO: Complete Unit Tests

using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RGO.Tests.Data.Models;
using RR.App.Controllers.HRIS;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;
using System.Security.Claims;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeSalaryDetailsControllerUnitTest
{
    private readonly EmployeeSalaryDetailsController _employeeController;
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeSalarayDetailsService> _salaryService;

    private readonly EmployeeSalaryDetailsDto _employeeSalaryDetailsDto;
    private readonly List<EmployeeSalaryDetailsDto> _salaries;

    public EmployeeSalaryDetailsControllerUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _salaryService = new Mock<IEmployeeSalarayDetailsService>();
        _employeeController = new EmployeeSalaryDetailsController(_salaryService.Object);

        _employeeSalaryDetailsDto = new EmployeeSalaryDetailsDto
        {
            Id = 1,
            EmployeeId = 1,
            Salary = 2000,
            MinSalary = 1500,
            MaxSalary = 3000,
            Remuneration = 2500,
            Band = EmployeeSalaryBand.Level1,
            Contribution = null
        };

        _salaries = new List<EmployeeSalaryDetailsDto> { _employeeSalaryDetailsDto };

    }

    public async Task SaveEmployeeSalaryValidInputReturnsOkResult()
    {
        var employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalarayDetailsService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeSalaryDetailsController(employeeSalaryDetailsServiceMock.Object);

        var employeeSalaryDetailsDto = new EmployeeSalaryDetailsDto
        {
            Id = 1,
            EmployeeId = 1,
            Salary = 2000,
            MinSalary = 1500,
            MaxSalary = 3000,
            Remuneration = 2500,
            Band = EmployeeSalaryBand.Level1,
            Contribution = null
        };

        employeeServiceMock.Setup(x => x.GetEmployeeById(employeeSalaryDetailsDto.EmployeeId))
                           .ReturnsAsync(EmployeeTestData.EmployeeDto);

        employeeSalaryDetailsServiceMock.Setup(x => x.SaveEmployeeSalary(It.IsAny<EmployeeSalaryDetailsDto>()))
                               .Returns((Task<EmployeeSalaryDetailsDto>)Task.CompletedTask);

        var result = await controller.AddEmployeeSalary(employeeSalaryDetailsDto);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task SaveEmployeeSalaryExceptionThrownReturnsNotFoundWithMessage()
    {
        var employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalarayDetailsService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeSalaryDetailsController(employeeSalaryDetailsServiceMock.Object);

        var employeeSalaryDetailsDto = new EmployeeSalaryDetailsDto
        {
            Id = 1,
            EmployeeId = 1,
            Salary = 2000,
            MinSalary = 1500,
            MaxSalary = 3000,
            Remuneration = 2500,
            Band = EmployeeSalaryBand.Level1,
            Contribution = null
        };

        employeeSalaryDetailsServiceMock.Setup(x => x.SaveEmployeeSalary(It.IsAny<EmployeeSalaryDetailsDto>()))
                               .ThrowsAsync(new Exception("An error occurred while saving employee salary information."));

        var result = await controller.AddEmployeeSalary(employeeSalaryDetailsDto);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

        Assert.Equal("An error occurred while saving employee salary information.", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteEmployeeSalaryValidInputReturnsOkResult()
    {
        var employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalarayDetailsService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeSalaryDetailsController(employeeSalaryDetailsServiceMock.Object);

        var employeeSalaryDetailsDto = new EmployeeSalaryDetailsDto
        {
            Id = 1,
            EmployeeId = 1,
            Salary = 2000,
            MinSalary = 1500,
            MaxSalary = 3000,
            Remuneration = 2500,
            Band = EmployeeSalaryBand.Level1,
            Contribution = null
        };

        employeeServiceMock.Setup(x => x.GetEmployeeById(employeeSalaryDetailsDto.EmployeeId))
                           .ReturnsAsync(EmployeeTestData.EmployeeDto);

        employeeSalaryDetailsServiceMock.Setup(x => x.DeleteEmployeeSalary(employeeSalaryDetailsDto.Id))
                                        .Returns((Task<EmployeeSalaryDetailsDto>)Task.CompletedTask);

        var result = await controller.DeleteSalary(employeeSalaryDetailsDto.Id);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task DeleteEmployeeSalaryExceptionThrownReturnsNotFoundWithMessage()
    {
        var employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalarayDetailsService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeSalaryDetailsController(employeeSalaryDetailsServiceMock.Object);

        var employeeSalaryDetailsDto = new EmployeeSalaryDetailsDto
        {
            Id = 1,
            EmployeeId = 1,
            Salary = 2000,
            MinSalary = 1500,
            MaxSalary = 3000,
            Remuneration = 2500,
            Band = EmployeeSalaryBand.Level1,
            Contribution = null
        };

        employeeSalaryDetailsServiceMock.Setup(x => x.DeleteEmployeeSalary(employeeSalaryDetailsDto.Id))
                               .ThrowsAsync(new Exception("An error occurred while deleting employee salary information."));

        var result = await controller.DeleteSalary(employeeSalaryDetailsDto.Id);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while deleting employee date information.", notFoundResult.Value);
    }

    [Fact]
    public async Task UpdateEmployeeSalaryValidInputReturnsOkResult()
    {
        var employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalarayDetailsService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeSalaryDetailsController(employeeSalaryDetailsServiceMock.Object);

        var employeeSalaryDetailsDto = new EmployeeSalaryDetailsDto
        {
            Id = 1,
            EmployeeId = 1,
            Salary = 2000,
            MinSalary = 1500,
            MaxSalary = 3000,
            Remuneration = 2500,
            Band = EmployeeSalaryBand.Level1,
            Contribution = null
        };

        employeeServiceMock.Setup(x => x.GetEmployeeById(employeeSalaryDetailsDto.EmployeeId))
                           .ReturnsAsync(EmployeeTestData.EmployeeDto);

        employeeSalaryDetailsServiceMock.Setup(x => x.UpdateEmployeeSalary(employeeSalaryDetailsDto))
                                        .ReturnsAsync(employeeSalaryDetailsDto);

        var result = await controller.UpdateSalary(employeeSalaryDetailsDto);

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task UpdateEmployeeSalaryExceptionThrownReturnsNotFoundWithMessage()
    {
        var employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalarayDetailsService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeSalaryDetailsController(employeeSalaryDetailsServiceMock.Object);

        var employeeSalaryDetailsDto = new EmployeeSalaryDetailsDto
        {
            Id = 1,
            EmployeeId = 1,
            Salary = 2000,
            MinSalary = 1500,
            MaxSalary = 3000,
            Remuneration = 2500,
            Band = EmployeeSalaryBand.Level1,
            Contribution = null
        };

        employeeSalaryDetailsServiceMock.Setup(x => x.UpdateEmployeeSalary(It.IsAny<EmployeeSalaryDetailsDto>()))
                               .ThrowsAsync(new Exception("An error occurred while updating employee salary information."));

        var result = await controller.UpdateSalary(employeeSalaryDetailsDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while updating employee salary information.", notFoundResult.Value);
    }

    [Fact]
    public void GetAllEmployeeSalariesByEmployeeReturnsOkResultWithList()
    {
        var employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalarayDetailsService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeSalaryDetailsController(employeeSalaryDetailsServiceMock.Object);

        var employeeId = 1;
        var expectedSalaryDetailsDto = new EmployeeSalaryDetailsDto
        {
            Id = 1,
            EmployeeId = employeeId,
            Salary = 2000,
            MinSalary = 1500,
            MaxSalary = 3000,
            Remuneration = 2500,
            Band = EmployeeSalaryBand.Level1,
            Contribution = null
        };

        //employeeSalaryDetailsServiceMock.Setup(x => x.GetEmployeeSalary(employeeId)).Returns(expectedSalaryDetailsDto);

        //employeeSalaryDetailsServiceMock.Setup(x => x.SaveEmployeeSalary(It.IsAny<EmployeeSalaryDetailsDto>())).Returns(Task.FromResult(expectedSalaryDetailsDto));
        employeeSalaryDetailsServiceMock.Setup(x => x.GetEmployeeSalary(employeeId)).Returns(Task.FromResult(expectedSalaryDetailsDto));
        //_salaryService.Setup(s => s.GetAllEmployeeSalaries()).ReturnsAsync(_salaries);

        var result = controller.GetEmployeeSalary(employeeId: employeeId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualSalaryDetailsDto = Assert.IsType<List<EmployeeSalaryDetailsDto>>(okResult.Value);
        Assert.Equal(new List<EmployeeSalaryDetailsDto> { expectedSalaryDetailsDto }, actualSalaryDetailsDto);
    }

    [Fact]
    public void GetAllEmployeeDateNoFiltersReturnsOkResultWithList()
    {
        var employeeSalaryDetailsServiceMock = new Mock<IEmployeeSalarayDetailsService>();
        var employeeServiceMock = new Mock<IEmployeeService>();
        var controller = new EmployeeSalaryDetailsController(employeeSalaryDetailsServiceMock.Object);

        var employeeId = 1;
        var expectedSalaryDetailsDto = new EmployeeSalaryDetailsDto
        {
            Id = 1,
            EmployeeId = employeeId,
            Salary = 2000,
            MinSalary = 1500,
            MaxSalary = 3000,
            Remuneration = 2500,
            Band = EmployeeSalaryBand.Level1,
            Contribution = null
        };

        var aList = new List<EmployeeSalaryDetailsDto>
        { expectedSalaryDetailsDto };
        List<EmployeeSalaryDetailsDto> salaries = new List<EmployeeSalaryDetailsDto> { expectedSalaryDetailsDto };
        employeeSalaryDetailsServiceMock.Setup(x => x.GetAllEmployeeSalaries()).Returns(Task.FromResult(aList));

        var result = controller.GetAllEmployeeSalaries();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualSalaryDetailsDto = Assert.IsType<List<EmployeeSalaryDetailsDto>>(okResult.Value);
        Assert.Equal(new List<EmployeeSalaryDetailsDto> { expectedSalaryDetailsDto }, actualSalaryDetailsDto);
    }
}