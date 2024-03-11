using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RGO.Tests.Services;

public class EmployeeDateServiceUnitTests
{
    private readonly EmployeeDateService _employeeDateService;
    private readonly EmployeeDto _employeeDto;
    private readonly Mock<IUnitOfWork> _mockDb;
    private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;

    public EmployeeDateServiceUnitTests()
    {
        _mockDb = new Mock<IUnitOfWork>();
        _errorLoggingServiceMock = new Mock<IErrorLoggingService>();
        _employeeDateService = new EmployeeDateService(_mockDb.Object,_errorLoggingServiceMock.Object);
        var employeeTypeDto = new EmployeeTypeDto{ Id = 1, Name = "Employee" };
        var employeeAddressDto =
            new EmployeeAddressDto{ Id = 1, UnitNumber = "2", ComplexName = "Complex", StreetNumber = "2", SuburbOrDistrict = "Suburb/District", City = "City", Country = "Country", Province = "Province", PostalCode = "1620" };
        _employeeDto = new EmployeeDto
        {
            Id = 1,
            EmployeeNumber = "001",
            TaxNumber = "34434434",
            EngagementDate = DateTime.Now,
            TerminationDate = DateTime.Now,
            PeopleChampion = null,
            Disability = false,
            DisabilityNotes = "None",
            Level = 4,
            EmployeeType = employeeTypeDto,
            Notes = "Notes",
            LeaveInterval = 1,
            SalaryDays = 28,
            PayRate = 128,
            Salary = 100000,
            Name = "Dorothy",
            Initials = "D",
            Surname = "Mahoko",
            DateOfBirth = DateTime.Now,
            CountryOfBirth = "South Africa",
            Nationality = "South African",
            IdNumber = "0000080000000",
            PassportNumber = " ",
            PassportExpirationDate = DateTime.Now,
            PassportCountryIssue = null,
            Race = Race.Black,
            Gender = Gender.Male,
            Email = "texample@retrorabbit.co.za",
            PersonalEmail = "test.example@gmail.com",
            CellphoneNo = "0000000000",
            PhysicalAddress = employeeAddressDto,
            PostalAddress = employeeAddressDto
        };
    }

