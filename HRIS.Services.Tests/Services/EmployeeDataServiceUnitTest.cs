using System.Linq.Expressions;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class EmployeeDataServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly EmployeeDataService _employeeDataService;
    private readonly AuthorizeIdentityMock _supportIdentity;
    private readonly AuthorizeIdentityMock _nonSupportIdentity;
    private readonly EmployeeData _employeeData = EmployeeDataTestData.EmployeeDataOne;

    public EmployeeDataServiceUnitTest()
    {
        _supportIdentity = new AuthorizeIdentityMock("test@gmail.com", "test", "Admin", 1);
        _nonSupportIdentity = new AuthorizeIdentityMock("test@gmail.com", "test", "User", 1);
        _dbMock = new Mock<IUnitOfWork>();
        _employeeDataService = new EmployeeDataService(_dbMock.Object, _supportIdentity);
        _employeeData = EmployeeDataTestData.EmployeeDataOne;
    }

    [Fact]
    public async Task CheckIfModelExistsReturnsTrue()
    {
        var testId = 1;
        _dbMock.Setup(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
            .ReturnsAsync(true);

        var result = await _employeeDataService.EmployeeDataExists(testId);

        Assert.True(result);
        _dbMock.Verify(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task CheckIfModelExistsReturnsFalse()
    {
        var testId = 1;
        _dbMock.Setup(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
            .ReturnsAsync(false);

        var result = await _employeeDataService.EmployeeDataExists(testId);

        Assert.False(result);
        _dbMock.Verify(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetAllEmployeeDataTest()
    {
        var employeeId = EmployeeDataTestData.EmployeeDataOne.EmployeeId;
        var employeeDataList = new List<EmployeeData> { EmployeeDataTestData.EmployeeDataOne };

        _dbMock.Setup(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeData.GetAll(It.IsAny<Expression<Func<EmployeeData, bool>>>())).ReturnsAsync(employeeDataList);

        var result = await _employeeDataService.GetAllEmployeeData(employeeId);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equivalent(employeeDataList.Select(x => x.ToDto()).ToList(), result);
        _dbMock.Verify(x => x.EmployeeData.GetAll(It.IsAny<Expression<Func<EmployeeData, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetAllEmployeeDataFail_Unauthorized()
    {
        var employeeId = EmployeeDataTestData.EmployeeDataOne.EmployeeId;

        var dataServiceWithNonSupportIdentity = new EmployeeDataService(_dbMock.Object, _nonSupportIdentity);

        _dbMock.Setup(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
           .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => dataServiceWithNonSupportIdentity.GetAllEmployeeData(employeeId));
    }

    //[Fact(Skip = "Fix unit test")]
    //public async Task GetAllEmployeeDataTest()
    //{
    //    var employee = EmployeeDataTestData.EmployeeDataOne.EntityToList();

    //    _dbMock.Setup(x => x.EmployeeData.GetAll(It.IsAny<Expression<Func<EmployeeData, bool>>>())).ReturnsAsync(employee);
    //    var result = await _employeeDataService.GetAllEmployeeData(EmployeeDataTestData.EmployeeDataOne.EmployeeId);

    //    Assert.NotNull(result);
    //    Assert.Single(result);
    //    Assert.Equivalent(employee.Select(x => x.ToDto()).ToList(), result);
    //}

    [Fact]
    public async Task GetEmployeeDataTestPass()
    {
        var employeeId = EmployeeDataTestData.EmployeeDataOne.EmployeeId;
        var value = EmployeeDataTestData.EmployeeDataOne.Value;
        var employeeDataList = new List<EmployeeData> { EmployeeDataTestData.EmployeeDataOne };

        _dbMock.Setup(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeData.GetAll(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
            .ReturnsAsync(employeeDataList);

        var result = await _employeeDataService.GetEmployeeData(employeeId, value);

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeDataTestData.EmployeeDataOne.ToDto(), result);
        _dbMock.Verify(x => x.EmployeeData.GetAll(It.IsAny<Expression<Func<EmployeeData, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetEmployeeDataFail_Unauthorized()
    {
        var employeeId = EmployeeDataTestData.EmployeeDataOne.EmployeeId;
        var value = EmployeeDataTestData.EmployeeDataOne.Value;

        var dataServiceWithNonSupportIdentity = new EmployeeDataService(_dbMock.Object, _nonSupportIdentity);

        _dbMock.Setup(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
           .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => dataServiceWithNonSupportIdentity.GetEmployeeData(employeeId, value));
    }

    //[Fact(Skip = "Fix unit test")]
    //public async Task GetEmployeeDataTest()
    //{
    //    var employee = EmployeeDataTestData.EmployeeDataOne.EntityToList();

    //    _dbMock.Setup(x => x.EmployeeData.GetAll(null)).ReturnsAsync(employee);
    //    var result = await _employeeDataService.GetEmployeeData(EmployeeDataTestData.EmployeeDataOne.EmployeeId, EmployeeDataTestData.EmployeeDataOne.Value);

    //    Assert.NotNull(result);
    //    Assert.Equivalent(EmployeeDataTestData.EmployeeDataOne.ToDto(), result);
    //    _dbMock.Verify(x => x.EmployeeData.GetAll(null), Times.Once);
    //}

    [Fact]
    public async Task SaveEmployeeDataTestPass()
    {
        var newEmployeeDataDto = EmployeeDataTestData.EmployeeDataTwo.ToDto();
        var employeeDataList = new List<EmployeeData> { EmployeeDataTestData.EmployeeDataOne };

        _dbMock.Setup(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeData.GetAll(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
            .ReturnsAsync(employeeDataList);

        _dbMock.Setup(x => x.EmployeeData.Add(It.IsAny<EmployeeData>()))
            .ReturnsAsync(EmployeeDataTestData.EmployeeDataOne);

        var result = await _employeeDataService.SaveEmployeeData(newEmployeeDataDto);

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeDataTestData.EmployeeDataOne.ToDto(), result);
        _dbMock.Verify(x => x.EmployeeData.Add(It.IsAny<EmployeeData>()), Times.Once);
    }

    [Fact]
    public async Task SaveEmployeeDataFail_Unauthorized()
    {
        var dataServiceWithNonSupportIdentity = new EmployeeDataService(_dbMock.Object, _nonSupportIdentity);

        _dbMock.Setup(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
           .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => dataServiceWithNonSupportIdentity.SaveEmployeeData(_employeeData.ToDto()));
    }

    //[Fact(Skip = "Fix unit test")]
    //public async Task SaveEmployeeDataTest()
    //{
    //    var employee = EmployeeDataTestData.EmployeeDataOne.EntityToList();
    //    _dbMock.Setup(x => x.EmployeeData.GetAll(null)).ReturnsAsync(employee);

    //    _dbMock.Setup(x => x.EmployeeData.Add(It.IsAny<EmployeeData>()))
    //           .ReturnsAsync(EmployeeDataTestData.EmployeeDataOne);

    //    var result = await _employeeDataService.SaveEmployeeData(EmployeeDataTestData.EmployeeDataTwo.ToDto());
    //    Assert.NotNull(result);
    //    Assert.Equivalent(EmployeeDataTestData.EmployeeDataOne.ToDto(), result);
    //    _dbMock.Verify(x => x.EmployeeData.Add(It.IsAny<EmployeeData>()), Times.Once);
    //}

    [Fact]
    public async Task UpdateEmployeeDataTest()
    {
        var updatedEmployeeDataDto = EmployeeDataTestData.EmployeeDataOne.ToDto();
        var employeeDataList = new List<EmployeeData> { EmployeeDataTestData.EmployeeDataOne };

        _dbMock.Setup(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeData.GetAll(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
            .ReturnsAsync(employeeDataList);

        _dbMock.Setup(x => x.EmployeeData.Update(It.IsAny<EmployeeData>()))
            .ReturnsAsync(EmployeeDataTestData.EmployeeDataOne);

        var result = await _employeeDataService.UpdateEmployeeData(updatedEmployeeDataDto);

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeDataTestData.EmployeeDataOne.ToDto(), result);
        _dbMock.Verify(x => x.EmployeeData.Update(It.IsAny<EmployeeData>()), Times.Once);
    }

    [Fact]
    public async Task UpdateEmployeeDataFail_Unauthorized()
    {
        var dataServiceWithNonSupportIdentity = new EmployeeDataService(_dbMock.Object, _nonSupportIdentity);

        _dbMock.Setup(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
           .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => dataServiceWithNonSupportIdentity.UpdateEmployeeData(_employeeData.ToDto()));
    }

    //[Fact(Skip = "Fix unit test")]
    //public async Task UpdateEmployeeDataTest()
    //{
    //    var employee = EmployeeDataTestData.EmployeeDataOne.EntityToList();
    //    _dbMock.Setup(x => x.EmployeeData.GetAll(null)).ReturnsAsync(employee);

    //    _dbMock.Setup(x => x.EmployeeData.Update(It.IsAny<EmployeeData>()))
    //           .ReturnsAsync(EmployeeDataTestData.EmployeeDataOne);

    //    var result = await _employeeDataService.UpdateEmployeeData(EmployeeDataTestData.EmployeeDataOne.ToDto());
    //    Assert.NotNull(result);
    //    Assert.Equivalent(EmployeeDataTestData.EmployeeDataOne.ToDto(), result);
    //    _dbMock.Verify(x => x.EmployeeData.Update(It.IsAny<EmployeeData>()), Times.Once);
    //}

    [Fact]
    public async Task DeleteEmployeeDataTest()
    {
        var employeeDataId = EmployeeDataTestData.EmployeeDataOne.Id;

        _dbMock.Setup(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeData.Delete(It.IsAny<int>()))
            .ReturnsAsync(EmployeeDataTestData.EmployeeDataOne);

        var result = await _employeeDataService.DeleteEmployeeData(employeeDataId);

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeDataTestData.EmployeeDataOne.ToDto(), result);
        _dbMock.Verify(x => x.EmployeeData.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteEmployeeDataFail_Unauthorized()
    {
        var employeeDataId = EmployeeDataTestData.EmployeeDataOne.Id;

        var dataServiceWithNonSupportIdentity = new EmployeeDataService(_dbMock.Object, _nonSupportIdentity);

        _dbMock.Setup(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
           .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => dataServiceWithNonSupportIdentity.DeleteEmployeeData(employeeDataId));
    }

    //[Fact(Skip = "Fix unit test")]
    //public async Task DeleteEmployeeDataTest()
    //{
    //    var employee = EmployeeDataTestData.EmployeeDataOne.EntityToList();
    //    _dbMock.Setup(x => x.EmployeeData.GetAll(null)).ReturnsAsync(employee);

    //    _dbMock.Setup(x => x.EmployeeData.Delete(It.IsAny<int>()))
    //           .ReturnsAsync(EmployeeDataTestData.EmployeeDataOne);

    //    var result = await _employeeDataService.DeleteEmployeeData(EmployeeDataTestData.EmployeeDataOne.Id);
    //    Assert.NotNull(result);
    //    Assert.Equivalent(EmployeeDataTestData.EmployeeDataOne.ToDto(), result);
    //    _dbMock.Verify(r => r.EmployeeData.Delete(It.IsAny<int>()), Times.Once);
    //}
}