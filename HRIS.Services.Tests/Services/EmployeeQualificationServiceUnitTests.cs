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

    public EmployeeQualificationServiceUnitTests()
    {
        _db = new Mock<IUnitOfWork>();
        _employeeService = new Mock<IEmployeeService>();
        _employeeQualificationService = new EmployeeQualificationService(_db.Object, _employeeService.Object);
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
        _employeeService.Setup(x => x.GetEmployeeById(It.IsAny<int>()))
            .ReturnsAsync(EmployeeTestData.EmployeeOne.ToDto());
        _db.Setup(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
            .ReturnsAsync(false);
        _db.Setup(x => x.EmployeeQualification.Add(It.IsAny<EmployeeQualification>()))
            .ReturnsAsync(EmployeeQualificationTestData.EmployeeQualification);

        var result =
            await _employeeQualificationService.SaveEmployeeQualification(
                EmployeeQualificationTestData.EmployeeQualificationNew.ToDto(), EmployeeId);

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeQualificationTestData.EmployeeQualification.ToDto(), result);

        _employeeService.Verify(x => x.GetEmployeeById(It.IsAny<int>()), Times.Once);
        _db.Verify(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),
            Times.Once);
        _db.Verify(x => x.EmployeeQualification.Add(It.IsAny<EmployeeQualification>()), Times.Once);
    }

    [Fact]
    public async Task SaveEmployeeQualification_Failure_GetEmployeeById()
    {
        _employeeService.Setup(x => x.GetEmployeeById(It.IsAny<int>()))
            .Throws(new CustomException("Unable to Load Employee"));

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
            _employeeQualificationService.SaveEmployeeQualification(It.IsAny<EmployeeQualificationDto>(),
                It.IsAny<int>()));

        Assert.Equivalent("Unable to Load Employee", exception.Message);

        _employeeService.Verify(x => x.GetEmployeeById(It.IsAny<int>()), Times.Once);
        _db.Verify(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),
            Times.Never);
        _db.Verify(x => x.EmployeeQualification.Add(It.IsAny<EmployeeQualification>()), Times.Never);
    }

    [Fact]
    public async Task SaveEmployeeQualification_Failure_QualificationExists()
    {
        _employeeService.Setup(x => x.GetEmployeeById(It.IsAny<int>()))
            .ReturnsAsync(EmployeeTestData.EmployeeOne.ToDto);
        _db.Setup(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
            .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
            _employeeQualificationService.SaveEmployeeQualification(It.IsAny<EmployeeQualificationDto>(),
                It.IsAny<int>()));

        Assert.Equivalent("Employee Already Have Existing Qualifications Saved", exception.Message);

        _employeeService.Verify(x => x.GetEmployeeById(It.IsAny<int>()), Times.Once);
        _db.Verify(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),
            Times.Once);
        _db.Verify(x => x.EmployeeQualification.Add(It.IsAny<EmployeeQualification>()), Times.Never);
    }

    #endregion

    #region GetEmployeeQualificationsByEmployeeId

    [Fact]
    public async Task GetEmployeeQualificationsByEmployeeId_Success()
    {
        _employeeService.Setup(x => x.GetEmployeeById(It.IsAny<int>()))
            .ReturnsAsync(EmployeeTestData.EmployeeOne.ToDto());
        _db.Setup(
                x => x.EmployeeQualification.FirstOrDefault(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
            .ReturnsAsync(EmployeeQualificationTestData.EmployeeQualification);

        var result = await _employeeQualificationService.GetEmployeeQualificationsByEmployeeId(It.IsAny<int>());

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeQualificationTestData.EmployeeQualification.ToDto(), result);

        _employeeService.Verify(x => x.GetEmployeeById(It.IsAny<int>()), Times.Once);
        _db.Verify(
            x => x.EmployeeQualification.FirstOrDefault(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),
            Times.Once);
    }

    [Fact]
    public async Task GetEmployeeQualificationsByEmployeeId_Failure_GetEmployeeById()
    {
        _employeeService.Setup(x => x.GetEmployeeById(It.IsAny<int>()))
            .Throws(new CustomException("Unable to Load Employee"));

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
            _employeeQualificationService.GetEmployeeQualificationsByEmployeeId(It.IsAny<int>()));

        Assert.Equivalent("Unable to Load Employee", exception.Message);

        _employeeService.Verify(x => x.GetEmployeeById(It.IsAny<int>()), Times.Once);
        _db.Verify(
            x => x.EmployeeQualification.FirstOrDefault(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),
            Times.Never);
    }

    [Fact]
    public async Task GetEmployeeQualificationsByEmployeeId_Failure_NoQualifications()
    {
        _employeeService.Setup(x => x.GetEmployeeById(It.IsAny<int>()))
            .ReturnsAsync(EmployeeTestData.EmployeeOne.ToDto());
        _db.Setup(
                x => x.EmployeeQualification.FirstOrDefault(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
            .ReturnsAsync(default(EmployeeQualification));

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
            _employeeQualificationService.GetEmployeeQualificationsByEmployeeId(EmployeeId));

        Assert.Equivalent("Unable to Load Employee Qualifications", exception.Message);

        _employeeService.Verify(x => x.GetEmployeeById(It.IsAny<int>()), Times.Once);
        _db.Verify(
            x => x.EmployeeQualification.FirstOrDefault(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),
            Times.Once);
    }

    #endregion

    #region UpdateEmployeeQualification

    [Fact]
    public async Task UpdateEmployeeQualification_Success()
    {
        _employeeService.Setup(x => x.GetEmployeeById(It.IsAny<int>()))
            .ReturnsAsync(EmployeeTestData.EmployeeOne.ToDto());
        _db.Setup(
                x => x.EmployeeQualification.FirstOrDefault(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
            .ReturnsAsync(EmployeeQualificationTestData.EmployeeQualification);
        _db.Setup(x => x.EmployeeQualification.Update(It.IsAny<EmployeeQualification>()))
            .ReturnsAsync(EmployeeQualificationTestData.EmployeeQualification);

        var result =
            await _employeeQualificationService.UpdateEmployeeQualification(EmployeeQualificationTestData
                .EmployeeQualificationNew.ToDto());

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeQualificationTestData.EmployeeQualification.ToDto(), result);

        _employeeService.Verify(x => x.GetEmployeeById(It.IsAny<int>()), Times.Once);
        _db.Verify(
            x => x.EmployeeQualification.FirstOrDefault(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),
            Times.Once);
        _db.Verify(x => x.EmployeeQualification.Update(It.IsAny<EmployeeQualification>()), Times.Once);
    }

    [Fact]
    public async Task UpdateEmployeeQualification_Failure_GetEmployeeById()
    {
        _employeeService.Setup(x => x.GetEmployeeById(It.IsAny<int>()))
            .Throws(new CustomException("Unable to Load Employee"));

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
            _employeeQualificationService.UpdateEmployeeQualification(EmployeeQualificationTestData
                .EmployeeQualificationNew.ToDto()));

        Assert.Equivalent("Unable to Load Employee", exception.Message);

        _employeeService.Verify(x => x.GetEmployeeById(It.IsAny<int>()), Times.Once);
        _db.Verify(
            x => x.EmployeeQualification.FirstOrDefault(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),
            Times.Never);
        _db.Verify(x => x.EmployeeQualification.Update(It.IsAny<EmployeeQualification>()), Times.Never);
    }

    [Fact]
    public async Task UpdateEmployeeQualification_Failure_QualificationNotExists()
    {
        _employeeService.Setup(x => x.GetEmployeeById(It.IsAny<int>()))
            .ReturnsAsync(EmployeeTestData.EmployeeOne.ToDto());
        _db.Setup(
                x => x.EmployeeQualification.FirstOrDefault(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
            .ReturnsAsync(default(EmployeeQualification));

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
            _employeeQualificationService.UpdateEmployeeQualification(EmployeeQualificationTestData
                .EmployeeQualificationNew.ToDto()));

        Assert.Equivalent("Employee Does Not Have Existing Qualifications Saved", exception.Message);

        _employeeService.Verify(x => x.GetEmployeeById(It.IsAny<int>()), Times.Once);
        _db.Verify(
            x => x.EmployeeQualification.FirstOrDefault(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),
            Times.Once);
        _db.Verify(x => x.EmployeeQualification.Update(It.IsAny<EmployeeQualification>()), Times.Never);
    }

    #endregion

    #region DeleteEmployeeQualification

    [Fact]
    public async Task DeleteEmployeeQualification_Success()
    {
        _db.Setup(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
            .ReturnsAsync(true);
        _db.Setup(x => x.EmployeeQualification.Delete(It.IsAny<int>()))
            .ReturnsAsync(EmployeeQualificationTestData.EmployeeQualification);

        var result = await _employeeQualificationService.DeleteEmployeeQualification(It.IsAny<int>());

        Assert.NotNull(result);
        Assert.Equivalent(EmployeeQualificationTestData.EmployeeQualification.ToDto(), result);

        _db.Verify(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),
            Times.Once);
        _db.Verify(x => x.EmployeeQualification.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteEmployeeQualification_Failure_QualificationNotExists()
    {
        _db.Setup(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()))
            .ReturnsAsync(false);
        _db.Setup(x => x.EmployeeQualification.Delete(It.IsAny<int>()))
            .ReturnsAsync(EmployeeQualificationTestData.EmployeeQualification);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
            _employeeQualificationService.DeleteEmployeeQualification(QualificationId));

        Assert.Equivalent("Employee Does Not Have Existing Qualifications Saved", exception.Message);

        _db.Verify(x => x.EmployeeQualification.Any(It.IsAny<Expression<Func<EmployeeQualification, bool>>>()),
            Times.Once);
        _db.Verify(x => x.EmployeeQualification.Delete(It.IsAny<int>()), Times.Never);
    }

    #endregion
}