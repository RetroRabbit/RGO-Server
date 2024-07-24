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

    public EmployeeTypesServiceUnitTest()
    {
        _identity = new Mock<AuthorizeIdentityMock>();
        _dbMock = new Mock<IUnitOfWork>();
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        _employeeTypeService = new EmployeeTypeService(_dbMock.Object, _identity.Object);
    }

    [Fact]
    public async Task CreateEmployeeTypeTestSuccessSave()
    {
        var employeeType = EmployeeTypeTestData.DeveloperType;
        var employeeTypeDto = EmployeeTypeTestData.DeveloperType.ToDto();

        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.Setup(x => x.EmployeeId).Returns(1);
        _employeeTypeServiceMock.Setup(x => x.EmployeeTypeExists(employeeTypeDto.Id)).ReturnsAsync(true);

        _employeeTypeServiceMock.Setup(x => x.CreateEmployeeType(employeeTypeDto)).ReturnsAsync(employeeTypeDto);

        _employeeTypeServiceMock.Setup(x => x.GetEmployeeType(employeeType.Name!)).ReturnsAsync(employeeTypeDto);

        _dbMock.Setup(r => r.EmployeeType.Add(It.IsAny<EmployeeType>())).ReturnsAsync(employeeType);
        _dbMock.Setup(x => x.EmployeeType.Add(employeeType)).ReturnsAsync(employeeType);

        var result = await _employeeTypeService.CreateEmployeeType(employeeTypeDto);

        Assert.NotNull(result);
        Assert.Equal(employeeTypeDto.Id, result.Id);
        Assert.Equal(employeeTypeDto.Name, result.Name);
    }

    [Fact]
    public async Task CreateEmployeeTypeDoesNotExists()
    {
        var employeeType = EmployeeTypeTestData.DeveloperType;
        var employeeTypeDto = EmployeeTypeTestData.DeveloperType.ToDto();

        _dbMock.Setup(x => x.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(true);

        _dbMock.Setup(r => r.EmployeeType.Add(It.IsAny<EmployeeType>())).ReturnsAsync(employeeType);

        await Assert.ThrowsAsync<CustomException>(() => _employeeTypeService.CreateEmployeeType(employeeTypeDto));

        _dbMock.Verify(x => x.EmployeeType.Add(It.IsAny<EmployeeType>()), Times.Never);
        _dbMock.Verify(x => x.EmployeeType.FirstOrDefault(It.IsAny<Expression<Func<EmployeeType, bool>>>()), Times.Never);
    }

    [Fact]
    public async Task CreateEmployeeTypeTestUnauthorized()
    {
        var employeeType = EmployeeTypeTestData.DeveloperType;
        var employeeTypeDto = EmployeeTypeTestData.DeveloperType.ToDto();

        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.Setup(x => x.EmployeeId).Returns(5);
        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(false);

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(It.IsAny<string>())).Throws(new Exception());
        _employeeTypeServiceMock.Setup(x => x.CreateEmployeeType(employeeTypeDto)).ReturnsAsync(employeeTypeDto);
        _employeeTypeServiceMock.Setup(x => x.GetEmployeeType(employeeType.Name!)).ReturnsAsync(employeeTypeDto);

        _dbMock.Setup(r => r.EmployeeType.Add(It.IsAny<EmployeeType>())).ReturnsAsync(employeeType);
        _dbMock.Setup(x => x.EmployeeType.Add(employeeType)).ReturnsAsync(employeeType);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
            _employeeTypeService.CreateEmployeeType(EmployeeTypeTestData.DeveloperType.ToDto()));

        Assert.Equivalent("Unauthorized Access.", exception.Message);
    }

    [Fact]
    public async Task GetAllEmployeeTypesTestSuccess()
    {
        var employeeTypes = new List<EmployeeType>
        {
           new() { Id = 1, Name = "CAM" },
           new() { Id = 2, Name = "Designer" },
           new() { Id = 3, Name = "Developer" }
        };

        _dbMock.Setup(r => r.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(employeeTypes);

        var result = await _employeeTypeService.GetEmployeeTypes();

        Assert.NotNull(result);
        Assert.IsType<List<EmployeeTypeDto>>(result);
        Assert.True(result.SequenceEqual(result.OrderBy(e => e.Name)));
        _dbMock.Verify(u => u.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetAllEmployeeTypesTestFail()
    {
        var employeeTypesDtos = new List<EmployeeType>
        {
           new() { Id = 1, Name = "CAM" },
           new() { Id = 2, Name = "Designer" },
           new() { Id = 3, Name = "Developer" }
        };

        _employeeTypeServiceMock.Setup(r => r.GetAllEmployeeType()).Throws(new Exception());

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(false);
        _dbMock.Setup(r => r.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(employeeTypesDtos);

        var result = await _employeeTypeService.GetEmployeeTypes();

        Assert.NotNull(result);
        Assert.IsType<List<EmployeeTypeDto>>(result);
        Assert.True(result.SequenceEqual(result.OrderBy(e => e.Name)));
        _dbMock.Verify(u => u.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task DeleteEmployeeTypeTestSuccess()
    {
        var employeeType = EmployeeTypeTestData.DeveloperType;
        var employeeTypeDto = EmployeeTypeTestData.DeveloperType.ToDto();

        var employeeTypeList = new List<EmployeeType>
        {
            employeeType
        };

        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.Setup(x => x.EmployeeId).Returns(1);

        _dbMock.Setup(x => x.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(true);
        _dbMock.Setup(x => x.EmployeeType.Delete(employeeTypeList[0].Id)).ReturnsAsync(employeeType);

        var result = await _employeeTypeService.DeleteEmployeeType(employeeTypeList[0].Id);

        Assert.NotNull(result);
        Assert.Equivalent(employeeTypeDto, result);
    }

    [Fact]
    public async Task DeleteEmployeeTypeDoesNotExists()
    {
        var employeeType = EmployeeTypeTestData.DeveloperType;
        var employeeTypeDto = EmployeeTypeTestData.DesignerType.ToDto();
        var employeeTypeList = new List<EmployeeType>
        {
            employeeType
        };

        _dbMock.Setup(x => x.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(false);

        _dbMock.Setup(r => r.EmployeeType.Delete(employeeTypeList[0].Id)).ReturnsAsync(employeeType);

        await Assert.ThrowsAsync<CustomException>(() => _employeeTypeService.DeleteEmployeeType(employeeTypeDto.Id));

        _dbMock.Verify(x => x.EmployeeType.Delete(employeeTypeList[0].Id), Times.Never);
        _dbMock.Verify(x => x.EmployeeType.FirstOrDefault(It.IsAny<Expression<Func<EmployeeType, bool>>>()), Times.Never);
    }

    [Fact]
    public async Task DeleteEmployeeTypeUnauthorized()
    {
        var employeeType = EmployeeTypeTestData.DeveloperType;
        var employeeTypeDto = EmployeeTypeTestData.DeveloperType.ToDto();

        var employeeTypeList = new List<EmployeeType>
        {
            employeeType
        };

        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.Setup(x => x.EmployeeId).Returns(5);

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name!))
                                .ReturnsAsync(employeeTypeDto);

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(true);
        _dbMock.Setup(x => x.EmployeeType.Delete(employeeTypeList[0].Id)).ReturnsAsync(employeeType);

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
            _employeeTypeService.DeleteEmployeeType(employeeTypeList[0].Id!));

        Assert.Equivalent("Unauthorized Access.", exception.Message);
    }

    [Fact]
    public async Task GetEmployeeTypeTestSuccess()
    {
        var employeeTypeDto = new EmployeeTypeDto { Id = 1, Name = "Developer" };

        var employeeTypeList = new List<EmployeeType>
        {
            new(employeeTypeDto)
        };

        _dbMock.Setup(e => e.EmployeeType.Get(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .Returns(employeeTypeList.ToMockIQueryable());

        var result = await _employeeTypeService.GetEmployeeType(employeeTypeDto.Name);

        Assert.NotNull(result);
        Assert.Equivalent(employeeTypeDto, result);
        Assert.Equal(employeeTypeDto.Id, result.Id);
        Assert.Equal(employeeTypeDto.Name, result.Name);
    }

    [Fact]
    public async Task GetEmployeeTypeTestFail()
    {
        var employeeTypeDto = new EmployeeTypeDto { Id = 1, Name = "Developer" };

        var employeeTypeList = new List<EmployeeType>
        {
            new(employeeTypeDto)
        };

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeList[0].Name!)).Throws(new Exception());

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(false);
        _dbMock.Setup(e => e.EmployeeType.Get(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .Returns(employeeTypeList.ToMockIQueryable());

        var result = await _employeeTypeService.GetEmployeeType(employeeTypeList[0].Name!);

        Assert.NotNull(result);
        Assert.Equal(employeeTypeDto.Id, result.Id);
        Assert.Equal(employeeTypeDto.Name, result.Name);
    }

    [Fact]
    public async Task GetAllEmployeeTypesTestSuccess2()
    {
        var employeeTypes = new List<EmployeeType>
        {
           new() { Id = 1, Name = "CAM" },
           new() { Id = 2, Name = "Designer" },
           new() { Id = 3, Name = "Developer" }
        };

        _dbMock.Setup(r => r.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(employeeTypes);

        var result = await _employeeTypeService.GetAllEmployeeType();

        Assert.NotNull(result);
        Assert.IsType<List<EmployeeTypeDto>>(result);
        Assert.True(result.SequenceEqual(result.OrderBy(e => e.Name)));
        _dbMock.Verify(u => u.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetAllEmployeeTypesTestFail2()
    {
        var employeeTypes = new List<EmployeeType>
        {
           new() { Id = 1, Name = "CAM" },
           new() { Id = 2, Name = "Designer" },
           new() { Id = 3, Name = "Developer" }
        };

        _employeeTypeServiceMock.Setup(r => r.GetAllEmployeeType()).Throws(new Exception());

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(false);
        _dbMock.Setup(r => r.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(employeeTypes);

        var result = await _employeeTypeService.GetAllEmployeeType();

        Assert.NotNull(result);
        Assert.IsType<List<EmployeeTypeDto>>(result);
        Assert.True(result.SequenceEqual(result.OrderBy(e => e.Name)));
        _dbMock.Verify(u => u.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task UpdateEmployeeTypeTestSuccess()
    {
        var employeeType = EmployeeTypeTestData.DeveloperType;
        var employeeTypeDto = EmployeeTypeTestData.DeveloperType.ToDto();

        var employeeTypeList = new List<EmployeeType>
        {
            new(employeeTypeDto)
        };

        _identity.Setup(i => i.Role).Returns("Admin");
        _identity.Setup(x => x.EmployeeId).Returns(1);

        _dbMock.Setup(x => x.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(true);
        _dbMock.Setup(e => e.EmployeeType.Get(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .Returns(employeeTypeList.ToMockIQueryable());
        _dbMock.Setup(r => r.EmployeeType.Update(It.IsAny<EmployeeType>())).ReturnsAsync(employeeType);

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name!))
                                .ReturnsAsync(employeeTypeDto);

        var result = await _employeeTypeService.UpdateEmployeeType(employeeTypeDto);

        Assert.NotNull(result);
        Assert.Equivalent(employeeTypeDto, result);
    }

    [Fact]
    public async Task UpdateEmployeeTypeDoesNotExists()
    {
        var employeeType = EmployeeTypeTestData.DeveloperType;
        var employeeTypeDto = EmployeeTypeTestData.DeveloperType.ToDto();

        _dbMock.Setup(x => x.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(false);

        _dbMock.Setup(r => r.EmployeeType.Update(It.IsAny<EmployeeType>())).ReturnsAsync(employeeType);

        await Assert.ThrowsAsync<CustomException>(() => _employeeTypeService.UpdateEmployeeType(employeeTypeDto));

        _dbMock.Verify(x => x.EmployeeType.Update(It.IsAny<EmployeeType>()), Times.Never);
        _dbMock.Verify(x => x.EmployeeType.FirstOrDefault(It.IsAny<Expression<Func<EmployeeType, bool>>>()), Times.Never);
    }

    [Fact]
    public async Task UpdateEmployeeTypeTestUnauthorized()
    {
        var employeeType = EmployeeTypeTestData.DeveloperType;
        var employeeTypeDto = EmployeeTypeTestData.DeveloperType.ToDto();

        _identity.Setup(i => i.Role).Returns("Employee");
        _identity.Setup(x => x.EmployeeId).Returns(5);

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(true);
        _dbMock.Setup(r => r.EmployeeType.Update(It.IsAny<EmployeeType>())).ReturnsAsync(employeeType);

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name!))
                              .ReturnsAsync(employeeTypeDto);

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