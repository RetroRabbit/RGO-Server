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

public class EmployeeDataServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly EmployeeDataService _employeeDataService;

    public EmployeeDataServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        Mock<IErrorLoggingService> errorLoggingServiceMock = new();
        _employeeDataService = new EmployeeDataService(_dbMock.Object, errorLoggingServiceMock.Object);
    }

    [Fact]
    public async Task GetAllEmployeeDataTest()
    {
        var employee = EmployeeDataTestData.EmployeeDataOne.EntityToList();

        _dbMock.Setup(x => x.EmployeeData.GetAll(It.IsAny<Expression<Func<EmployeeData, bool>>>())).ReturnsAsync(employee);
        var result = await _employeeDataService.GetAllEmployeeData(EmployeeDataTestData.EmployeeDataOne.EmployeeId);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equivalent(employee.Select(x => x.ToDto()).ToList(), result);
    }

    [Fact]
    public async Task GetEmployeeDataTest()
    {
        var employee = EmployeeDataTestData.EmployeeDataOne.EntityToList();

        _dbMock.Setup(x => x.EmployeeData.GetAll(null)).ReturnsAsync(employee);
        var result = await _employeeDataService.GetEmployeeData(EmployeeDataTestData.EmployeeDataOne.EmployeeId, EmployeeDataTestData.EmployeeDataOne.Value);

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeDataTestData.EmployeeDataOne.ToDto(), result);
        _dbMock.Verify(x => x.EmployeeData.GetAll(null), Times.Once);
    }

    [Fact]
    public async Task SaveEmployeeDataTest()
    {
        var employee = EmployeeDataTestData.EmployeeDataOne.EntityToList();
        _dbMock.Setup(x => x.EmployeeData.GetAll(null)).ReturnsAsync(employee);

        _dbMock.Setup(x => x.EmployeeData.Add(It.IsAny<EmployeeData>()))
               .ReturnsAsync(EmployeeDataTestData.EmployeeDataOne);

        var result = await _employeeDataService.SaveEmployeeData(EmployeeDataTestData.EmployeeDataTwo.ToDto());
        Assert.NotNull(result);
        Assert.Equivalent(EmployeeDataTestData.EmployeeDataOne.ToDto(), result);
        _dbMock.Verify(x => x.EmployeeData.Add(It.IsAny<EmployeeData>()), Times.Once);
    }

    [Fact]
    public async Task UpdateEmployeeDataTest()
    {
        var employee = EmployeeDataTestData.EmployeeDataOne.EntityToList();
        _dbMock.Setup(x => x.EmployeeData.GetAll(null)).ReturnsAsync(employee);

        _dbMock.Setup(x => x.EmployeeData.Update(It.IsAny<EmployeeData>()))
               .ReturnsAsync(EmployeeDataTestData.EmployeeDataOne);

        var result = await _employeeDataService.UpdateEmployeeData(EmployeeDataTestData.EmployeeDataOne.ToDto());
        Assert.NotNull(result);
        Assert.Equivalent(EmployeeDataTestData.EmployeeDataOne.ToDto(), result);
        _dbMock.Verify(x => x.EmployeeData.Update(It.IsAny<EmployeeData>()), Times.Once);
    }

    [Fact]
    public async Task DeleteEmployeeDataTest()
    {
        var employee = EmployeeDataTestData.EmployeeDataOne.EntityToList();
        _dbMock.Setup(x => x.EmployeeData.GetAll(null)).ReturnsAsync(employee);

        _dbMock.Setup(x => x.EmployeeData.Delete(It.IsAny<int>()))
               .ReturnsAsync(EmployeeDataTestData.EmployeeDataOne);

        var result = await _employeeDataService.DeleteEmployeeData(EmployeeDataTestData.EmployeeDataOne.Id);
        Assert.NotNull(result);
        Assert.Equivalent(EmployeeDataTestData.EmployeeDataOne.ToDto(), result);
        _dbMock.Verify(r => r.EmployeeData.Delete(It.IsAny<int>()), Times.Once);
    }
}