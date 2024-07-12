using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class EmployeeDateServiceUnitTests
{
    private readonly EmployeeDateService _employeeDateService;
    private readonly Employee _employee;
    private readonly Mock<IUnitOfWork> _mockDb;
    private readonly EmployeeDate _employeeDate;
    private readonly EmployeeDateDto _employeeDateDto;
    private readonly EmployeeDateInput _employeeDateInput;
    private readonly Mock<IEmployeeService> _employeeServiceMock;

    public EmployeeDateServiceUnitTests()
    {
        _mockDb = new Mock<IUnitOfWork>();
        _employeeServiceMock = new Mock<IEmployeeService>();
        _employeeDateService = new EmployeeDateService(_mockDb.Object, _employeeServiceMock.Object);
        _employee = EmployeeTestData.EmployeeOne;
        _employeeDate = new EmployeeDate { Id = 1, EmployeeId = _employee.Id, Employee = _employee, Subject = "Subject", Note = "Note", Date = new DateOnly() };
        _employeeDateDto = new EmployeeDate { Id = 1, Employee = _employee, Subject = "Subject", Note = "Note", Date = new DateOnly() }.ToDto();
        _employeeDateInput = new EmployeeDateInput { Id = 1, Email = "email@gmail.com", Subject = "Subject", Note = "Note", Date = new DateOnly() };
    }

    [Fact]
    public async Task CheckIfExistsFailTest()
    {
        _mockDb.Setup(x => x.EmployeeDate.Any(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .ReturnsAsync(false);

        var exists = await _employeeDateService.CheckIfExists(_employeeDateDto);
        Assert.False(exists);
    }

    [Fact]
    public async Task CheckIfExistsPassTest()
    {
        _mockDb.Setup(x => x.EmployeeDate.Any(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .ReturnsAsync(true);

        var exists = await _employeeDateService.CheckIfExists(_employeeDateDto);
        Assert.True(exists);
    }

    [Fact]
    public async Task SaveFailTest()
    {
        _mockDb.Setup(x => x.EmployeeDate.Any(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _employeeDateService.SaveEmployeeDate(_employeeDateInput));
    }

    [Fact]
    public async Task SavePassTest()
    {
        _mockDb.Setup(x => x.EmployeeDate.Any(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .ReturnsAsync(false);

        await _employeeDateService.SaveEmployeeDate(_employeeDateInput);
        _mockDb.Verify(x => x.EmployeeDate.Add(It.IsAny<EmployeeDate>()), Times.Once);
    }

    [Fact]
    public async Task UpdateFailTest()
    {
        _mockDb.Setup(x => x.EmployeeDate.Any(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _employeeDateService.UpdateEmployeeDate(_employeeDate.ToDto()));
    }

    [Fact]
    public async Task UpdatePassTest()
    {
        _mockDb.Setup(x => x.EmployeeDate.Any(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .ReturnsAsync(true);

        await _employeeDateService.UpdateEmployeeDate(_employeeDateDto);
        _mockDb.Verify(x => x.EmployeeDate.Update(It.IsAny<EmployeeDate>()), Times.Once);
    }

    [Fact]
    public async Task DeletePassTest()
    {
        _mockDb.Setup(x => x.EmployeeDate.Any(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .ReturnsAsync(true);

        _mockDb.Setup(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .Returns(_employeeDate.ToMockIQueryable());

        await _employeeDateService.Delete(1);
        _mockDb.Verify(x => x.EmployeeDate.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task GetFailTest()
    {
        _mockDb.Setup(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .Returns(new List<EmployeeDate>().ToMockIQueryable());
        
        await Assert.ThrowsAsync<CustomException>(() => _employeeDateService.Get(_employeeDate.ToDto()));
    }

    [Fact]
    public async Task GetPassTest()
    {
        _mockDb.Setup(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .Returns(_employeeDate.ToMockIQueryable());

        await _employeeDateService.Get(_employeeDate.ToDto());
        _mockDb.Verify(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()), Times.Once);
    }

    [Fact]
    public void GetAllTest()
    {
        var employeeDateList = _employeeDate.EntityToList();

        var employeeList = _employee.EntityToList();

        _mockDb.Setup(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .Returns(employeeDateList.ToMockIQueryable());

        _mockDb.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.ToMockIQueryable());

        var result = _employeeDateService.GetAll();

        Assert.Equivalent(employeeDateList.Count, result.Count);
    }

    [Fact]
    public void GetAllByEmployeeTest()
    {
        var employeeDateList = _employeeDate.EntityToList();

        var employeeList = _employee.EntityToList();

        _mockDb.Setup(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .Returns(employeeDateList.ToMockIQueryable());

        _mockDb.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employeeList.ToMockIQueryable());

        var result = _employeeDateService.GetAllByEmployee(_employee.Email!);

        Assert.Equivalent(employeeDateList.Count, result.Count);
    }

    [Fact]
    public void GetAllByDateTest()
    {
        var employeeDateList = _employeeDate.EntityToList();

        _mockDb.Setup(x => x.EmployeeDate.Get(It.IsAny<Expression<Func<EmployeeDate, bool>>>()))
               .Returns(employeeDateList.ToMockIQueryable());

        _mockDb.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(_employee.EntityToList().ToMockIQueryable());

        var result = _employeeDateService.GetAllByDate(It.IsAny<DateOnly>());

        Assert.Equivalent(employeeDateList.Count, result.Count);
    }

    [Fact]
    public void GetAllBySubjectTest()
    {
        var employeeDateList = _employeeDate.EntityToList();

        var employeeList = _employee.EntityToList();

        _mockDb.Setup(x => x.EmployeeDate.Get(null))
               .Returns(employeeDateList.ToMockIQueryable());

        _mockDb.Setup(x => x.Employee.Get(null))
               .Returns(employeeList.ToMockIQueryable());

        var result = _employeeDateService.GetAllBySubject("Subject");

        Assert.Equivalent(employeeDateList.Count, result.Count);
    }
}