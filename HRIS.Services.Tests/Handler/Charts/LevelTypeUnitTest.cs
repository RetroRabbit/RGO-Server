using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Employee.Commons;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.Tests.Data;
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
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeTypeByName(employeeType.Name!))
                                .ReturnsAsync(employeeTypeDto);

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
            Level = levelType,
            EmployeeType = employeeTypeDto,
            Notes = "Notes",
            LeaveInterval = 1,
            SalaryDays = 28,
            PayRate = 128,
            Salary = 100000,
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
    public void LevelTypeNullTestSuccess()
    {
        var employeeDto = CreateEmployee(null);

        var employeeList = new List<Employee>
        {
            new(employeeDto, employeeDto.EmployeeType!)
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.ToMockIQueryable());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = levelType.GenerateData(employeeDto, realServiceProvider);

        Assert.Null(result);
    }

    [Fact]
    public void LevelTypeNullFail()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeTypeByName(employeeTypeDto.Name))
                                .Throws(new Exception("Failed to get employee type of employee"));

        var employeeDto = CreateEmployee(null);

        var employeeList = new List<Employee>
        {
            new(employeeDto, employeeDto.EmployeeType!)
        };

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(false);
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .ReturnsAsync(false);
        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.ToMockIQueryable());

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
               .Returns(employeeList.ToMockIQueryable());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = levelType.GenerateData(employeeDto, realServiceProvider);

        Assert.Equal("Level 3, ", result);
    }

    [Fact]
    public void LevelTypeValueTestFail()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeTypeByName(employeeTypeDto.Name))
                                .Throws(new Exception("Failed to get employee type of employee"));

        var employeeDto = CreateEmployee(3);

        var employeeList = new List<Employee>
        {
            new(employeeDto, employeeDto.EmployeeType!)
        };

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(false);
        _dbMock.Setup(r => r.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
               .ReturnsAsync(false);
        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.ToMockIQueryable());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = levelType.GenerateData(employeeDto, realServiceProvider);

        Assert.Equal("Level 3, ", result);
    }
}
