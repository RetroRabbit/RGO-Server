using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RGO.Tests.Services;

public class FieldCodeServiceUnitTests
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IErrorLoggingService> _errorLoggingServiceMock;
    private readonly FieldCodeDto _fieldCodeDto;
    private readonly FieldCodeDto _fieldCodeDto2;
    private readonly FieldCodeDto _fieldCodeDto3;
    private readonly FieldCodeDto _fieldCodeDto4;
    private readonly FieldCodeOptionsDto _fieldCodeOptionsDto;
    private readonly FieldCodeOptionsDto _fieldCodeOptionsDto2;
    private readonly Mock<IFieldCodeOptionsService> _fieldCodeOptionsService;
    private readonly FieldCodeService _fieldCodeService;

    public FieldCodeServiceUnitTests()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _errorLoggingServiceMock = new Mock<IErrorLoggingService>();
        _fieldCodeOptionsService = new Mock<IFieldCodeOptionsService>();

        _fieldCodeDto = new FieldCodeDto
        {
            Id = 1,
            Code = "AAA000",
            Name = "string",
            Description = "string",
            Regex = "string",
            Type = FieldCodeType.String,
            Status = ItemStatus.Active,
            Internal = false,
            InternalTable = "",
            Category = FieldCodeCategory.Profile,
            Required = false
        };

        _fieldCodeDto2 = new FieldCodeDto
        {
            Id = 2,
            Code = "AAA000",
            Name = "string2",
            Description = "string",
            Regex = "string",
            Type = FieldCodeType.String,
            Status = ItemStatus.Archive,
            Internal = false,
            InternalTable = "",
            Category = FieldCodeCategory.Profile,
            Required = false
        };

        _fieldCodeDto3 = new FieldCodeDto
        {
            Id = 0,
            Code = "CCC222",
            Name = "string3",
            Description = "string",
            Regex = "string",
            Type = FieldCodeType.String,
            Status = ItemStatus.Active,
            Internal = false,
            InternalTable = "",
            Category = FieldCodeCategory.Banking,
            Required = false
        };

        _fieldCodeDto4 = new FieldCodeDto
        {
            Id = 1,
            Code = "AAA000",
            Name = "string",
            Description = "string",
            Regex = "string",
            Type = FieldCodeType.String,
            Status = ItemStatus.Archive,
            Internal = false,
            InternalTable = "",
            Category = FieldCodeCategory.Documents,
            Required = false
        };

        _fieldCodeOptionsDto = new FieldCodeOptionsDto{ Id = 1, FieldCodeId = 1, Option = "string" };
        _fieldCodeOptionsDto2 = new FieldCodeOptionsDto{ Id = 2, FieldCodeId = 1, Option = "string" };
        _fieldCodeService = new FieldCodeService(_dbMock.Object, _fieldCodeOptionsService.Object, _errorLoggingServiceMock.Object);
    }

    [Fact]
    public async Task GetAllFieldCodesTest()
    {
        var fields = new List<FieldCodeDto> { _fieldCodeDto, _fieldCodeDto2 };
        var options = new List<FieldCodeOptionsDto> { _fieldCodeOptionsDto };

        _fieldCodeOptionsService.Setup(x => x.GetFieldCodeOptions(It.IsAny<int>()))
                                .Returns(Task.FromResult(options));

        _dbMock.Setup(x => x.FieldCode.GetAll(null)).Returns(Task.FromResult(fields));
        var result = await _fieldCodeService.GetAllFieldCodes();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal(fields, result);
    }

    [Fact]
    public async Task SaveFieldCodeTest()
    {
        var fields = new List<FieldCodeDto> { _fieldCodeDto, _fieldCodeDto2 };
        var options = new List<FieldCodeOptionsDto> { _fieldCodeOptionsDto };
        _fieldCodeDto3.Options = options;

        _dbMock.Setup(x => x.FieldCode.GetAll(null)).Returns(Task.FromResult(fields));

        _dbMock.Setup(x => x.FieldCode.Add(It.IsAny<FieldCode>()))
               .Returns(Task.FromResult(_fieldCodeDto3));
        _fieldCodeOptionsService.Setup(x => x.SaveFieldCodeOptions(It.IsAny<FieldCodeOptionsDto>()))
                                .Returns(Task.FromResult(_fieldCodeOptionsDto));
        _fieldCodeOptionsService.Setup(x => x.GetFieldCodeOptions(It.IsAny<int>()))
                                .Returns(Task.FromResult(options));

        var result = await _fieldCodeService.SaveFieldCode(_fieldCodeDto3);

        Assert.NotNull(result);
        Assert.Equal(_fieldCodeDto3, result);
        _dbMock.Verify(x => x.FieldCode.Add(It.IsAny<FieldCode>()), Times.Once);
    }

    [Fact]
    public async Task UpdateFieldCodeTest()
    {
        var fields = new List<FieldCodeDto> { _fieldCodeDto, _fieldCodeDto2 };
        var options = new List<FieldCodeOptionsDto> { _fieldCodeOptionsDto, _fieldCodeOptionsDto2 };
        var optionsList2 = new List<FieldCodeOptionsDto> { _fieldCodeOptionsDto };
        _dbMock.Setup(x => x.FieldCode.Update(It.IsAny<FieldCode>()))
               .Returns(Task.FromResult(_fieldCodeDto));
        _dbMock.Setup(x => x.FieldCode.GetAll(null)).Returns(Task.FromResult(fields));
        _fieldCodeOptionsService.Setup(x => x.UpdateFieldCodeOptions(It.IsAny<List<FieldCodeOptionsDto>>()))
                                .Returns(Task.FromResult(optionsList2));
        _fieldCodeOptionsService.Setup(x => x.GetAllFieldCodeOptions()).Returns(Task.FromResult(options));
        _fieldCodeOptionsService.Setup(x => x.GetFieldCodeOptions(It.IsAny<int>()))
                                .Returns(Task.FromResult(options));
        _dbMock.Setup(x => x.FieldCodeOptions.Delete(It.IsAny<int>())).Returns(Task.FromResult(_fieldCodeOptionsDto2));
        _fieldCodeDto.Options = options;
        var result = await _fieldCodeService.UpdateFieldCode(_fieldCodeDto);
        Assert.NotNull(result);
        Assert.Equal(_fieldCodeDto, result);
        _dbMock.Verify(x => x.FieldCode.Update(It.IsAny<FieldCode>()), Times.Once);
    }

    [Fact]
    public async Task DeleteFieldCodeTest()
    {
        var fields = new List<FieldCodeDto> { _fieldCodeDto };
        _dbMock.Setup(x => x.FieldCode.GetAll(null)).Returns(Task.FromResult(fields));

        _dbMock.Setup(x => x.FieldCode.Update(It.IsAny<FieldCode>()))
               .Returns(Task.FromResult(_fieldCodeDto4));

        var result = await _fieldCodeService.DeleteFieldCode(_fieldCodeDto);
        Assert.NotNull(result);
        Assert.Equal(_fieldCodeDto4, result);
        _dbMock.Verify(r => r.FieldCode.Update(It.IsAny<FieldCode>()), Times.Once);
    }

    [Fact]
    public async Task GetByCategoryPass()
    {
        var fieldCodes = new List<FieldCode>
        {
            new(_fieldCodeDto),
            new(_fieldCodeDto2)
        };

        _dbMock.Setup(db => db.FieldCode.Get(It.IsAny<Expression<Func<FieldCode, bool>>>()))
               .Returns(fieldCodes.AsQueryable().BuildMock());

        var category = 0;

        var result = await _fieldCodeService.GetByCategory(category);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        fieldCodes = new List<FieldCode>
        {
            new(_fieldCodeDto3)
        };

        _dbMock.Setup(db => db.FieldCode.Get(It.IsAny<Expression<Func<FieldCode, bool>>>()))
               .Returns(fieldCodes.AsQueryable().BuildMock());

        category = 1;

        result = await _fieldCodeService.GetByCategory(category);

        Assert.NotNull(result);
        Assert.Single(result);

        fieldCodes = new List<FieldCode>
        {
            new(_fieldCodeDto4)
        };

        _dbMock.Setup(db => db.FieldCode.Get(It.IsAny<Expression<Func<FieldCode, bool>>>()))
               .Returns(fieldCodes.AsQueryable().BuildMock());

        category = 2;

        result = await _fieldCodeService.GetByCategory(category);

        Assert.NotNull(result);
        Assert.Single(result);

        _dbMock.Setup(db => db.FieldCode.Get(It.IsAny<Expression<Func<FieldCode, bool>>>()))
               .Returns(fieldCodes.AsQueryable().BuildMock());

        category = 3;

        result = await _fieldCodeService.GetByCategory(category);

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task DeleteThrowNoFieldFoundException()
    {
        var fieldCodeDto5 = new FieldCodeDto
        {
            Id = 1,
            Code = "AAA000",
            Name = "RandomField",
            Description = "string",
            Regex = "string",
            Type = FieldCodeType.String,
            Status = ItemStatus.Archive,
            Internal = false,
            InternalTable = "",
            Category = FieldCodeCategory.Documents,
            Required = false
        };

        var fields = new List<FieldCodeDto> { fieldCodeDto5, _fieldCodeDto2 };
        var options = new List<FieldCodeOptionsDto> { _fieldCodeOptionsDto };
        _fieldCodeDto.Options = options;


        _dbMock.Setup(x => x.FieldCode.Add(It.IsAny<FieldCode>()))
              .Returns(Task.FromResult(fieldCodeDto5));
        _fieldCodeOptionsService.Setup(x => x.SaveFieldCodeOptions(It.IsAny<FieldCodeOptionsDto>()))
                                .Returns(Task.FromResult(_fieldCodeOptionsDto));
        _fieldCodeOptionsService.Setup(x => x.GetFieldCodeOptions(It.IsAny<int>()))
                                .Returns(Task.FromResult(options));

        _dbMock.Setup(x => x.FieldCode.Add(It.IsAny<FieldCode>()))
              .Returns(Task.FromResult(fieldCodeDto5));

        _dbMock.Setup(x => x.FieldCode.GetAll(It.IsAny<Expression<Func<FieldCode, bool>>>())).Returns(Task.FromResult(fields));
        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception("No field with that name found"));

        await Assert.ThrowsAsync<Exception>(async () => await _fieldCodeService.GetFieldCode("AdminDocuments"));
    }

    [Fact]
    public async Task GetByCategoryFail()
    {
        _errorLoggingServiceMock.Setup(r => r.LogException(It.IsAny<Exception>())).Throws(new Exception());

        var invalid = 4;
        await Assert.ThrowsAsync<Exception>(async () => await _fieldCodeService.GetByCategory(invalid));

        invalid = -1;
        await Assert.ThrowsAsync<Exception>(async () => await _fieldCodeService.GetByCategory(invalid));
    }
}
