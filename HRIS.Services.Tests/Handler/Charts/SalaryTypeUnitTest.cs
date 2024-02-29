using Moq;
using Xunit;
using System.Linq.Expressions;
using MockQueryable.Moq;
using HRIS.Models.Enums;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork;

namespace RGO.Services.Tests.Handler.Charts;

public class SalaryTypeUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private SalaryType salaryType;
    EmployeeTypeDto employeeTypeDto;
    EmployeeType employeeType;
    RoleDto roleDto;
    EmployeeAddressDto employeeAddressDto;

    public SalaryTypeUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        salaryType = new SalaryType();
        employeeTypeDto = new EmployeeTypeDto{ Id = 1, Name = "Developer" };
        employeeType = new EmployeeType(employeeTypeDto);
        roleDto = new RoleDto{ Id = 3, Description = "Employee"};
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name)).Returns(Task.FromResult(employeeTypeDto));
        employeeAddressDto = new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");
    }

    private EmployeeDto CreateEmployee(int? salary)
    {
        return new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
           null, false, "None", 1, employeeTypeDto, "Notes", 1, 28, 128, salary, "Matt", "MT",
           "Schoeman", new DateTime(), "South Africa", "South African", "0000080000000", " ",
           new DateTime(), null, Race.Black, Gender.Male, null,
           "test@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null, employeeAddressDto, employeeAddressDto, null, null, null);
    }

    [Fact]
    public async Task SalaryTypeNullTestSuccess()
    {
        var employeeDto = CreateEmployee(null);

        var employeeList = new List<Employee>
        {
            new Employee(employeeDto,employeeDto.EmployeeType)
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(employeeList.AsQueryable().BuildMock());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = salaryType.GenerateData(employeeDto, realServiceProvider);
        Assert.Null(result);
    }

    [Fact]
    public async Task SalaryTypePass()
    {
        var employeeDto = CreateEmployee(3);

        var employeeList = new List<Employee>
        {
            new Employee(employeeDto,employeeDto.EmployeeType)
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(employeeList.AsQueryable().BuildMock());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = salaryType.GenerateData(employeeDto, realServiceProvider);
        Assert.NotNull(result);
        Assert.Equal("Salary 3, ", result);
    }

    [Fact]
    public async Task SalaryTypeNullEmployee()
    {
        var employeeList = new List<Employee>
        {
            null
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(employeeList.AsQueryable().BuildMock());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = salaryType.GenerateData(null, realServiceProvider);
        Assert.Null(result);
    }
}
