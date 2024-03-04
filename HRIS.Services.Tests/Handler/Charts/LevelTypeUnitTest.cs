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

public class LevelTypeUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private EmployeeAddressDto? employeeAddressDto;
    private readonly EmployeeType employeeType;
    private readonly EmployeeTypeDto employeeTypeDto;
    private readonly LevelType levelType;

    public LevelTypeUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        levelType = new LevelType();
        employeeTypeDto = new EmployeeTypeDto{ Id = 1, Name = "Developer" };
        employeeType = new EmployeeType(employeeTypeDto);
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name!))
                                .Returns(Task.FromResult(employeeTypeDto));

        employeeAddressDto = new EmployeeAddressDto
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
    }

    private EmployeeDto CreateEmployee(int? levelType)
    {
        return new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
                               null, false, "None", levelType, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt",
                               "MT",
                               "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
                               new DateTime(), null, Race.Black, Gender.Male, null,
                               "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null,
                               employeeAddressDto, employeeAddressDto, null, null, null);
    }

    [Fact]
    public void LevelTypeNullTestSuccess()
    {
        var employeeDto = CreateEmployee(null);

        var employeeList = new List<Employee>
        {
            new(employeeDto, employeeDto.EmployeeType!)
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.AsQueryable().BuildMock());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = levelType.GenerateData(employeeDto, realServiceProvider);

        Assert.Null(result);
    }

    [Fact]
    public void LevelTypeNullFail()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeDto.Name))
                                .Throws(new Exception("Failed to get employee type of employee"));

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
        var result = levelType.GenerateData(employeeDto, realServiceProvider);

        Assert.Null(result);
    }

    [Fact]
    public void LevelTypeValueTestSuccess()
    {
        var employeeDto = CreateEmployee(3);

        var employeeList = new List<Employee>
        {
            new(employeeDto, employeeDto.EmployeeType!)
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.AsQueryable().BuildMock());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = levelType.GenerateData(employeeDto, realServiceProvider);

        Assert.Equal("Level 3, ", result);
    }

    [Fact]
    public void LevelTypeValueTestFail()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeDto.Name))
                                .Throws(new Exception("Failed to get employee type of employee"));

        var employeeDto = CreateEmployee(3);

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
        var result = levelType.GenerateData(employeeDto, realServiceProvider);

        Assert.Equal("Level 3, ", result);
    }
}
