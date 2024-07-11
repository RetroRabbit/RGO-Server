using System.Linq.Expressions;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RGO.Tests.Services;

public class EmployeeDateServiceUnitTests
{
    private readonly EmployeeDateService _employeeDateService;
    private readonly Employee _employee;
    private readonly Mock<IUnitOfWork> _mockDb;
    private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;
    private readonly Mock<IEmployeeService> _employeeServiceMock;

    public EmployeeDateServiceUnitTests()
    {
        _mockDb = new Mock<IUnitOfWork>();
        _errorLoggingServiceMock = new Mock<IErrorLoggingService>();
        _employeeDateService = new EmployeeDateService(_mockDb.Object, _errorLoggingServiceMock.Object, _employeeServiceMock.Object);
        _employee = EmployeeTestData.EmployeeOne;
    }

    [Fact]
    public async Task CheckIfExistsFailTest()
    {
        _mockDb.Setup(x => x.EmployeeDate.Any(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .ReturnsAsync(false);

        var exists = await _employeeDateService.CheckIfExists(new EmployeeDate
        {
            Id = 1,
            Employee = _employee,
            Subject = "Subject",
            Note = "Note",
            Date = new DateOnly()
        }.ToDto());

        Assert.False(exists);
    }

    [Fact]
    public async Task CheckIfExistsPassTest()
    {
        _mockDb.Setup(x => x.EmployeeDate.Any(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .ReturnsAsync(true);

        var exists = await _employeeDateService.CheckIfExists(new EmployeeDate
        {
            Id = 1,
            Employee = _employee,
            Subject = "Subject",
            Note = "Note",
            Date = new DateOnly()
        }.ToDto());

        Assert.True(exists);
    }

    [Fact]
    public async Task SaveFailTest()
    {
        _mockDb.Setup(x => x.EmployeeDate.Any(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .ReturnsAsync(true);

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _employeeDateService.Save(new EmployeeDate
        {
            Id = 1,
            Employee = _employee,
            Subject = "Subject",
            Note = "Note",
            Date = new DateOnly()
        }.ToDto()));
    }

    [Fact]
    public async Task SavePassTest()
    {
        _mockDb.Setup(x => x.EmployeeDate.Any(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .ReturnsAsync(false);

        await _employeeDateService.Save(new EmployeeDate { Id = 1, Employee = _employee, Subject = "Subject", Note = "Note", Date = new DateOnly() }.ToDto());

        _mockDb.Verify(x => x.EmployeeDate.Add(It.IsAny<EmployeeDate>()), Times.Once);
    }

    [Fact]
    public async Task UpdateFailTest()
    {
        _mockDb.Setup(x => x.EmployeeDate.Any(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .ReturnsAsync(false);

        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _employeeDateService.Update(new EmployeeDate
        {
            Id = 1,
            Employee = _employee,
            Subject = "Subject",
            Note = "Note",
            Date = new DateOnly()
        }.ToDto()));
    }

    [Fact]
    public async Task UpdatePassTest()
    {
        _mockDb.Setup(x => x.EmployeeDate.Any(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .ReturnsAsync(true);

        await _employeeDateService.Update(new EmployeeDate
        {
            Id = 1,
            Employee = _employee,
            Subject = "Subject",
            Note = "Note",
            Date = new DateOnly()
        }.ToDto());

        _mockDb.Verify(x => x.EmployeeDate.Update(It.IsAny<EmployeeDate>()), Times.Once);
    }

    [Fact]
    public async Task DeletePassTest()
    {
        var employeeDate = new EmployeeDate { Id = 1, Employee = _employee, Subject = "Subject", Note = "Note", Date = new DateOnly() };

        _mockDb.Setup(x => x.EmployeeDate.Any(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .ReturnsAsync(true);

        _mockDb.Setup(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .Returns(employeeDate.ToMockIQueryable());

        await _employeeDateService.Delete(1);

        _mockDb.Verify(x => x.EmployeeDate.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetFailTest()
    {
        var employeeDate = new EmployeeDate { Id = 1, Employee = _employee, Subject = "Subject", Note = "Note", Date = new DateOnly() };

        _mockDb.Setup(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .Returns(new List<EmployeeDate>().ToMockIQueryable());
        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        await Assert.ThrowsAsync<Exception>(() => _employeeDateService.Get(employeeDate.ToDto()));
    }

    [Fact]
    public async Task GetPassTest()
    {
        var employeeDate = new EmployeeDate { Id = 1, Employee = _employee, Subject = "Subject", Note = "Note", Date = new DateOnly() };

        _mockDb.Setup(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .Returns(employeeDate.ToMockIQueryable());

        await _employeeDateService.Get(employeeDate.ToDto());

        _mockDb.Verify(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()), Times.Once);
    }

    [Fact(Skip = "Needs Works")]
    public void GetAllTest()
    {
        var employeeDateList = new EmployeeDate{ Id = 1, Employee = _employee, Subject = "Subject", Note = "Note", Date = new DateOnly()}.EntityToList();

        var employeeList = _employee.EntityToList();

        _mockDb.Setup(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .Returns(employeeDateList.ToMockIQueryable());

        _mockDb.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.ToMockIQueryable());

        var result = _employeeDateService.GetAll();

        Assert.Equivalent(employeeDateList.Count, result.Count);
    }

    [Fact(Skip = "Needs Works")]
    public void GetAllByEmployeeTest()
    {
        var employeeDateList = new EmployeeDate{ Id = 1, Employee = _employee, Subject = "Subject", Note = "Note", Date = new DateOnly()}.EntityToList();

        var employeeList = _employee.EntityToList();

        _mockDb.Setup(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .Returns(employeeDateList.ToMockIQueryable());

        _mockDb.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.ToMockIQueryable());

        var result = _employeeDateService.GetAllByEmployee(_employee.Email!);

        Assert.Equivalent(employeeDateList.Count, result.Count);
    }

    [Fact(Skip = "Needs Works")]
    public void GetAllByDateTest()
    {
        var employeeDateList = new EmployeeDate { Id = 1, Employee = _employee, Subject = "Subject", Note = "Note", Date = new DateOnly()}.EntityToList();

        _mockDb.Setup(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .Returns(employeeDateList.ToMockIQueryable());

        _mockDb.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(_employee.EntityToList().ToMockIQueryable());

        var result = _employeeDateService.GetAllByDate(new DateOnly());

        Assert.Equivalent(employeeDateList.Count, result.Count);
    }

    [Fact(Skip = "Needs Works")]
    public void GetAllBySubjectTest()
    {
        var employeeDateList = new EmployeeDate { Id = 1, Employee = _employee, Subject = "Subject", Note = "Note", Date = new DateOnly() }.EntityToList();

        var employeeList = _employee.EntityToList();

        _mockDb.Setup(x => x.EmployeeDate.Get(null))
               .Returns(employeeDateList.ToMockIQueryable());

        _mockDb.Setup(x => x.Employee.Get(null))
               .Returns(employeeList.ToMockIQueryable());

        var result = _employeeDateService.GetAllBySubject("Subject");

        Assert.Equivalent(employeeDateList.Count, result.Count);
    }
}