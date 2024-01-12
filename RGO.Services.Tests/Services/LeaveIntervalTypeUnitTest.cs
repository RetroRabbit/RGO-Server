using Moq;
using Xunit;
using RGO.Services.Services;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.Models;
using RGO.UnitOfWork.Entities;
using System.Linq.Expressions;
using MockQueryable.Moq;
using RGO.Models.Enums;

namespace RGO.Services.Tests.Services;

public class LeaveIntervalTypeUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private LeaveIntervalType leaveIntervalType;
    EmployeeTypeDto employeeTypeDto;
    EmployeeType employeeType;
    RoleDto roleDto;
    EmployeeAddressDto employeeAddressDto;

    public LeaveIntervalTypeUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        leaveIntervalType = new LeaveIntervalType();
        employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        employeeType = new EmployeeType(employeeTypeDto);
        roleDto = new RoleDto(3, "Employee");
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name)).Returns(Task.FromResult(employeeTypeDto));
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");
    }

    [Fact]
    public async Task GenerateDataNullTestSuccess()
    {
        EmployeeDto employeeDto = new(1, "001", "34434434", new DateTime(), new DateTime(),
            null, false, "None", 4, employeeTypeDto, "Notes", null, 28, 128, 100000, "Matt", "MT",
            "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
            new DateTime(), null, Race.Black, Gender.Male, null,
            "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        var employeeDtoList = new List<EmployeeDto>
        {
            employeeDto
        };

        var employeeList = new List<Employee>
        {
            new Employee(employeeDto,employeeDto.EmployeeType)
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
        EmployeeDto employeeDto = new(1, "001", "34434434", new DateTime(), new DateTime(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
            "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
            new DateTime(), null, Race.Black, Gender.Male, null,
            "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        var employeeDtoList = new List<EmployeeDto>
        {
            employeeDto
        };

        var employeeList = new List<Employee>
        {
            new Employee(employeeDto,employeeDto.EmployeeType)
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
        EmployeeDto employeeDto = new(2, "001", "34434434", new DateTime(), new DateTime(),
            null, false, "None", 4, employeeTypeDto, "Notes", 5, 28, 128, 100000, "Llewellyn", "LA",
            "Anthony", new DateTime(), "South Africa", "South African", "0000080000000", " ",
            new DateTime(), null, Race.White, Gender.Male, null,
            "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        var employeeDtoList = new List<EmployeeDto>
        {
            employeeDto
        };

        var employeeList = new List<Employee>
        {
            new Employee(employeeDto,employeeDto.EmployeeType)
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(employeeList.AsQueryable().BuildMock());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = leaveIntervalType.GenerateData(employeeDto, realServiceProvider);

        Assert.True(result.Contains("Days"));
    }

    [Fact]
    public async Task GenerateDataNullTestFail()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeDto.Name)).Throws(new Exception());

        EmployeeDto employeeDto = new(1, "001", "34434434", new DateTime(), new DateTime(),
            null, false, "None", 4, employeeTypeDto, "Notes", null, 28, 128, 100000, "Matt", "MT",
            "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
            new DateTime(), null, Race.Black, Gender.Male, null,
            "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        var employeeDtoList = new List<EmployeeDto>
        {
            employeeDto
        };

        var employeeList = new List<Employee>
        {
            new Employee(employeeDto,employeeDto.EmployeeType)
        };

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>())).Returns(Task.FromResult(false));
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(Task.FromResult(false));
        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(employeeList.AsQueryable().BuildMock());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = leaveIntervalType.GenerateData(employeeDto, realServiceProvider);
      
        Assert.Null(result);
    }

    [Fact]
    public async Task GenerateDataOneDayTestFail()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeDto.Name)).Throws(new Exception());

        EmployeeDto employeeDto = new(1, "001", "34434434", new DateTime(), new DateTime(),
            null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
            "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
            new DateTime(), null, Race.Black, Gender.Male, null,
            "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        var employeeDtoList = new List<EmployeeDto>
        {
            employeeDto
        };

        var employeeList = new List<Employee>
        {
            new Employee(employeeDto,employeeDto.EmployeeType)
        };

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>())).Returns(Task.FromResult(false));
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(Task.FromResult(false));
        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(employeeList.AsQueryable().BuildMock());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = leaveIntervalType.GenerateData(employeeDto, realServiceProvider);

        Assert.Contains("1 Day", result);
    }

    [Fact]
    public async Task GenerateDataMoreDaysTestFail()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeDto.Name)).Throws(new Exception());

        EmployeeDto employeeDto = new(2, "001", "34434434", new DateTime(), new DateTime(),
            null, false, "None", 4, employeeTypeDto, "Notes", 5, 28, 128, 100000, "Llewellyn", "LA",
            "Anthony", new DateTime(), "South Africa", "South African", "0000080000000", " ",
            new DateTime(), null, Race.White, Gender.Male, null,
            "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);

        var employeeDtoList = new List<EmployeeDto>
        {
            employeeDto
        };

        var employeeList = new List<Employee>
        {
            new Employee(employeeDto,employeeDto.EmployeeType)
        };

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>())).Returns(Task.FromResult(false));
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(Task.FromResult(false));
        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(employeeList.AsQueryable().BuildMock());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result2 = leaveIntervalType.GenerateData(employeeDto, realServiceProvider);

        Assert.True(result2.Contains("Days"));
    }
}
