using System.Linq.Expressions;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using HRIS.Services.Session;
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
    public async Task GetEmployeeDataTestPass()
    {
        var employeeId = _employeeData.EmployeeId;
        var employeeData = _employeeData;

        _dbMock.Setup(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>())).ReturnsAsync(true);
        _dbMock.Setup(x => x.EmployeeData.GetById(employeeId)).ReturnsAsync(employeeData);

        var result = await _employeeDataService.GetEmployeeData(employeeId);

        Assert.NotNull(result);
        Assert.Equivalent(employeeData.ToDto(), result);
        _dbMock.Verify(x => x.EmployeeData.GetById(employeeId), Times.Once);
    }

    [Fact]
    public async Task GetEmployeeDataFail_Unauthorized()
    {
        var employeeId = EmployeeDataTestData.EmployeeDataOne.EmployeeId;
        var dataServiceWithNonSupportIdentity = new EmployeeDataService(_dbMock.Object, _nonSupportIdentity);

        _dbMock.Setup(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
           .ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => dataServiceWithNonSupportIdentity.GetEmployeeData(employeeId));
    }

    [Fact]
    public async Task GetEmployeeDataTest_NoModelFound()
    {
        var employeeId = EmployeeDataTestData.EmployeeDataOne.EmployeeId;

        _dbMock.Setup(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => _employeeDataService.GetEmployeeData(employeeId));
    }

    [Fact]
    public async Task GetEmployeeDataTest_NoRecordFoundInDatabase()
    {
        var employeeId = EmployeeDataTestData.EmployeeDataOne.EmployeeId;

        _dbMock.Setup(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeData.GetById(employeeId))
            .ReturnsAsync((EmployeeData)null);

        await Assert.ThrowsAsync<CustomException>(() => _employeeDataService.GetEmployeeData(employeeId));
    }



    [Fact]
    public async Task CreateEmployeeDataTest_Pass()
    {
        var newEmployeeDataDto = EmployeeDataTestData.EmployeeDataTwo.ToDto();

        _dbMock.Setup(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>())).ReturnsAsync(false);
        _dbMock.Setup(x => x.EmployeeData.Add(It.IsAny<EmployeeData>())).ReturnsAsync(EmployeeDataTestData.EmployeeDataTwo);

        var result = await _employeeDataService.CreateEmployeeData(newEmployeeDataDto);

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeDataTestData.EmployeeDataTwo.ToDto(), result);
        _dbMock.Verify(x => x.EmployeeData.Add(It.IsAny<EmployeeData>()), Times.Once);
    }

    [Fact]
    public async Task CreateEmployeeDataTest_UnauthorizedAccess()
    {
        var dataServiceWithNonSupportIdentity = new EmployeeDataService(_dbMock.Object, _nonSupportIdentity);
        var newEmployeeDataDto = EmployeeDataTestData.EmployeeDataTwo.ToDto();
        newEmployeeDataDto.EmployeeId = 2;

        _dbMock.Setup(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<CustomException>(() => dataServiceWithNonSupportIdentity.CreateEmployeeData(newEmployeeDataDto));
    }


    [Fact]
    public async Task CreateEmployeeDataTest_ExistingRecord()
    {
        var newEmployeeDataDto = EmployeeDataTestData.EmployeeDataTwo.ToDto();

        _dbMock.Setup(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<CustomException>(() => _employeeDataService.CreateEmployeeData(newEmployeeDataDto));
    }

    [Fact]
    public async Task UpdateEmployeeDataTest()
    {
        var updatedEmployeeDataDto = EmployeeDataTestData.EmployeeDataOne.ToDto();

        _dbMock.Setup(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>())).ReturnsAsync(true);
        _dbMock.Setup(x => x.EmployeeData.GetById(updatedEmployeeDataDto.EmployeeId)).ReturnsAsync(EmployeeDataTestData.EmployeeDataOne);
        _dbMock.Setup(x => x.EmployeeData.Update(It.IsAny<EmployeeData>())).ReturnsAsync(EmployeeDataTestData.EmployeeDataOne);

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

    [Fact]
    public async Task UpdateEmployeeDataFail_NoRecordFound()
    {
        var updatedEmployeeDataDto = EmployeeDataTestData.EmployeeDataOne.ToDto();
        var employeeDataList = new List<EmployeeData>();

        _dbMock.Setup(x => x.EmployeeData.Any(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
            .ReturnsAsync(true);

        _dbMock.Setup(x => x.EmployeeData.GetAll(It.IsAny<Expression<Func<EmployeeData, bool>>>()))
            .ReturnsAsync(employeeDataList);

        await Assert.ThrowsAsync<CustomException>(() => _employeeDataService.UpdateEmployeeData(updatedEmployeeDataDto));
        _dbMock.Verify(x => x.EmployeeData.Update(It.IsAny<EmployeeData>()), Times.Never);
    }

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
}