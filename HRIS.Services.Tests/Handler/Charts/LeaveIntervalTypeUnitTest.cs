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

public class LeaveIntervalTypeUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private readonly EmployeeAddressDto employeeAddressDto;
    private readonly EmployeeType employeeType;
    private readonly EmployeeTypeDto employeeTypeDto;
    private readonly LeaveIntervalType leaveIntervalType;

    public LeaveIntervalTypeUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        leaveIntervalType = new LeaveIntervalType();
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

    private EmployeeDto CreateEmployee(float? leaveInterval)
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
            Level = 4,
            EmployeeType = employeeTypeDto,
            Notes = "Notes",
            LeaveInterval = leaveInterval,
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
    public void GenerateDataNullTestSuccess()
    {
        var employeeDto = CreateEmployee(null);

        var employeeList = new List<Employee>
        {
            new(employeeDto, employeeDto.EmployeeType!)
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.ToMockIQueryable());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = leaveIntervalType.GenerateData(employeeDto, realServiceProvider);

        Assert.Null(result);
    }

    [Fact]
    public void GenerateDataOneDayTestSuccess()
    {
        var employeeDto = CreateEmployee(1);

        var employeeList = new List<Employee>
        {
            new(employeeDto, employeeDto.EmployeeType!)
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.ToMockIQueryable());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = leaveIntervalType.GenerateData(employeeDto, realServiceProvider);

        Assert.Contains("1 Day", result);
    }

    [Fact]
    public void GenerateDataMoreDaysTestSuccess()
    {
        var employeeDto = CreateEmployee(5);

        var employeeList = new List<Employee>
        {
            new(employeeDto, employeeDto.EmployeeType!)
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.ToMockIQueryable());

        var realServiceProvider = Mock.Of<IServiceProvider>();
        var result = leaveIntervalType.GenerateData(employeeDto, realServiceProvider);

        Assert.Contains("Days",result);
    }

    [Fact]
    public void GenerateDataNullTestFail()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeTypeByName(employeeTypeDto.Name))
                                .Throws(new Exception("There was a problem fetching the employee type of the employee"));

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
        var result = leaveIntervalType.GenerateData(employeeDto, realServiceProvider);

        Assert.Null(result);
    }

    [Fact]
    public void GenerateDataOneDayTestFail()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeTypeByName(employeeTypeDto.Name))
                                .Throws(new Exception("There was a problem fetching the employee type of the employee"));

        var employeeDto = CreateEmployee(1);

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
        var result = leaveIntervalType.GenerateData(employeeDto, realServiceProvider);

        Assert.Contains("1 Day", result);
    }

    [Fact]
    public void GenerateDataMoreDaysTestFail()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeTypeByName(employeeTypeDto.Name))
                                .Throws(new Exception("There was a problem fetching the employee type of the employee"));

        var employeeDto = CreateEmployee(5);

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
        var result2 = leaveIntervalType.GenerateData(employeeDto, realServiceProvider);

        Assert.Contains("Days", result2);
    }
}
