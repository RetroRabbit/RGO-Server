using System.Linq.Expressions;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RR.Tests.Data;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Handler.Charts;

public class PeopleChampionTypeUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private EmployeeAddress? employeeAddressDto;
    private readonly EmployeeType employeeType1;
    private readonly EmployeeType employeeType2;
    private readonly EmployeeType employeeTypeDto1;
    private readonly EmployeeType employeeTypeDto2;
    private readonly PeopleChampionType peopleChampionType;

    public PeopleChampionTypeUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        peopleChampionType = new PeopleChampionType();
        employeeTypeDto1 = new EmployeeType{ Id = 3, Name = "Developer" };
        employeeTypeDto2 = new EmployeeType{ Id = 7, Name = "People Champion" };
        employeeType1 = employeeTypeDto1;
        employeeType2 = employeeTypeDto2;
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeTypeByName(employeeType1.Name!))
                                .ReturnsAsync(employeeTypeDto1.ToDto());
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeTypeByName(employeeType2.Name!))
                                .ReturnsAsync(employeeTypeDto2.ToDto());
        employeeAddressDto =
            new EmployeeAddress{ Id = 1, UnitNumber = "2", ComplexName = "Complex", StreetNumber = "2", SuburbOrDistrict = "Suburb/District", City = "City", Country = "Country", Province = "Province", PostalCode = "1620" };
    }

    private Employee CreateEmployee(int id, int? peopleChampionType, string? employeeName, string? employeeSurname,
                                       EmployeeType employeeType)
    {
        return new Employee
        {
            Id = id,
            EmployeeNumber = "001",
            TaxNumber = "34434434",
            EngagementDate = DateTime.Now,
            TerminationDate = DateTime.Now,
            PeopleChampion = peopleChampionType,
            Disability = false,
            DisabilityNotes = "None",
            Level = 3,
            EmployeeType = employeeType,
            Notes = "Notes",
            LeaveInterval = 1,
            SalaryDays = 28,
            PayRate = 128,
            Salary = 100000,
            Name = employeeName,
            Initials = "MT",
            Surname = employeeSurname,
            DateOfBirth = DateTime.Now,
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "0000080000000",
            PassportNumber = " ",
            PassportExpirationDate = DateTime.Now,
            PassportCountryIssue = null,
            Race = Race.Black,
            Gender = Gender.Male,
            Email = $"test{id}@retrorabbit.co.za",
            PersonalEmail = "test.example@gmail.com",
            CellphoneNo = "0000000000",
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto
        };
    }

    [Fact]
    public void PeopleChampionTypeNullTestSuccess()
    {
        var employeeDto1 = CreateEmployee(1, null, "Matt", "Smith", employeeTypeDto1);
        var employeeDto2 = CreateEmployee(4, null, "Dotty", "Missile", employeeTypeDto2);

        var employeeList = new List<Employee>
        {
            employeeDto1,
            employeeDto2
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.ToMockIQueryable());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = peopleChampionType.GenerateData(employeeDto1.ToDto(), realServiceProvider);

        Assert.Null(result);
    }

    [Fact]
    public void PeopleChampionTypeNullFail()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeTypeByName(employeeTypeDto1.Name))
                                .Throws(new Exception("Failed to get employee type of employee"));

        var employeeDto1 = CreateEmployee(1, null, "Matt", "Smith", employeeTypeDto1);
        var employeeDto2 = CreateEmployee(4, null, "Dotty", "Missile", employeeTypeDto2);

        var employeeList = new List<Employee>
        {
            employeeDto1,
            employeeDto2
        };

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(false);
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .ReturnsAsync(false);
        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.ToMockIQueryable());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = peopleChampionType.GenerateData(employeeDto1.ToDto(), realServiceProvider);

        Assert.Null(result);
    }

    [Fact]
    public async Task PeopleChampionTypeValueTestSuccess()
    {
        var regularEmployee = CreateEmployee(1, 2, "Matt", "Smith", employeeTypeDto1);
        var peopleChampion = CreateEmployee(2, 3, "Dotty", "Missile", employeeTypeDto2);

        var employeeList = new List<Employee>
        {
            regularEmployee,
            peopleChampion
        };

        var employeeServiceMock = new Mock<IEmployeeService>();
        employeeServiceMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(peopleChampion.ToDto());

        var employeeServiceProvider = employeeServiceMock.Object;

        _dbMock.Setup(e => e.Employee.GetById(peopleChampion.Id)).ReturnsAsync(peopleChampion!);
        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.ToMockIQueryable());
        var emp = await employeeServiceProvider.GetById(peopleChampion.Id);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(sp => employeeServiceMock.Object);
        var realServiceProvider = serviceCollection.BuildServiceProvider();

        var result = peopleChampionType.GenerateData(emp!, realServiceProvider);

        Assert.Equal("Dotty Missile, ", result);
    }

    [Fact]
    public async Task PeopleChampionTypeValueTestFail()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeTypeByName(employeeTypeDto1.Name))
                                .Throws(new Exception("Failed to get employee type of employee"));

        var regularEmployee = CreateEmployee(1, 2, "Matt", "Smith", employeeTypeDto1);
        var peopleChampion = CreateEmployee(2, 3, "Dotty", "Missile", employeeTypeDto2);

        var employeeList = new List<Employee>
        {
            regularEmployee,
            peopleChampion
        };

        var employeeServiceMock = new Mock<IEmployeeService>();
        employeeServiceMock.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(peopleChampion.ToDto());

        var employeeServiceProvider = employeeServiceMock.Object;

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(false);
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .ReturnsAsync(false);
        _dbMock.Setup(e => e.Employee.GetById(peopleChampion.Id)).ReturnsAsync(peopleChampion!);
        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.ToMockIQueryable());
        var emp = await employeeServiceProvider.GetById(peopleChampion.Id);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped(sp => employeeServiceMock.Object);
        var realServiceProvider = serviceCollection.BuildServiceProvider();

        var result = peopleChampionType.GenerateData(emp!, realServiceProvider);

        Assert.Equal("Dotty Missile, ", result);
    }
}
