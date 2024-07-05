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
    private readonly EmployeeTypeService employeeTypeService;

    public EmployeeTypesServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        employeeTypeService = new EmployeeTypeService(_dbMock.Object);
    }

    [Fact]
    public async Task SaveEmployeeTypeTestFail()
    {
        var employeeType = EmployeeTypeTestData.DeveloperType;
        var employeeTypeDto = EmployeeTypeTestData.DeveloperType.ToDto();

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(false);

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(It.IsAny<string>())).Throws(new Exception());
        _employeeTypeServiceMock.Setup(x => x.SaveEmployeeType(employeeTypeDto)).ReturnsAsync(employeeTypeDto);
        _employeeTypeServiceMock.Setup(x => x.GetEmployeeType(employeeType.Name!)).ReturnsAsync(employeeTypeDto);

        _dbMock.Setup(r => r.EmployeeType.Add(It.IsAny<EmployeeType>())).ReturnsAsync(employeeType);
        _dbMock.Setup(x => x.EmployeeType.Add(employeeType)).ReturnsAsync(employeeType);

        var result = await employeeTypeService.SaveEmployeeType(employeeTypeDto);

        Assert.NotNull(result);
        Assert.Equivalent(employeeTypeDto, result);
        Assert.Equal(employeeTypeDto.Id, result.Id);
        Assert.Equal(employeeTypeDto.Name, result.Name);
    }

    [Fact]
    public async Task SaveEmployeeTypeTestSuccessSave()
    {
        var employeeType = EmployeeTypeTestData.DeveloperType;
        var employeeTypeDto = EmployeeTypeTestData.DeveloperType.ToDto();

        _employeeTypeServiceMock.Setup(x => x.SaveEmployeeType(employeeTypeDto)).ReturnsAsync(employeeTypeDto);

        _employeeTypeServiceMock.Setup(x => x.GetEmployeeType(employeeType.Name!)).ReturnsAsync(employeeTypeDto);

        _dbMock.Setup(r => r.EmployeeType.Add(It.IsAny<EmployeeType>())).ReturnsAsync(employeeType);
        _dbMock.Setup(x => x.EmployeeType.Add(employeeType)).ReturnsAsync(employeeType);

        var result = await employeeTypeService.SaveEmployeeType(employeeTypeDto);

        Assert.NotNull(result);
        Assert.Equal(employeeTypeDto.Id, result.Id);
        Assert.Equal(employeeTypeDto.Name, result.Name);
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

        var result = await employeeTypeService.GetEmployeeTypes();

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

        var result = await employeeTypeService.GetEmployeeTypes();

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

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name!))
                                .ReturnsAsync(employeeTypeDto);

        _dbMock.Setup(e => e.EmployeeType.Get(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .Returns(employeeTypeList.ToMockIQueryable());
        _dbMock.Setup(x => x.EmployeeType.Delete(employeeTypeList[0].Id)).ReturnsAsync(employeeType);

        var result = await employeeTypeService.DeleteEmployeeType(employeeTypeList[0].Name!);

        Assert.NotNull(result);
        Assert.Equivalent(employeeTypeDto, result);
    }

    [Fact]
    public async Task DeleteEmployeeTypeTestFail()
    {
        var employeeType = EmployeeTypeTestData.DeveloperType;
        var employeeTypeDto = EmployeeTypeTestData.DeveloperType.ToDto();

        var employeeTypeList = new List<EmployeeType>
        {
            employeeType
        };

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeList[0].Name!)).Throws(new Exception());
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name!))
                                .ReturnsAsync(employeeTypeDto);

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(false);
        _dbMock.Setup(e => e.EmployeeType.Get(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .Returns(employeeTypeList.ToMockIQueryable());
        _dbMock.Setup(x => x.EmployeeType.Delete(employeeTypeList[0].Id)).ReturnsAsync(employeeType);

        var result = await employeeTypeService.DeleteEmployeeType(employeeTypeList[0].Name!);

        Assert.NotNull(result);
        Assert.Equivalent(employeeTypeDto, result);
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

        var result = await employeeTypeService.GetEmployeeType(employeeTypeDto.Name);

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

        var result = await employeeTypeService.GetEmployeeType(employeeTypeList[0].Name!);

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

        var result = await employeeTypeService.GetAllEmployeeType();

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

        var result = await employeeTypeService.GetAllEmployeeType();

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

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name!))
                                .ReturnsAsync(employeeTypeDto);

        _dbMock.Setup(e => e.EmployeeType.Get(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .Returns(employeeTypeList.ToMockIQueryable());
        _dbMock.Setup(r => r.EmployeeType.Update(It.IsAny<EmployeeType>())).ReturnsAsync(employeeType);

        var result = await employeeTypeService.UpdateEmployeeType(employeeTypeDto);

        Assert.NotNull(result);
        Assert.Equivalent(employeeTypeDto, result);
    }

    [Fact]
    public async Task UpdateEmployeeTypeTestFail()
    {
        var employeeType = EmployeeTypeTestData.DeveloperType;
        var employeeTypeDto = EmployeeTypeTestData.DeveloperType.ToDto();

        var employeeTypeList = new List<EmployeeType>
        {
            new(employeeTypeDto)
        };

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeList[0].Name!)).Throws(new Exception());
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name!))
                                .ReturnsAsync(employeeTypeDto);

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .ReturnsAsync(false);
        _dbMock.Setup(e => e.EmployeeType.Get(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
               .Returns(employeeTypeList.ToMockIQueryable());
        _dbMock.Setup(r => r.EmployeeType.Update(It.IsAny<EmployeeType>())).ReturnsAsync(employeeType);

        var result = await employeeTypeService.UpdateEmployeeType(employeeTypeDto);

        Assert.NotNull(result);
        Assert.Equivalent(employeeTypeDto, result);
    }
}