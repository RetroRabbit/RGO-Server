using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Handler.Charts;

public class SalaryTypeUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private readonly EmployeeType employeeType;
    private readonly SalaryType salaryType;

    public SalaryTypeUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        salaryType = new SalaryType();
        employeeType = new EmployeeType(EmployeeTypeTestData.DeveloperType);
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name))
                                .Returns(Task.FromResult(EmployeeTypeTestData.DeveloperType));
    }

    private EmployeeDto CreateEmployee(int? salary)
    {
        return EmployeeTestData.EmployeeDto;
    }

    [Fact]
    public async Task SalaryTypeNullTestSuccess()
    {
        var employeeDto = CreateEmployee(null);

        var employeeList = new List<Employee>
        {
            new(employeeDto, employeeDto.EmployeeType)
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
            new(employeeDto, employeeDto.EmployeeType)
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