    [Fact]
    public async Task CheckIfExistsFailTest()
    {
        _mockDb.Setup(x => x.EmployeeDate.Any(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .ReturnsAsync(false);

        var exists = await _employeeDateService.CheckIfExists(new EmployeeDateDto
                    {
                    Id=1,
                    Employee = _employeeDto,
                    Subject = "Subject",
                    Note =  "Note",
                    Date = new DateOnly() 
                    }
        );

        Assert.False(exists);
    }

    [Fact]
    public async Task CheckIfExistsPassTest()
    {
        _mockDb.Setup(x => x.EmployeeDate.Any(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .ReturnsAsync(true);

        var exists = await _employeeDateService.CheckIfExists(new EmployeeDateDto
                        {
                        Id = 1,
                        Employee = _employeeDto,
                        Subject = "Subject",
                        Note = "Note",
                        Date = new DateOnly()
                        }
        );

        Assert.True(exists);
    }

    [Fact]
    public async Task SaveFailTest()
    {
        _mockDb.Setup(x => x.EmployeeDate.Any(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .ReturnsAsync(true);

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _employeeDateService.Save(new EmployeeDateDto
                                                  {
                                                    Id = 1,
                                                    Employee = _employeeDto,
                                                    Subject = "Subject",
                                                    Note = "Note",
                                                    Date = new DateOnly()
                                                  }));
    }

    [Fact]
    public async Task SavePassTest()
    {
        _mockDb.Setup(x => x.EmployeeDate.Any(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .ReturnsAsync(false);

        await _employeeDateService.Save(new EmployeeDateDto{ Id = 1, Employee = _employeeDto, Subject = "Subject", Note = "Note", Date = new DateOnly()});

        _mockDb.Verify(x => x.EmployeeDate.Add(It.IsAny<EmployeeDate>()), Times.Once);
    }

    [Fact]
    public async Task UpdateFailTest()
    {
        _mockDb.Setup(x => x.EmployeeDate.Any(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .ReturnsAsync(false);

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _employeeDateService.Update(new EmployeeDateDto
                                                  {   
                                                    Id = 1,
                                                    Employee = _employeeDto,
                                                    Subject = "Subject",
                                                    Note = "Note",
                                                    Date = new DateOnly()
                                                  }));
    }

    [Fact]
    public async Task UpdatePassTest()
    {
        _mockDb.Setup(x => x.EmployeeDate.Any(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .ReturnsAsync(true);

        await _employeeDateService.Update(new EmployeeDateDto{ Id = 1, Employee = _employeeDto, Subject = "Subject", Note = "Note", Date = new DateOnly()});

        _mockDb.Verify(x => x.EmployeeDate.Update(It.IsAny<EmployeeDate>()), Times.Once);
    }

    [Fact]
    public async Task DeletePassTest()
    {
        var employeeDate = new EmployeeDateDto { Id = 1, Employee = _employeeDto, Subject = "Subject", Note = "Note", Date = new DateOnly()};

        _mockDb.Setup(x => x.EmployeeDate.Any(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .ReturnsAsync(true);

        _mockDb.Setup(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .Returns(new List<EmployeeDate> { new(employeeDate) }.AsQueryable().BuildMock());

        await _employeeDateService.Delete(1);

        _mockDb.Verify(x => x.EmployeeDate.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetFailTest()
    {
        var employeeDate = new EmployeeDateDto{ Id = 1, Employee = _employeeDto, Subject = "Subject", Note = "Note", Date = new DateOnly()};

        _mockDb.Setup(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .Returns(new List<EmployeeDate>().AsQueryable().BuildMock());
        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _employeeDateService.Get(employeeDate));
    }

    [Fact]
    public async Task GetPassTest()
    {
        var employeeDate = new EmployeeDateDto{ Id = 1, Employee = _employeeDto, Subject = "Subject", Note = "Note", Date = new DateOnly()};

        _mockDb.Setup(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .Returns(new List<EmployeeDate> { new(employeeDate) }.AsQueryable().BuildMock());

        await _employeeDateService.Get(employeeDate);

        _mockDb.Verify(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()), Times.Once);
    }

    [Fact]
    public void GetAllTest()
    {
        var employeeDateList = new List<EmployeeDate>
            {new(new EmployeeDateDto{ Id = 1, Employee = _employeeDto, Subject = "Subject", Note = "Note", Date = new DateOnly()})};

        var employeeList = new List<Employee> { new(_employeeDto, _employeeDto.EmployeeType!) };

        _mockDb.Setup(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .Returns(employeeDateList.AsQueryable().BuildMock());

        _mockDb.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.AsQueryable().BuildMock());

        var result = _employeeDateService.GetAll();

        Assert.Equal(employeeDateList.Count, result.Count);
    }

    [Fact]
    public void GetAllByEmployeeTest()
    {
        var employeeDateList = new List<EmployeeDate>
            {new(new EmployeeDateDto{ Id = 1, Employee = _employeeDto, Subject = "Subject", Note = "Note", Date = new DateOnly()})};

        var employeeList = new List<Employee> { new(_employeeDto, _employeeDto.EmployeeType!) };

        _mockDb.Setup(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .Returns(employeeDateList.AsQueryable().BuildMock());

        _mockDb.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.AsQueryable().BuildMock());

        var result = _employeeDateService.GetAllByEmployee(_employeeDto.Email!);

        Assert.Equal(employeeDateList.Count, result.Count);
    }

    [Fact]
    public void GetAllByDateTest()
    {
        var employeeDateList = new List<EmployeeDate>
            {new(new EmployeeDateDto{ Id = 1, Employee = _employeeDto, Subject = "Subject", Note = "Note", Date = new DateOnly()})};

        var employeeList = new List<Employee> { new(_employeeDto, _employeeDto.EmployeeType!) };

        _mockDb.Setup(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .Returns(employeeDateList.AsQueryable().BuildMock());

        _mockDb.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.AsQueryable().BuildMock());

        var result = _employeeDateService.GetAllByDate(new DateOnly());

        Assert.Equal(employeeDateList.Count, result.Count);
    }

    [Fact]
    public void GetAllBySubjectTest()
    {
        var employeeDateList = new List<EmployeeDate>
            {new(new EmployeeDateDto{ Id = 1, Employee = _employeeDto, Subject = "Subject", Note = "Note", Date = new DateOnly() })};

        var employeeList = new List<Employee> { new(_employeeDto, _employeeDto.EmployeeType!) };

        _mockDb.Setup(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .Returns(employeeDateList.AsQueryable().BuildMock());

        _mockDb.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.AsQueryable().BuildMock());

        var result = _employeeDateService.GetAllBySubject("Subject");

        Assert.Equal(employeeDateList.Count, result.Count);
    }
}
