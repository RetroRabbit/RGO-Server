using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.Extensions.DependencyInjection;
using MockQueryable.Moq;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Handler.Charts;

public class PeopleChampionTypeUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    //private EmployeeAddressDto employeeAddressDto;
    private readonly EmployeeType employeeType1;
    private readonly EmployeeType employeeType2;
    private readonly EmployeeTypeDto employeeTypeDto1;
    private readonly EmployeeTypeDto employeeTypeDto2;
    private readonly PeopleChampionType peopleChampionType;

    public PeopleChampionTypeUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        peopleChampionType = new PeopleChampionType();
        employeeTypeDto1 = new EmployeeTypeDto(3, "Developer");
        employeeTypeDto2 = new EmployeeTypeDto(7, "People Champion");
        employeeType1 = new EmployeeType(employeeTypeDto1);
        employeeType2 = new EmployeeType(employeeTypeDto2);
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType1.Name))
                                .Returns(Task.FromResult(employeeTypeDto1));
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType2.Name))
                                .Returns(Task.FromResult(employeeTypeDto2));
        var employeeAddressDto =
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");
    }

    private EmployeeDto CreateEmployee(int id, int? peopleChampionType, string? employeeName, string? employeeSurname,
                                       EmployeeTypeDto employeeType)
    {
        return new EmployeeDto(id, "001", "34434434", new DateTime(), new DateTime(),
                               peopleChampionType, false, "None", 3, employeeType, "Notes", 1, 28, 128, 100000,
                               employeeName, "MT",
                               employeeSurname, new DateTime(), "South Africa", "South African", "0000080000000", " ",
                               new DateTime(), null, Race.Black, Gender.Male, null,
                               $"test{id}@retrorabbit.co.za", "test.example@gmail.com", "0000000000", null, null,
                               employeeAddressDto, employeeAddressDto, null, null, null);
    }

    [Fact]
    public async Task PeopleChampionTypeNullTestSuccess()
    {
        var employeeDto1 = CreateEmployee(1, null, "Matt", "Smith", employeeTypeDto1);
        var employeeDto2 = CreateEmployee(4, null, "Dotty", "Missile", employeeTypeDto2);

        var employeeList = new List<Employee>
        {
            new(employeeDto1, employeeDto1.EmployeeType),
            new(employeeDto2, employeeDto2.EmployeeType)
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.AsQueryable().BuildMock());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = peopleChampionType.GenerateData(employeeDto1, realServiceProvider);

        Assert.Null(result);
    }

    [Fact]
    public async Task PeopleChampionTypeNullFail()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeDto1.Name))
                                .Throws(new Exception("Failed to get employee type of employee"));

        var employeeDto1 = CreateEmployee(1, null, "Matt", "Smith", employeeTypeDto1);
        var employeeDto2 = CreateEmployee(4, null, "Dotty", "Missile", employeeTypeDto2);

        var employeeList = new List<Employee>
        {
            new(employeeDto1, employeeDto1.EmployeeType),
            new(employeeDto2, employeeDto2.EmployeeType)
        };

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .Returns(Task.FromResult(false));
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(Task.FromResult(false));
        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.AsQueryable().BuildMock());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = peopleChampionType.GenerateData(employeeDto1, realServiceProvider);

        Assert.Null(result);
    }

    [Fact]
    public async Task PeopleChampionTypeValueTestSuccess()
    {
        var regularEmployee = CreateEmployee(1, 2, "Matt", "Smith", employeeTypeDto1);
        var peopleChampion = CreateEmployee(2, 3, "Dotty", "Missile", employeeTypeDto2);

        var employeeList = new List<Employee>
        {
            new(regularEmployee, regularEmployee.EmployeeType),
            new(peopleChampion, peopleChampion.EmployeeType)
        };

        var employeeServiceMock = new Mock<IEmployeeService>();
        employeeServiceMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(peopleChampion);

        var employeeServiceProvider = employeeServiceMock.Object;

        _dbMock.Setup(e => e.Employee.GetById(peopleChampion.Id)).Returns(Task.FromResult(peopleChampion));
        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.AsQueryable().BuildMock());
        var emp = await employeeServiceProvider.GetById(peopleChampion.Id);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(sp => employeeServiceMock.Object);
        var realServiceProvider = serviceCollection.BuildServiceProvider();

        var result = peopleChampionType.GenerateData(emp, realServiceProvider);

        Assert.Equal("Dotty Missile, ", result);
    }

    [Fact]
    public async Task PeopleChampionTypeValueTestFail()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeDto1.Name))
                                .Throws(new Exception("Failed to get employee type of employee"));

        var regularEmployee = CreateEmployee(1, 2, "Matt", "Smith", employeeTypeDto1);
        var peopleChampion = CreateEmployee(2, 3, "Dotty", "Missile", employeeTypeDto2);

        var employeeList = new List<Employee>
        {
            new(regularEmployee, regularEmployee.EmployeeType),
            new(peopleChampion, peopleChampion.EmployeeType)
        };

        var employeeServiceMock = new Mock<IEmployeeService>();
        employeeServiceMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(peopleChampion);

        var employeeServiceProvider = employeeServiceMock.Object;

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .Returns(Task.FromResult(false));
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(Task.FromResult(false));
        _dbMock.Setup(e => e.Employee.GetById(peopleChampion.Id)).Returns(Task.FromResult(peopleChampion));
        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.AsQueryable().BuildMock());
        var emp = await employeeServiceProvider.GetById(peopleChampion.Id);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(sp => employeeServiceMock.Object);
        var realServiceProvider = serviceCollection.BuildServiceProvider();

        var result = peopleChampionType.GenerateData(emp, realServiceProvider);

        Assert.Equal("Dotty Missile, ", result);
    }
}