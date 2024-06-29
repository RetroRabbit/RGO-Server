using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.Tests.Data;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RGO.Tests.Services;

public class FieldCodeOptionsServiceUnitTests
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;
    private readonly FieldCodeOptions _fieldCodeOptions;
    private readonly FieldCodeOptions _fieldCodeOptions2;
    private readonly FieldCodeOptionsService _fieldCodeOptionsService;

    public FieldCodeOptionsServiceUnitTests()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _errorLoggingServiceMock = new Mock<IErrorLoggingService>();
        _fieldCodeOptionsService = new FieldCodeOptionsService(_dbMock.Object, _errorLoggingServiceMock.Object);
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
    public async Task GetFieldCodeOptionsTest()
    {
        var fields = _fieldCodeOptions.EntityToList();
        _dbMock.Setup(x => x.FieldCodeOptions.GetAll(null)).ReturnsAsync(fields);

        var result = await _fieldCodeOptionsService.GetFieldCodeOptions(_fieldCodeOptions.Id);
        Assert.NotNull(result);
        Assert.Equivalent(fields.Select(x => x.ToDto()).ToList(), result);
        _dbMock.Verify(x => x.FieldCodeOptions.GetAll(null), Times.Once);
    }

    [Fact]
    public async Task SaveFieldCodeOptionsTest()
    {
        var fields = _fieldCodeOptions.EntityToList();
        _dbMock.Setup(x => x.FieldCodeOptions.GetAll(null)).ReturnsAsync(fields);

        _dbMock.Setup(x => x.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>()))
               .ReturnsAsync(_fieldCodeOptions2);

        var result = await _fieldCodeOptionsService.SaveFieldCodeOptions(_fieldCodeOptions2.ToDto());
        Assert.NotNull(result);
        Assert.Equivalent(_fieldCodeOptions2.ToDto(), result);
        _dbMock.Verify(x => x.FieldCodeOptions.Add(It.IsAny<FieldCodeOptions>()), Times.Once);
    }

    [Fact]
    public async Task UpdateFieldCodeOptionsTest()
    {
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
    public async Task DeleteFieldCode()
    {
        var fields = new List<FieldCodeOptions> { _fieldCodeOptions };
        _dbMock.Setup(x => x.FieldCodeOptions.GetAll(null)).ReturnsAsync(fields);

        _dbMock.Setup(x => x.FieldCodeOptions.Delete(It.IsAny<int>()))
               .ReturnsAsync(_fieldCodeOptions);

        var result = await _fieldCodeOptionsService.DeleteFieldCodeOptions(_fieldCodeOptions.ToDto());
        Assert.NotNull(result);
        Assert.Equivalent(_fieldCodeOptions.ToDto(), result);
        _dbMock.Verify(r => r.FieldCodeOptions.Delete(It.IsAny<int>()), Times.Once);
    }
}