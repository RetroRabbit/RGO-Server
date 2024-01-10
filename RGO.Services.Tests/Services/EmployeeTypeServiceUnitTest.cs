using Moq;
using Xunit;
using RGO.Services.Services;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.Models;
using RGO.UnitOfWork.Entities;
using System.Linq.Expressions;
using MockQueryable.Moq;

namespace RGO.Services.Tests.Services;

public class EmployeeTypeServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private EmployeeTypeService employeeTypeService;

    public EmployeeTypeServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        employeeTypeService = new EmployeeTypeService(_dbMock.Object);
    }

    [Fact]
    public async Task SaveEmployeeTypeTestFail()
    {
        var employeeTypeDto = new EmployeeTypeDto(1, "Llewellyn");
        var employeeType = new EmployeeType(employeeTypeDto);

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>())).Returns(Task.FromResult(false));

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(It.IsAny<string>())).Throws(new Exception());
        _employeeTypeServiceMock.Setup(x => x.SaveEmployeeType(employeeTypeDto)).ReturnsAsync(employeeTypeDto);
        _employeeTypeServiceMock.Setup(x => x.GetEmployeeType(employeeType.Name)).ReturnsAsync(employeeTypeDto);


        _dbMock.Setup(r => r.EmployeeType.Add(It.IsAny<EmployeeType>())).Returns(Task.FromResult(employeeTypeDto));
        _dbMock.Setup(x => x.EmployeeType.Add(employeeType)).ReturnsAsync(employeeTypeDto);

        var result = await employeeTypeService.SaveEmployeeType(employeeTypeDto);

        Assert.NotNull(result);
        Assert.Equal(employeeTypeDto, result);
        Assert.Equal(employeeTypeDto.Id, result.Id);
        Assert.Equal(employeeTypeDto.Name, result.Name);
    }


    [Fact]
    public async Task SaveEmployeeTypeTestSuccessSave()
    {
        var employeeTypeDto = new EmployeeTypeDto(1, "Test");
        var employeeType = new EmployeeType(employeeTypeDto);

        _employeeTypeServiceMock.Setup(x => x.SaveEmployeeType(employeeTypeDto)).ReturnsAsync(employeeTypeDto);

        _employeeTypeServiceMock.Setup(x => x.GetEmployeeType(employeeType.Name)).ReturnsAsync(employeeTypeDto);

        _dbMock.Setup(r => r.EmployeeType.Add(It.IsAny<EmployeeType>())).Returns(Task.FromResult(employeeTypeDto));
        _dbMock.Setup(x => x.EmployeeType.Add(employeeType)).ReturnsAsync(employeeTypeDto);

        var result = await employeeTypeService.SaveEmployeeType(employeeTypeDto);

        Assert.NotNull(result);
        Assert.Equal(employeeTypeDto.Id, result.Id);
        Assert.Equal(employeeTypeDto.Name, result.Name);
    }

    [Fact]
    public async Task GetAllEmployeeTypesTestSuccess()
    {
        var employeeTypesDtos = new List<EmployeeTypeDto>
    {
        new EmployeeTypeDto(1, "Test1"),
        new EmployeeTypeDto(2, "Test2"),
        new EmployeeTypeDto(3, "Test3")
    };

        var employeeTypes = new List<EmployeeType>
    {
        new EmployeeType(employeeTypesDtos[0]),
        new EmployeeType(employeeTypesDtos[1]),
        new EmployeeType(employeeTypesDtos[2])
    };

        _dbMock.Setup(r => r.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>())).ReturnsAsync(employeeTypesDtos);

        var result = await employeeTypeService.GetEmployeeTypes();

        Assert.NotNull(result);
        Assert.IsType<List<EmployeeTypeDto>>(result);
        Assert.True(result.SequenceEqual(result.OrderBy(e => e.Name)));
        _dbMock.Verify(u => u.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetAllEmployeeTypesTestFail()
    {
        var employeeTypesDtos = new List<EmployeeTypeDto>
    {
        new EmployeeTypeDto(1, "Test1"),
        new EmployeeTypeDto(2, "Test2"),
        new EmployeeTypeDto(3, "Test3")
    };

        var employeeTypes = new List<EmployeeType>
    {
        new EmployeeType(employeeTypesDtos[0]),
        new EmployeeType(employeeTypesDtos[1]),
        new EmployeeType(employeeTypesDtos[2])
    };

        _employeeTypeServiceMock.Setup(r => r.GetAllEmployeeType()).Throws(new Exception());

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>())).Returns(Task.FromResult(false));
        _dbMock.Setup(r => r.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>())).ReturnsAsync(employeeTypesDtos);

        var result = await employeeTypeService.GetEmployeeTypes();

        Assert.NotNull(result);
        Assert.IsType<List<EmployeeTypeDto>>(result);
        Assert.True(result.SequenceEqual(result.OrderBy(e => e.Name)));
        _dbMock.Verify(u => u.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task DeleteEmployeeTypeTestSuccess()
    {
        var employeeTypeDto = new EmployeeTypeDto(1, "Llewellyn");
        var employeeType = new EmployeeType(employeeTypeDto);

        var employeeTypeList = new List<EmployeeType>
    {
        new EmployeeType(employeeTypeDto)
    };

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name)).Returns(Task.FromResult(employeeTypeDto));

        _dbMock.Setup(e => e.EmployeeType.Get(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
                 .Returns(employeeTypeList.AsQueryable().BuildMock());
        _dbMock.Setup(x => x.EmployeeType.Delete(employeeTypeList[0].Id)).Returns(Task.FromResult(employeeTypeDto));

        var result = await employeeTypeService.DeleteEmployeeType(employeeTypeList[0].Name);

        Assert.NotNull(result);
        Assert.Equal(employeeTypeDto, result);
    }

    [Fact]
    public async Task DeleteEmployeeTypeTestFail()
    {
        var employeeTypeDto = new EmployeeTypeDto(1, "Llewellyn");
        var employeeType = new EmployeeType(employeeTypeDto);

        var employeeTypeList = new List<EmployeeType>
    {
        new EmployeeType(employeeTypeDto)
    };

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeList[0].Name)).Throws(new Exception());
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name)).Returns(Task.FromResult(employeeTypeDto));

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>())).Returns(Task.FromResult(false));
        _dbMock.Setup(e => e.EmployeeType.Get(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
                 .Returns(employeeTypeList.AsQueryable().BuildMock());
        _dbMock.Setup(x => x.EmployeeType.Delete(employeeTypeList[0].Id)).Returns(Task.FromResult(employeeTypeDto));

        var result = await employeeTypeService.DeleteEmployeeType(employeeTypeList[0].Name);

        Assert.NotNull(result);
        Assert.Equal(employeeTypeDto, result);
    }

    [Fact]
    public async Task GetEmployeeTypeTestSuccess()
    {
        var employeeTypeDto = new EmployeeTypeDto(1, "Llewellyn");
        var employeeType = new EmployeeType(employeeTypeDto);

        var employeeTypeList = new List<EmployeeType>
    {
        new EmployeeType(employeeTypeDto)
    };

        _dbMock.Setup(e => e.EmployeeType.Get(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
                 .Returns(employeeTypeList.AsQueryable().BuildMock());

        var result = await employeeTypeService.GetEmployeeType(employeeTypeList[0].Name);

        Assert.NotNull(result);
        Assert.Equal(employeeTypeDto, result);
        Assert.Equal(employeeTypeDto.Id, result.Id);
        Assert.Equal(employeeTypeDto.Name, result.Name);
    }

    [Fact]
    public async Task GetEmployeeTypeTestFail()
    {
        var employeeTypeDto = new EmployeeTypeDto(1, "Llewellyn");
        var employeeType = new EmployeeType(employeeTypeDto);

        var employeeTypeList = new List<EmployeeType>
    {
        new EmployeeType(employeeTypeDto)
    };

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeList[0].Name)).Throws(new Exception());

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>())).Returns(Task.FromResult(false));
        _dbMock.Setup(e => e.EmployeeType.Get(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
                 .Returns(employeeTypeList.AsQueryable().BuildMock());

        var result = await employeeTypeService.GetEmployeeType(employeeTypeList[0].Name);

        Assert.NotNull(result);
        Assert.Equal(employeeTypeDto, result);
        Assert.Equal(employeeTypeDto.Id, result.Id);
        Assert.Equal(employeeTypeDto.Name, result.Name);
    }

    [Fact]
    public async Task GetAllEmployeeTypesTestSuccess2()
    {
        var employeeTypesDtos = new List<EmployeeTypeDto>
    {
        new EmployeeTypeDto(1, "Test1"),
        new EmployeeTypeDto(2, "Test2"),
        new EmployeeTypeDto(3, "Test3")
    };

        var employeeTypes = new List<EmployeeType>
    {
        new EmployeeType(employeeTypesDtos[0]),
        new EmployeeType(employeeTypesDtos[1]),
        new EmployeeType(employeeTypesDtos[2])
    };

        _dbMock.Setup(r => r.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>())).ReturnsAsync(employeeTypesDtos);

        var result = await employeeTypeService.GetAllEmployeeType();

        Assert.NotNull(result);
        Assert.IsType<List<EmployeeTypeDto>>(result);
        Assert.True(result.SequenceEqual(result.OrderBy(e => e.Name)));
        _dbMock.Verify(u => u.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task GetAllEmployeeTypesTestFail2()
    {
        var employeeTypesDtos = new List<EmployeeTypeDto>
    {
        new EmployeeTypeDto(1, "Test1"),
        new EmployeeTypeDto(2, "Test2"),
        new EmployeeTypeDto(3, "Test3")
    };

        var employeeTypes = new List<EmployeeType>
    {
        new EmployeeType(employeeTypesDtos[0]),
        new EmployeeType(employeeTypesDtos[1]),
        new EmployeeType(employeeTypesDtos[2])
    };

        _employeeTypeServiceMock.Setup(r => r.GetAllEmployeeType()).Throws(new Exception());

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>())).Returns(Task.FromResult(false));
        _dbMock.Setup(r => r.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>())).ReturnsAsync(employeeTypesDtos);

        var result = await employeeTypeService.GetAllEmployeeType();

        Assert.NotNull(result);
        Assert.IsType<List<EmployeeTypeDto>>(result);
        Assert.True(result.SequenceEqual(result.OrderBy(e => e.Name)));
        _dbMock.Verify(u => u.EmployeeType.GetAll(It.IsAny<Expression<Func<EmployeeType, bool>>>()), Times.Once);
    }

    [Fact]
    public async Task UpdateEmployeeTypeTestSuccess()
    {
        var employeeTypeDto = new EmployeeTypeDto(1, "Llewellyn");
        var employeeType = new EmployeeType(employeeTypeDto);

        var employeeTypeList = new List<EmployeeType>
    {
        new EmployeeType(employeeTypeDto)
    };

        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name)).Returns(Task.FromResult(employeeTypeDto));

        _dbMock.Setup(e => e.EmployeeType.Get(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
                 .Returns(employeeTypeList.AsQueryable().BuildMock());
        _dbMock.Setup(r => r.EmployeeType.Update(It.IsAny<EmployeeType>())).Returns(Task.FromResult(employeeTypeDto));

        var result = await employeeTypeService.UpdateEmployeeType(employeeTypeDto);

        Assert.NotNull(result);
        Assert.Equal(employeeTypeDto, result);
    }

    [Fact]
    public async Task UpdateEmployeeTypeTestFail()
    {
        var employeeTypeDto = new EmployeeTypeDto(1, "Llewellyn");
        var employeeType = new EmployeeType(employeeTypeDto);

        var employeeTypeList = new List<EmployeeType>
    {
        new EmployeeType(employeeTypeDto)
    };


        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeTypeList[0].Name)).Throws(new Exception());
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType.Name)).Returns(Task.FromResult(employeeTypeDto));

        _dbMock.Setup(r => r.EmployeeType.Any(It.IsAny<Expression<Func<EmployeeType, bool>>>())).Returns(Task.FromResult(false));
        _dbMock.Setup(e => e.EmployeeType.Get(It.IsAny<Expression<Func<EmployeeType, bool>>>()))
                 .Returns(employeeTypeList.AsQueryable().BuildMock());
        _dbMock.Setup(r => r.EmployeeType.Update(It.IsAny<EmployeeType>())).Returns(Task.FromResult(employeeTypeDto));

        var result = await employeeTypeService.UpdateEmployeeType(employeeTypeDto);

        Assert.NotNull(result);
        Assert.Equal(employeeTypeDto, result);
    }
}
