using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.Tests.Data;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Handler.Charts;

public class SalaryTypeUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly SalaryType _salaryType;
    readonly EmployeeTypeDto _employeeTypeDto;

    public SalaryTypeUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        Mock<IEmployeeTypeService> employeeTypeServiceMock = new();
        _salaryType = new SalaryType();
        _employeeTypeDto = new EmployeeTypeDto{ Id = 1, Name = "Developer" };
        var employeeType = new EmployeeType(_employeeTypeDto);
        employeeTypeServiceMock.Setup(r => r.GetEmployeeTypeByName(employeeType.Name!)).ReturnsAsync(_employeeTypeDto);
    }

    private EmployeeDto CreateEmployee(int? salary)
    {
        return new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "001",
            TaxNumber = "34434434",
            EngagementDate = new DateTime(),
            TerminationDate = new DateTime(),
            PeopleChampion = null,
            Disability = false,
            DisabilityNotes = "None",
            Level = 1,
            EmployeeType = _employeeTypeDto,
            Notes = "Notes",
            LeaveInterval = 1,
            SalaryDays = 28,
            PayRate = 128,
            Salary = salary,
            Name = "Matt",
            Initials = "MT",
            Surname = "Schoeman",
            DateOfBirth = new DateTime(),
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "0000080000000",
            PassportNumber = " ",
            PassportExpirationDate = new DateTime(),
            PassportCountryIssue = null,
            Race = Race.Black,
            Gender = Gender.Male,
            Photo = null,
            Email = "test@retrorabbit.co.za",
            PersonalEmail = "test.example@gmail.com",
            CellphoneNo = "0000000000",
            ClientAllocated = null,
            TeamLead = null,
            HouseNo = null,
            EmergencyContactName = null,
            EmergencyContactNo = null
        };
    }

    [Fact]
    public void SalaryTypeNullTestSuccess()
    {
        var employeeDto = CreateEmployee(null);

        var employeeList = new List<Employee>
        {
            new(employeeDto,employeeDto.EmployeeType!)
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(employeeList.ToMockIQueryable());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = _salaryType.GenerateData(employeeDto, realServiceProvider);
        Assert.Null(result);
    }

    [Fact]
    public void SalaryTypePass()
    {
        var employeeDto = CreateEmployee(3);

        var employeeList = new List<Employee>
        {
            new(employeeDto,employeeDto.EmployeeType!)
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(employeeList.ToMockIQueryable());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = _salaryType.GenerateData(employeeDto, realServiceProvider);
        Assert.NotNull(result);
        Assert.Equal("Salary 3, ", result);
    }

    [Fact]
    public void SalaryTypeNullEmployee()
    {
        var employeeList = new List<Employee>();

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
            .Returns(employeeList.ToMockIQueryable());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = _salaryType.GenerateData(null, realServiceProvider);
        Assert.Null(result);
    }
}
