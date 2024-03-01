using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
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

    public EmployeeDateServiceUnitTests()
    {
        _mockDb = new Mock<IUnitOfWork>();
        _employeeDateService = new EmployeeDateService(_mockDb.Object);
        var employeeTypeDto = new EmployeeTypeDto{ Id = 1, Name = "Employee" };
        var employeeAddressDto =
            new EmployeeAddressDto(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");
        _employeeDto = new EmployeeDto(1, "001", "34434434", new DateTime(), new DateTime(),
                                       null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Dotty",
                                       "D",
                                       "Missile", new DateTime(), "South Africa", "South African", "1234457899", " ",
                                       new DateTime(), null, Race.Black, Gender.Female, null!,
                                       "dm@retrorabbit.co.za", "test@gmail.com", "0123456789", null, null,
                                       employeeAddressDto, employeeAddressDto, null, null, null);
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
