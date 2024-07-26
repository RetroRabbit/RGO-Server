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

public class EmployeeTypesServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private readonly EmployeeTypeService _employeeTypeService;
    private readonly Mock<AuthorizeIdentityMock> _identity;
    private readonly EmployeeType _employeeType;
    private readonly EmployeeTypeDto _employeeTypeDto;
    private readonly List<EmployeeType> _employeeTypeList;
    private readonly List<EmployeeType> _employeeTypes;

    public EmployeeTypesServiceUnitTest()
    {
        _identity = new Mock<AuthorizeIdentityMock>();
        _dbMock = new Mock<IUnitOfWork>();
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        _employeeTypeService = new EmployeeTypeService(_dbMock.Object, _identity.Object);

        _employeeType = EmployeeTypeTestData.DeveloperType;
        _employeeTypeDto = EmployeeTypeTestData.DeveloperType.ToDto();

        _employeeTypeList = new List<EmployeeType>
        {
            _employeeType
        };

        _employeeTypes = new List<EmployeeType>
        {
           new() { Id = 1, Name = "CAM" },
           new() { Id = 2, Name = "Designer" },
           new() { Id = 3, Name = "Developer" }
        };
    }

    [Fact]
    public async Task CreateEmployeeTypeTestSuccessSave()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.Setup(x => x.EmployeeId).Returns(1);
        _employeeTypeServiceMock.Setup(x => x.EmployeeTypeExists(_employeeTypeDto.Id)).ReturnsAsync(true);

        _employeeTypeServiceMock.Setup(x => x.CreateEmployeeType(_employeeTypeDto)).ReturnsAsync(_employeeTypeDto);

        _employeeTypeServiceMock.Setup(x => x.GetEmployeeTypeByName(_employeeType.Name!)).ReturnsAsync(_employeeTypeDto);

        _dbMock.Setup(r => r.EmployeeType.Add(It.IsAny<EmployeeType>())).ReturnsAsync(_employeeType);
        _dbMock.Setup(x => x.EmployeeType.Add(_employeeType)).ReturnsAsync(_employeeType);

        var result = await _employeeTypeService.CreateEmployeeType(_employeeTypeDto);

        Assert.NotNull(result);
        Assert.Equal(_employeeTypeDto.Id, result.Id);
        Assert.Equal(_employeeTypeDto.Name, result.Name);
    }

    [Fact]
    public async Task CreateEmployeeTypeDoesNotExists()
    {
        _dbMock.Setup(x => x.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(true);

        _dbMock.Setup(r => r.EmployeeType.Add(It.IsAny<EmployeeType>())).ReturnsAsync(_employeeType);

        await Assert.ThrowsAsync<CustomException>(() => _employeeTypeService.CreateEmployeeType(_employeeTypeDto));

        _dbMock.Verify(x => x.EmployeeType.Add(It.IsAny<EmployeeType>()), Times.Never);
        _dbMock.Verify(x => x.EmployeeType.FirstOrDefault(It.IsAny<Expression<Func<EmployeeType, bool>>>()), Times.Never);
    }

    [Fact]
    public async Task CreateEmployeeTypeTestUnauthorized()
    {
        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.Setup(x => x.EmployeeId).Returns(5);
        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(false);

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeTypeByName(It.IsAny<string>())).Throws(new Exception());
        _employeeTypeServiceMock.Setup(x => x.CreateEmployeeType(_employeeTypeDto)).ReturnsAsync(_employeeTypeDto);
        _employeeTypeServiceMock.Setup(x => x.GetEmployeeTypeByName(_employeeType.Name!)).ReturnsAsync(_employeeTypeDto);

        _dbMock.Setup(r => r.EmployeeType.Add(It.IsAny<EmployeeType>())).ReturnsAsync(_employeeType);
        _dbMock.Setup(x => x.EmployeeType.Add(_employeeType)).ReturnsAsync(_employeeType);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
            _employeeTypeService.CreateEmployeeType(EmployeeTypeTestData.DeveloperType.ToDto()));

        Assert.Equivalent("Unauthorized Access.", exception.Message);
    }

    [Fact]
    public async Task GetAllEmployeeTypesTestSuccess()
    {
        _dbMock.Setup(r => r.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(_employeeTypes);

        var result = await _employeeTypeService.GetEmployeeTypes();

        Assert.NotNull(result);
        Assert.IsType<List<EmployeeTypeDto>>(result);
        Assert.True(result.SequenceEqual(result.OrderBy(e => e.Name)));
        _dbMock.Verify(u => u.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetAllEmployeeTypesTestFail()
    {
        _employeeTypeServiceMock.Setup(r => r.GetAllEmployeeType()).Throws(new Exception());

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(false);
        _dbMock.Setup(r => r.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(_employeeTypes);

        var result = await _employeeTypeService.GetEmployeeTypes();

        Assert.NotNull(result);
        Assert.IsType<List<EmployeeTypeDto>>(result);
        Assert.True(result.SequenceEqual(result.OrderBy(e => e.Name)));
        _dbMock.Verify(u => u.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task DeleteEmployeeTypeTestSuccess()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.Setup(x => x.EmployeeId).Returns(1);

        _dbMock.Setup(x => x.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(true);
        _dbMock.Setup(x => x.EmployeeType.Delete(_employeeTypeList[0].Id)).ReturnsAsync(_employeeType);

        var result = await _employeeTypeService.DeleteEmployeeType(_employeeTypeList[0].Id);

        Assert.NotNull(result);
        Assert.Equivalent(_employeeTypeDto, result);
    }

    [Fact]
    public async Task DeleteEmployeeTypeDoesNotExists()
    {
        _dbMock.Setup(x => x.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(false);

        _dbMock.Setup(r => r.EmployeeType.Delete(_employeeTypeList[0].Id)).ReturnsAsync(_employeeType);

        await Assert.ThrowsAsync<CustomException>(() => _employeeTypeService.DeleteEmployeeType(_employeeTypeDto.Id));

        _dbMock.Verify(x => x.EmployeeType.Delete(_employeeTypeList[0].Id), Times.Never);
        _dbMock.Verify(x => x.EmployeeType.FirstOrDefault(It.IsAny<Expression<Func<EmployeeType, bool>>>()), Times.Never);
    }

    [Fact]
    public async Task DeleteEmployeeTypeUnauthorized()
    {
        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.Setup(x => x.EmployeeId).Returns(5);

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeTypeByName(_employeeType.Name!))
                                .ReturnsAsync(_employeeTypeDto);

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(true);
        _dbMock.Setup(x => x.EmployeeType.Delete(_employeeTypeList[0].Id)).ReturnsAsync(_employeeType);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
            _employeeTypeService.DeleteEmployeeType(_employeeTypeList[0].Id!));

        Assert.Equivalent("Unauthorized Access.", exception.Message);
    }

    [Fact]
    public async Task GetEmployeeTypeTestSuccess()
    {
        _dbMock.Setup(e => e.EmployeeType.Get(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .Returns(_employeeTypeList.ToMockIQueryable());

        var result = await _employeeTypeService.GetEmployeeTypeByName(_employeeTypeDto.Name);

        Assert.NotNull(result);
        Assert.Equivalent(_employeeTypeDto, result);
        Assert.Equal(_employeeTypeDto.Id, result.Id);
        Assert.Equal(_employeeTypeDto.Name, result.Name);
    }

    [Fact]
    public async Task GetEmployeeTypeTestFail()
    {
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeTypeByName(_employeeTypeList[0].Name!)).Throws(new Exception());

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(false);
        _dbMock.Setup(e => e.EmployeeType.Get(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .Returns(_employeeTypeList.ToMockIQueryable());

        var result = await _employeeTypeService.GetEmployeeTypeByName(_employeeTypeList[0].Name!);

        Assert.NotNull(result);
        Assert.Equal(_employeeTypeDto.Id, result.Id);
        Assert.Equal(_employeeTypeDto.Name, result.Name);
    }

    [Fact]
    public async Task GetAllEmployeeTypesTestSuccess2()
    {
        _dbMock.Setup(r => r.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(_employeeTypes);

        var result = await _employeeTypeService.GetAllEmployeeType();

        Assert.NotNull(result);
        Assert.IsType<List<EmployeeTypeDto>>(result);
        Assert.True(result.SequenceEqual(result.OrderBy(e => e.Name)));
        _dbMock.Verify(u => u.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetAllEmployeeTypesTestFail2()
    {
        _employeeTypeServiceMock.Setup(r => r.GetAllEmployeeType()).Throws(new Exception());

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(false);
        _dbMock.Setup(r => r.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(_employeeTypes);

        var result = await _employeeTypeService.GetAllEmployeeType();

        Assert.NotNull(result);
        Assert.IsType<List<EmployeeTypeDto>>(result);
        Assert.True(result.SequenceEqual(result.OrderBy(e => e.Name)));
        _dbMock.Verify(u => u.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task UpdateEmployeeTypeTestSuccess()
    {
        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.Setup(x => x.EmployeeId).Returns(1);

        _dbMock.Setup(x => x.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(true);
        _dbMock.Setup(e => e.EmployeeType.Get(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .Returns(_employeeTypeList.ToMockIQueryable());
        _dbMock.Setup(r => r.EmployeeType.Update(It.IsAny<EmployeeType>())).ReturnsAsync(_employeeType);

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeTypeByName(_employeeType.Name!))
                                .ReturnsAsync(_employeeTypeDto);

        var result = await _employeeTypeService.UpdateEmployeeType(_employeeTypeDto);

        Assert.NotNull(result);
        Assert.Equivalent(_employeeTypeDto, result);
    }

    [Fact]
    public async Task UpdateEmployeeTypeDoesNotExists()
    {
        _dbMock.Setup(x => x.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(false);

        _dbMock.Setup(r => r.EmployeeType.Update(It.IsAny<EmployeeType>())).ReturnsAsync(_employeeType);

        await Assert.ThrowsAsync<CustomException>(() => _employeeTypeService.UpdateEmployeeType(_employeeTypeDto));

        _dbMock.Verify(x => x.EmployeeType.Update(It.IsAny<EmployeeType>()), Times.Never);
        _dbMock.Verify(x => x.EmployeeType.FirstOrDefault(It.IsAny<Expression<Func<EmployeeType, bool>>>()), Times.Never);
    }

    [Fact]
    public async Task UpdateEmployeeTypeTestUnauthorized()
    {
        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.Setup(x => x.EmployeeId).Returns(5);

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(true);
        _dbMock.Setup(r => r.EmployeeType.Update(It.IsAny<EmployeeType>())).ReturnsAsync(_employeeType);

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeTypeByName(_employeeType.Name!))
                              .ReturnsAsync(_employeeTypeDto);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
             _employeeTypeService.UpdateEmployeeType(EmployeeTypeTestData.DeveloperType.ToDto()));

        Assert.Equivalent("Unauthorized Access.", exception.Message);
    }

    [Fact]
    public async Task CheckIfEmployeeTypeExistsReturnsTrue()
    {
        var testId = 1;
        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(true);

        var result = await _employeeTypeService.EmployeeTypeExists(testId);

        Assert.True(result);
        _dbMock.Verify(x => x.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task CheckIfModelExistsReturnsFalse()
    {
        var testId = 1;
        _dbMock.Setup(x => x.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(false);

        var result = await _employeeTypeService.EmployeeTypeExists(testId);

        Assert.False(result);
        _dbMock.Verify(x => x.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()), Times.Once);
    }
}