﻿using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Handler.Charts;

public class LeaveIntervalTypeUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private EmployeeAddressDto employeeAddressDto;
    private readonly EmployeeType employeeType;
    private readonly EmployeeTypeDto employeeTypeDto;
    private readonly LeaveIntervalType leaveIntervalType;

    public LeaveIntervalTypeUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        leaveIntervalType = new LeaveIntervalType();
        employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        employeeType = new EmployeeType(employeeTypeDto);
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name))
                                .Returns(Task.FromResult(employeeTypeDto));
        employeeAddressDto =
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");
    }

    private EmployeeDto CreateEmployee(float? leaveInterval)
    {
        return new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
                               null, false, "None", 4, employeeTypeDto, "Notes", leaveInterval, 28, 128, 100000, "Matt",
                               "MT",
                               "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
                               new DateTime(), null, Race.Black, Gender.Male, null,
                               "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null,
                               employeeAddressDto, employeeAddressDto, null, null, null);
    }

    [Fact]
    public async Task GenerateDataNullTestSuccess()
    {
        var employeeDto = CreateEmployee(null);

        var employeeList = new List<Employee>
        {
            new(employeeDto, employeeDto.EmployeeType!)
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.AsQueryable().BuildMock());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = leaveIntervalType.GenerateData(employeeDto, realServiceProvider);

        Assert.Null(result);
    }

    [Fact]
    public async Task GenerateDataOneDayTestSuccess()
    {
        var employeeDto = CreateEmployee(1);

        var employeeList = new List<Employee>
        {
            new(employeeDto, employeeDto.EmployeeType!)
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.AsQueryable().BuildMock());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = leaveIntervalType.GenerateData(employeeDto, realServiceProvider);

        Assert.Contains("1 Day", result);
    }

    [Fact]
    public async Task GenerateDataMoreDaysTestSuccess()
    {
        var employeeDto = CreateEmployee(5);

        var employeeList = new List<Employee>
        {
            new(employeeDto, employeeDto.EmployeeType!)
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.AsQueryable().BuildMock());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = leaveIntervalType.GenerateData(employeeDto, realServiceProvider);

        Assert.Contains("Days",result);
    }

    [Fact]
    public async Task GenerateDataNullTestFail()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeDto.Name))
                                .Throws(new Exception("There was a problem fetching the employee type of the employee"));

        var employeeDto = CreateEmployee(null);

        var employeeList = new List<Employee>
        {
            new(employeeDto, employeeDto.EmployeeType!)
        };

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .Returns(Task.FromResult(false));
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(Task.FromResult(false));
        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.AsQueryable().BuildMock());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = leaveIntervalType.GenerateData(employeeDto, realServiceProvider);

        Assert.Null(result);
    }

    [Fact]
    public async Task GenerateDataOneDayTestFail()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeDto.Name))
                                .Throws(new Exception("There was a problem fetching the employee type of the employee"));

        var employeeDto = CreateEmployee(1);

        var employeeList = new List<Employee>
        {
            new(employeeDto, employeeDto.EmployeeType!)
        };

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .Returns(Task.FromResult(false));
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(Task.FromResult(false));
        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.AsQueryable().BuildMock());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = leaveIntervalType.GenerateData(employeeDto, realServiceProvider);

        Assert.Contains("1 Day", result);
    }

    [Fact]
    public async Task GenerateDataMoreDaysTestFail()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeDto.Name))
                                .Throws(new Exception("There was a problem fetching the employee type of the employee"));

        var employeeDto = CreateEmployee(5);

        var employeeList = new List<Employee>
        {
            new(employeeDto, employeeDto.EmployeeType)
        };

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .Returns(Task.FromResult(false));
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(Task.FromResult(false));
        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.AsQueryable().BuildMock());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result2 = leaveIntervalType.GenerateData(employeeDto, realServiceProvider);

        Assert.Contains("Days", result2);
    }
}