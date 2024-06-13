using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RGO.Tests.Services;

public class FieldCodeOptionsServiceUnitTests
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;
    private readonly FieldCodeOptionsDto _fieldCodeOptionsDto;
    private readonly FieldCodeOptionsDto _fieldCodeOptionsDto2;
    private readonly FieldCodeOptionsService _fieldCodeOptionsService;

    public FieldCodeOptionsServiceUnitTests()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _errorLoggingServiceMock = new Mock<IErrorLoggingService>();
        _fieldCodeOptionsService = new FieldCodeOptionsService(_dbMock.Object,_errorLoggingServiceMock.Object);
        _fieldCodeOptionsDto = new FieldCodeOptionsDto { Id=1, FieldCodeId = 1, Option = "string" };
        _fieldCodeOptionsDto2 = new FieldCodeOptionsDto{ Id = 0, FieldCodeId = 1, Option = "string2" };
    }

    [Fact]
    public async Task GetAllFieldCodeOptionsTest()
    {
        var fields = new List<FieldCodeOptionsDto> { _fieldCodeOptionsDto };

        _dbMock.Setup(x => x.FieldCodeOptions.GetAll(null)).Returns(Task.FromResult(fields));
        var result = await _fieldCodeOptionsService.GetAllFieldCodeOptions();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal(fields, result);
    }

    [Fact]
    public async Task GetFieldCodeOptionsTest()
    {
        var fields = new List<FieldCodeOptionsDto> { _fieldCodeOptionsDto };
        _dbMock.Setup(x => x.FieldCodeOptions.GetAll(null)).Returns(Task.FromResult(fields));

        var result = await _fieldCodeOptionsService.GetFieldCodeOptions(_fieldCodeOptionsDto.Id);
        Assert.NotNull(result);
        Assert.Equal(fields, result);
        _dbMock.Verify(x => x.FieldCodeOptions.GetAll(null), Times.Once);
    }

    [Fact]
    public async Task SaveFieldCodeOptionsTest()
    {
        var fields = new List<FieldCodeOptionsDto> { _fieldCodeOptionsDto };
        _dbMock.Setup(x => x.FieldCodeOptions.GetAll(null)).Returns(Task.FromResult(fields));

        _dbMock.Setup(x => x.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>()))
               .Returns(Task.FromResult(_fieldCodeOptionsDto2));

        var result = await _fieldCodeOptionsService.SaveFieldCodeOptions(_fieldCodeOptionsDto2);
        Assert.NotNull(result);
        Assert.Equal(_fieldCodeOptionsDto2, result);
        _dbMock.Verify(x => x.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>()), Times.Once);
    }

    [Fact]
    public async Task UpdateFieldCodeOptionsTest()
    {
        var fieldList = new List<FieldCodeOptionsDto> { _fieldCodeOptionsDto, _fieldCodeOptionsDto2 };
        var field = new List<FieldCodeOptionsDto> { _fieldCodeOptionsDto };
        var field2 = new List<FieldCodeOptionsDto> { _fieldCodeOptionsDto2 };

        _dbMock.SetupSequence(x => x.FieldCodeOptions.GetAll(null))
               .ReturnsAsync(field)
               .ReturnsAsync(fieldList)
               .ReturnsAsync(field2)
               .ReturnsAsync(field2);


        _dbMock.Setup(x => x.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>()))
               .Returns(Task.FromResult(_fieldCodeOptionsDto2));
        _dbMock.Setup(x => x.FieldCodeOptions.Delete(It.IsAny<int>()))
               .Returns(Task.FromResult(_fieldCodeOptionsDto));

        var result = await _fieldCodeOptionsService.UpdateFieldCodeOptions(field2);

        _dbMock.Verify(x => x.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>()), Times.Once);
        _dbMock.Verify(x => x.FieldCodeOptions.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task DeleteFieldCode()
    {
        var fields = new List<FieldCodeOptionsDto> { _fieldCodeOptionsDto };
        _dbMock.Setup(x => x.FieldCodeOptions.GetAll(null)).Returns(Task.FromResult(fields));

        _dbMock.Setup(x => x.FieldCodeOptions.Delete(It.IsAny<int>()))
               .Returns(Task.FromResult(_fieldCodeOptionsDto));

        var result = await _fieldCodeOptionsService.DeleteFieldCodeOptions(_fieldCodeOptionsDto);
        Assert.NotNull(result);
        Assert.Equal(_fieldCodeOptionsDto, result);
        _dbMock.Verify(r => r.FieldCodeOptions.Delete(It.IsAny<int>()), Times.Once);
    }
}