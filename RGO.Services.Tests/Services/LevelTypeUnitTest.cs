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

public class LevelTypeUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private LevelType levelType;
    EmployeeTypeDto employeeTypeDto;
    EmployeeType employeeType;
    RoleDto roleDto;
    EmployeeAddressDto employeeAddressDto;

    public LevelTypeUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        levelType = new LevelType();
        employeeTypeDto = new EmployeeTypeDto(1, "Developer");
        employeeType = new EmployeeType(employeeTypeDto);
        roleDto = new RoleDto(3, "Employee");
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name)).Returns(Task.FromResult(employeeTypeDto));
        EmployeeAddressDto employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");
    }

    private EmployeeDto CreateEmployee(int? levelType)
    {
        return new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
           null, false, "None", levelType, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Matt", "MT",
           "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
           new DateTime(), null, Race.Black, Gender.Male, null,
           "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);
    }

    [Fact]
    public async Task LevelTypeNullTestSuccess()
    {
        var employeeDto = CreateEmployee(null);

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
        var result = levelType.GenerateData(employeeDto, realServiceProvider);

        Assert.Null(result);
    }

    [Fact]
    public async Task LevelTypeNullFail()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeDto.Name)).Throws(new Exception("Failed to get employee type of employee"));

        var employeeDto = CreateEmployee(null);

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
        var result = levelType.GenerateData(employeeDto, realServiceProvider);

        Assert.Null(result);
    }

    [Fact]
    public async Task LevelTypeValueTestSuccess()
    {
        var employeeDto = CreateEmployee(3);

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
        var result = levelType.GenerateData(employeeDto, realServiceProvider);

        Assert.Equal("Level 3, ",result);
    }

    [Fact]
    public async Task LevelTypeValueTestFail()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeDto.Name)).Throws(new Exception("Failed to get employee type of employee"));

        var employeeDto = CreateEmployee(3);

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
        var result = levelType.GenerateData(employeeDto, realServiceProvider);

        Assert.Equal("Level 3, ",result);
    }
}
