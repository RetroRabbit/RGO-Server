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

public class EmployeeQualificationServiceUnitTests
{
    private const int EmployeeId = 1;
    private const int QualificationId = 1;
    private readonly Mock<IUnitOfWork> _db;
    private readonly IEmployeeQualificationService _employeeQualificationService;
    private readonly Mock<IEmployeeService> _employeeService;
    private readonly Mock<AuthorizeIdentityMock> _identity;


    public EmployeeQualificationServiceUnitTests()
    {
        _db = new Mock<IUnitOfWork>();
        _identity = new Mock<AuthorizeIdentityMock>();
        _employeeService = new Mock<IEmployeeService>();
        _employeeQualificationService = new EmployeeQualificationService(_db.Object, _employeeService.Object, _identity.Object);
    }
    [Fact]
    public async Task CheckIfModelExistsReturnsTrue()
    {
        var Id = 1;
        _db.Setup(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
            .ReturnsAsync(true);

        var result = await _employeeQualificationService.CheckIfExists(Id);

        Assert.True(result);
        _db.Verify(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task CheckIfModelExistsReturnsFalse()
    {
        var Id = 10;
        _db.Setup(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
            .ReturnsAsync(false);

        var result = await _employeeQualificationService.CheckIfExists(Id);

        Assert.False(result);
        _db.Verify(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()), Times.Once);
    }
    #region GetAllEmployeeQualifications

    [Fact]
    public async Task GetAllEmployeeQualifications_Success()
    {
        _db.Setup(x => x.EmployeeQualification.Get(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
            .Returns(EmployeeQualificationTestData.EmployeeQualification.ToMockIQueryable());

        var result = await _employeeQualificationService.GetAllEmployeeQualifications();

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeQualificationTestData.EmployeeQualification.ToDto(), result[0]);

        _db.Verify(x => x.EmployeeQualification.Get(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),
            Times.Once);
    }

    #endregion

    #region SaveEmployeeQualification

    [Fact]
    public async Task SaveEmployeeQualification_Success()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

        _db.Setup(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
            .ReturnsAsync(false);
        _db.Setup(x => x.EmployeeQualification.Add(It.IsAny<EmployeeQualification>()))
            .ReturnsAsync(EmployeeQualificationTestData.EmployeeQualification);

        var result = await _employeeQualificationService.SaveEmployeeQualification(
                EmployeeQualificationTestData.EmployeeQualification.ToDto(), 1);

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeQualificationTestData.EmployeeQualification.ToDto(), result);

        _db.Verify(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),
           Times.Once);
        _db.Verify(x => x.EmployeeQualification.Add(It.IsAny<EmployeeQualification>()), Times.Once);
    }

    [Fact]
    public async Task SaveEmployeeQualification_UnauthorizedAccessFail()
    {
        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.SetupGet(i => i.EmployeeId).Returns(2);

        _db.Setup(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
        .ReturnsAsync(false);
 
        _db.Setup(x => x.EmployeeQualification.Add(It.IsAny<EmployeeQualification>()))
        .ReturnsAsync(EmployeeQualificationTestData.EmployeeQualification);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
           _employeeQualificationService.SaveEmployeeQualification(EmployeeQualificationTestData.EmployeeQualification.ToDto(), 1));

        Assert.Equivalent("Unauthorized access.", exception.Message);

        _db.Verify(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),
            Times.Once);
        _db.Verify(x => x.EmployeeQualification.Add(It.IsAny<EmployeeQualification>()), Times.Never);
    }

    [Fact]
    public async Task SaveEmployeeQualificationDoesNotExist()
    {
        _db.Setup(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
          .ReturnsAsync(true);

        _db.Setup(x => x.EmployeeQualification.Add(It.IsAny<EmployeeQualification>()))
        .ReturnsAsync(EmployeeQualificationTestData.EmployeeQualificationTwo);

        await Assert.ThrowsAsync<CustomException>(() => _employeeQualificationService.SaveEmployeeQualification(EmployeeQualificationTestData
                .EmployeeQualificationNew.ToDto(),1));

        _db.Verify(x => x.EmployeeQualification.Add(It.IsAny<EmployeeQualification>()), Times.Never);
        _db.Verify(x => x.EmployeeQualification.FirstOrDefault(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()), Times.Never);
    }

    #endregion

    #region GetEmployeeQualificationsByEmployeeId

    [Fact]
    public async Task GetEmployeeQualificationsByEmployeeId_Success()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

        _employeeService.Setup(x => x.GetEmployeeById(It.IsAny<int>()))
            .ReturnsAsync(EmployeeTestData.EmployeeOne.ToDto());
        _db.Setup(
                x => x.EmployeeQualification.FirstOrDefault(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
            .ReturnsAsync(EmployeeQualificationTestData.EmployeeQualification);

        var result = await _employeeQualificationService.GetEmployeeQualificationsByEmployeeId(It.IsAny<int>());

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeQualificationTestData.EmployeeQualification.ToDto(), result);

        _db.Verify(
            x => x.EmployeeQualification.FirstOrDefault(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),
            Times.Once);
    }

    #endregion
    [Fact]
    public async Task GetEmployeeQualificationsByEmployeeId_unAuthorizedAccess()
    {
        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.SetupGet(i => i.EmployeeId).Returns(2);

        _db.Setup(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
        .ReturnsAsync(true);

        _db.Setup(x => x.EmployeeQualification.GetById(It.IsAny<int>()))
        .ReturnsAsync(EmployeeQualificationTestData.EmployeeQualification);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
           _employeeQualificationService.GetEmployeeQualificationsByEmployeeId(EmployeeQualificationTestData.EmployeeQualification.EmployeeId));

        Assert.Equivalent("Unauthorized access.", exception.Message);

        _db.Verify(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),
            Times.Never);
        _db.Verify(x => x.EmployeeQualification.GetById(It.IsAny<int>()), Times.Never);
    }

    #region UpdateEmployeeQualification

    [Fact]
    public async Task UpdateEmployeeQualification_Success()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

        _db.Setup(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
        .ReturnsAsync(true);

        _db.Setup(
                x => x.EmployeeQualification.FirstOrDefault(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
            .ReturnsAsync(EmployeeQualificationTestData.EmployeeQualification);

        _db.Setup(x => x.EmployeeQualification.Update(It.IsAny<EmployeeQualification>()))
            .ReturnsAsync(EmployeeQualificationTestData.EmployeeQualification);

        var result = await _employeeQualificationService.UpdateEmployeeQualification(EmployeeQualificationTestData.EmployeeQualification.ToDto());

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeQualificationTestData.EmployeeQualification.ToDto(), result);

        _db.Verify(
            x => x.EmployeeQualification.FirstOrDefault(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),
            Times.Once);
        _db.Verify(x => x.EmployeeQualification.Update(It.IsAny<EmployeeQualification>()), Times.Once);
    }

    [Fact]
    public async Task UpdateEmployeeQaulificationDoesNotExist()
    {
        _db.Setup(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
           .ReturnsAsync(false);

        _db.Setup(r => r.EmployeeQualification.Update(It.IsAny<EmployeeQualification>()))
            .ReturnsAsync(EmployeeQualificationTestData.EmployeeQualificationTwo);

        await Assert.ThrowsAsync<CustomException>(() => _employeeQualificationService.UpdateEmployeeQualification(EmployeeQualificationTestData
                .EmployeeQualificationNew.ToDto()));

        _db.Verify(x => x.EmployeeQualification.Update(It.IsAny<EmployeeQualification>()), Times.Never);
        _db.Verify(x => x.EmployeeQualification.FirstOrDefault(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),Times.Never);
    }
    #endregion

    [Fact]
    public async Task UpdateEmployeeQualification_UnauthorizedAccessFail()
    {
        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.SetupGet(i => i.EmployeeId).Returns(2);

        _db.Setup(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
        .ReturnsAsync(true);

        _db.Setup(x => x.EmployeeQualification.Update(It.IsAny<EmployeeQualification>()))
        .ReturnsAsync(EmployeeQualificationTestData.EmployeeQualification);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
           _employeeQualificationService.UpdateEmployeeQualification(EmployeeQualificationTestData.EmployeeQualification.ToDto()));

        Assert.Equivalent("Unauthorized access.", exception.Message);

        _db.Verify(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),
            Times.Once);
        _db.Verify(x => x.EmployeeQualification.Update(It.IsAny<EmployeeQualification>()), Times.Never);
    }

    #region DeleteEmployeeQualification

    [Fact]
    public async Task DeleteEmployeeQualification_Success()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.SetupGet(i => i.EmployeeId).Returns(1);

        _db.Setup(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
            .ReturnsAsync(true);

        _db.Setup(x => x.EmployeeQualification.Delete(It.IsAny<int>()))
            .ReturnsAsync(EmployeeQualificationTestData.EmployeeQualificationTwo);

        var result = await _employeeQualificationService.DeleteEmployeeQualification(1);

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeQualificationTestData.EmployeeQualificationTwo.ToDto(), result);

        _db.Verify(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),
            Times.Once);
        _db.Verify(x => x.EmployeeQualification.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteEmployeeQualification_Failure_QualificationNotExists()
    {
        _db.Setup(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
            .ReturnsAsync(true);

        _db.Setup(x => x.EmployeeQualification.Delete(It.IsAny<int>()))
            .ReturnsAsync(EmployeeQualificationTestData.EmployeeQualification);

        await Assert.ThrowsAsync<CustomException>(() => _employeeQualificationService.UpdateEmployeeQualification(EmployeeQualificationTestData
                .EmployeeQualificationNew.ToDto()));

        _db.Verify(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),
            Times.Once);
        _db.Verify(x => x.EmployeeQualification.Delete(It.IsAny<int>()), Times.Never);
    }

    #endregion

    [Fact]
    public async Task DeleteEmployeeQualification_UnauthorizedAccessFail()
    {
        _identity.Setup(i => i.Role).Returns("Employee");

        _db.Setup(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
        .ReturnsAsync(true);

        _db.Setup(x => x.EmployeeQualification.Delete(It.IsAny<int>()))
                  .ReturnsAsync(EmployeeQualificationTestData.EmployeeQualification);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
           _employeeQualificationService.DeleteEmployeeQualification((EmployeeQualificationTestData.EmployeeQualification.Id)));

        Assert.Equivalent("Unauthorized access.", exception.Message);

        _db.Verify(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),
            Times.Once);
        _db.Verify(x => x.EmployeeQualification.Delete(It.IsAny<int>()), Times.Never);
    }

}