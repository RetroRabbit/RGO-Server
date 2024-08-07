using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Repositories.HRIS;
using System.Linq.Expressions;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class FieldCodeOptionsServiceUnitTests
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly FieldCodeOptions _fieldCodeOptions;
    private readonly FieldCodeOptions _fieldCodeOptions2;
    private readonly FieldCodeOptionsService _fieldCodeOptionsService;
    private readonly Mock<AuthorizeIdentityMock> _identity;

    public FieldCodeOptionsServiceUnitTests()
    {
        _identity = new Mock<AuthorizeIdentityMock>();
        _dbMock = new Mock<IUnitOfWork>();
        _fieldCodeOptionsService = new FieldCodeOptionsService(_dbMock.Object, _identity.Object);
        _fieldCodeOptions = new FieldCodeOptions { Id = 1, FieldCodeId = 1, Option = "string" };
        _fieldCodeOptions2 = new FieldCodeOptions { Id = 0, FieldCodeId = 1, Option = "string2" };
    }

    [Fact]
    public async Task GetAllFieldCodeOptionsTest()
    {
        var fields = _fieldCodeOptions.EntityToList();
        _dbMock.Setup(x => x.FieldCodeOptions.GetAll(null)).ReturnsAsync(fields);
        var result = await _fieldCodeOptionsService.GetAllFieldCodeOptions();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equivalent(fields.Select(x => x.ToDto()).ToList(), result);
    }

    [Fact]
    public async Task GetFieldCodeOptionsById_Success()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.Setup(x => x.EmployeeId).Returns(1);

        _dbMock.Setup(x => x.FieldCodeOptions.Any(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()))
            .ReturnsAsync(true);

        var fields = _fieldCodeOptions.EntityToList();
        _dbMock.Setup(x => x.FieldCodeOptions.GetAll(null)).ReturnsAsync(fields);

        var result = await _fieldCodeOptionsService.GetFieldCodeOptionsById(_fieldCodeOptions.Id);
        Assert.NotNull(result);
        Assert.Equivalent(fields.Select(x => x.ToDto()).ToList(), result);
        _dbMock.Verify(x => x.FieldCodeOptions.GetAll(null), Times.Once);
    }

    [Fact]
    public async Task GetFieldCodeOptionsById_Unauthorized()
    {
        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.Setup(x => x.EmployeeId).Returns(2);

        _dbMock.Setup(x => x.FieldCodeOptions.Any(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()))
            .ReturnsAsync(true);

        var fields = _fieldCodeOptions.EntityToList();
        _dbMock.Setup(x => x.FieldCodeOptions.GetAll(null)).ReturnsAsync(fields);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
        _fieldCodeOptionsService.GetFieldCodeOptionsById(_fieldCodeOptions.Id));

        Assert.Equivalent("Unauthorized Access.", exception.Message);
    }

    [Fact]
    public async Task GetFieldCodeOptionsById_DoesNotExist()
    {
        _dbMock.Setup(x => x.FieldCodeOptions.Any(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()))
        .ReturnsAsync(false);

        var fields = _fieldCodeOptions.EntityToList();
        _dbMock.Setup(x => x.FieldCodeOptions.GetAll(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>())).ReturnsAsync(fields);

        await Assert.ThrowsAsync<CustomException>(() => _fieldCodeOptionsService.GetFieldCodeOptionsById(_fieldCodeOptions.Id));

        _dbMock.Verify(x => x.FieldCodeOptions.Get(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()), Times.Never);
        _dbMock.Verify(x => x.FieldCodeOptions.FirstOrDefault(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()), Times.Never);
    }

    [Fact]
    public async Task CreateFieldCodeOptions_Success()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.Setup(x => x.EmployeeId).Returns(1);

        _dbMock.Setup(x => x.FieldCodeOptions.Any(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()))
            .ReturnsAsync(false);

        var fields = _fieldCodeOptions.EntityToList();
        _dbMock.Setup(x => x.FieldCodeOptions.GetAll(null)).ReturnsAsync(fields);

        _dbMock.Setup(x => x.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>()))
               .ReturnsAsync(_fieldCodeOptions2);

        var result = await _fieldCodeOptionsService.CreateFieldCodeOptions(_fieldCodeOptions2.ToDto());
        Assert.NotNull(result);
        Assert.Equivalent(_fieldCodeOptions2.ToDto(), result);
        _dbMock.Verify(x => x.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>()), Times.Once);
    }

    [Fact]
    public async Task CreateFieldCodeOptions_Unauthorized()
    {
        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.Setup(x => x.EmployeeId).Returns(2);

        _dbMock.Setup(x => x.FieldCodeOptions.Any(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()))
            .ReturnsAsync(false);

        var fields = _fieldCodeOptions.EntityToList();
        _dbMock.Setup(x => x.FieldCodeOptions.GetAll(null)).ReturnsAsync(fields);

        _dbMock.Setup(r => r.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>()))
               .ReturnsAsync(_fieldCodeOptions2);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
      _fieldCodeOptionsService.CreateFieldCodeOptions(_fieldCodeOptions2.ToDto()));

        Assert.Equivalent("Unauthorized Access.", exception.Message);
    }

    [Fact]
    public async Task CreateFieldCodeOptions_DoesNotExist()
    {
        _dbMock.Setup(x => x.FieldCodeOptions.Any(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()))
        .ReturnsAsync(true);
        _dbMock.Setup(r => r.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>()))
                .ReturnsAsync(_fieldCodeOptions2);

        await Assert.ThrowsAsync<CustomException>(() => _fieldCodeOptionsService.CreateFieldCodeOptions(_fieldCodeOptions2.ToDto()));

        _dbMock.Verify(x => x.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>()), Times.Never); ;
        _dbMock.Verify(x => x.FieldCodeOptions.FirstOrDefault(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()), Times.Never);
    }

    [Fact]
    public async Task UpdateFieldCodeOptions_Success()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.Setup(x => x.EmployeeId).Returns(1);

        _dbMock.Setup(x => x.FieldCodeOptions.Any(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()))
            .ReturnsAsync(true);

        var fieldList = _fieldCodeOptions.EntityToList(_fieldCodeOptions2);
        var field = _fieldCodeOptions.EntityToList();
        var field2 = _fieldCodeOptions2.EntityToList();

        _dbMock.SetupSequence(x => x.FieldCodeOptions.GetAll(null))
               .ReturnsAsync(field)
               .ReturnsAsync(fieldList)
               .ReturnsAsync(field2)
               .ReturnsAsync(field2);


        _dbMock.Setup(x => x.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>()))
               .ReturnsAsync(_fieldCodeOptions2);
        _dbMock.Setup(x => x.FieldCodeOptions.Delete(It.IsAny<int>()))
               .ReturnsAsync(_fieldCodeOptions);

        await _fieldCodeOptionsService.UpdateFieldCodeOptions(field2.Select(x => x.ToDto()).ToList());

        _dbMock.Verify(x => x.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>()), Times.Once);
        _dbMock.Verify(x => x.FieldCodeOptions.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task UpdateFieldCodeOptions_Unauthorized()
    {
        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.Setup(x => x.EmployeeId).Returns(2);

        _dbMock.Setup(x => x.FieldCodeOptions.Any(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()))
            .ReturnsAsync(true);

        var fieldList = _fieldCodeOptions.EntityToList(_fieldCodeOptions2);
        var field = _fieldCodeOptions.EntityToList();
        var field2 = _fieldCodeOptions2.EntityToList();

        _dbMock.SetupSequence(x => x.FieldCodeOptions.GetAll(null))
               .ReturnsAsync(field)
               .ReturnsAsync(fieldList)
               .ReturnsAsync(field2)
               .ReturnsAsync(field2);


        _dbMock.Setup(x => x.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>()))
               .ReturnsAsync(_fieldCodeOptions2);
        _dbMock.Setup(x => x.FieldCodeOptions.Delete(It.IsAny<int>()))
               .ReturnsAsync(_fieldCodeOptions);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
     _fieldCodeOptionsService.UpdateFieldCodeOptions(field2.Select(x => x.ToDto()).ToList()));

        Assert.Equivalent("Unauthorized Access.", exception.Message);
    }

    [Fact]
    public async Task UpdateFieldCodeOptions_DoesNotExist()
    {
        _dbMock.Setup(x => x.FieldCodeOptions.Any(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()))
        .ReturnsAsync(false);

        var fieldList = _fieldCodeOptions.EntityToList(_fieldCodeOptions2);
        var field = _fieldCodeOptions.EntityToList();
        var field2 = _fieldCodeOptions2.EntityToList();

        _dbMock.SetupSequence(x => x.FieldCodeOptions.GetAll(null))
               .ReturnsAsync(field)
               .ReturnsAsync(fieldList)
               .ReturnsAsync(field2)
               .ReturnsAsync(field2);


        _dbMock.Setup(x => x.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>()))
               .ReturnsAsync(_fieldCodeOptions2);
        _dbMock.Setup(x => x.FieldCodeOptions.Delete(It.IsAny<int>()))
               .ReturnsAsync(_fieldCodeOptions);

        await Assert.ThrowsAsync<CustomException>(() => _fieldCodeOptionsService.UpdateFieldCodeOptions(field2.Select(x => x.ToDto()).ToList()));

        _dbMock.Verify(x => x.FieldCodeOptions.Update(It.IsAny<FieldCodeOptions>()), Times.Never); ;
        _dbMock.Verify(x => x.FieldCodeOptions.FirstOrDefault(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()), Times.Never);
    }

    [Fact]
    public async Task DeleteFieldCode_Success()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.Setup(x => x.EmployeeId).Returns(1);

        _dbMock.Setup(x => x.FieldCodeOptions.Any(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()))
            .ReturnsAsync(true);

        var fields = new List<FieldCodeOptions> { _fieldCodeOptions };
        _dbMock.Setup(x => x.FieldCodeOptions.GetAll(null)).ReturnsAsync(fields);

        _dbMock.Setup(x => x.FieldCodeOptions.Delete(It.IsAny<int>()))
               .ReturnsAsync(_fieldCodeOptions);

        var result = await _fieldCodeOptionsService.DeleteFieldCodeOptions(_fieldCodeOptions.ToDto());
        Assert.NotNull(result);
        Assert.Equivalent(_fieldCodeOptions.ToDto(), result);
        _dbMock.Verify(r => r.FieldCodeOptions.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteFieldCode_Unauthorized()
    {
        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.Setup(x => x.EmployeeId).Returns(2);

        _dbMock.Setup(x => x.FieldCodeOptions.Any(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()))
            .ReturnsAsync(true);

        var fields = new List<FieldCodeOptions> { _fieldCodeOptions };
        _dbMock.Setup(x => x.FieldCodeOptions.GetAll(null)).ReturnsAsync(fields);

        _dbMock.Setup(x => x.FieldCodeOptions.Delete(It.IsAny<int>()))
               .ReturnsAsync(_fieldCodeOptions);

        var exception = await Assert.ThrowsAnyAsync<CustomException>(() => _fieldCodeOptionsService
              .DeleteFieldCodeOptions(_fieldCodeOptions.ToDto()));

        Assert.Equivalent("Unauthorized Access.", exception.Message);
    }

    [Fact]
    public async Task DeleteFieldCode_DoesNotExist()
    {
        _dbMock.Setup(x => x.FieldCodeOptions.Any(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()))
        .ReturnsAsync(false);

        _dbMock.Setup(x => x.FieldCodeOptions.Delete(It.IsAny<int>()))
                .ReturnsAsync(_fieldCodeOptions);

        await Assert.ThrowsAsync<CustomException>(() => _fieldCodeOptionsService.DeleteFieldCodeOptions(_fieldCodeOptions.ToDto()));

        _dbMock.Verify(x => x.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>()), Times.Never); ;
        _dbMock.Verify(x => x.FieldCodeOptions.FirstOrDefault(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()), Times.Never);
    }

    [Fact]
    public async Task FieldCodeOptionsExistsReturnsTrue()
    {
        var testId = 1;
        _dbMock.Setup(r => r.FieldCodeOptions.Any(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()))
        .ReturnsAsync(true);

        var result = await _fieldCodeOptionsService.FieldCodeOptionsExists(testId);

        Assert.True(result);
        _dbMock.Verify(x => x.FieldCodeOptions.Any(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task FieldCodeOptionsExistsReturnsFalse()
    {
        var testId = 1;
        _dbMock.Setup(x => x.FieldCodeOptions.Any(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()))
        .ReturnsAsync(false);

        var result = await _fieldCodeOptionsService.FieldCodeOptionsExists(testId);

        Assert.False(result);
        _dbMock.Verify(x => x.FieldCodeOptions.Any(It.IsAny<Expression<Func<FieldCodeOptions, bool>>>()), Times.Once);
    }
}