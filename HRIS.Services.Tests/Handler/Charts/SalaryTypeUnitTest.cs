using Moq;
using Xunit;
using System.Linq.Expressions;
using HRIS.Models.Enums;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork;
using RR.Tests.Data;

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
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name!)).ReturnsAsync(employeeTypeDto);
        employeeAddressDto = new EmployeeAddressDto{ Id = 1, UnitNumber = "2", ComplexName = "Complex", StreetNumber = "2", SuburbOrDistrict = "Suburb/District", City = "City", Country = "Country", Province = "Province", PostalCode = "1620" };
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
            EmployeeType = employeeTypeDto,
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
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto,
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
        var result = salaryType.GenerateData(employeeDto, realServiceProvider);
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
        var result = salaryType.GenerateData(employeeDto, realServiceProvider);
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
        var result = salaryType.GenerateData(null, realServiceProvider);
        Assert.Null(result);
    }
}
