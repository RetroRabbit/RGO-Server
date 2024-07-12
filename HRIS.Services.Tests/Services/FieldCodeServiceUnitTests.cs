using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.Tests.Data;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class FieldCodeServiceUnitTests
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly FieldCode _fieldCodeDto;
    private readonly FieldCode _fieldCodeDto2;
    private readonly FieldCode _fieldCodeDto3;
    private readonly FieldCode _fieldCodeDto4;
    private readonly FieldCodeOptions _fieldCodeOptionsDto;
    private readonly FieldCodeOptions _fieldCodeOptionsDto2;
    private readonly Mock<IFieldCodeOptionsService> _fieldCodeOptionsService;
    private readonly FieldCodeService _fieldCodeService;

    public FieldCodeServiceUnitTests()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _fieldCodeOptionsService = new Mock<IFieldCodeOptionsService>();

        _fieldCodeDto = new FieldCode
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

        _fieldCodeDto2 = new FieldCode
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

        _fieldCodeDto3 = new FieldCode
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

        _fieldCodeDto4 = new FieldCode
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

        _fieldCodeOptionsDto = new FieldCodeOptions { Id = 1, FieldCodeId = 1, Option = "string" };
        _fieldCodeOptionsDto2 = new FieldCodeOptions { Id = 2, FieldCodeId = 1, Option = "string" };
        _fieldCodeService = new FieldCodeService(_dbMock.Object, _fieldCodeOptionsService.Object);
    }

    [Fact]
    public async Task GetAllFieldCodesTest()
    {
        _fieldCodeDto.Options = _fieldCodeOptionsDto.EntityToList();
        _fieldCodeDto2.Options = _fieldCodeOptionsDto.EntityToList();

        var fields = new List<FieldCode> { _fieldCodeDto, _fieldCodeDto2 };
        var options = new List<FieldCodeOptions> { _fieldCodeOptionsDto };

        _fieldCodeOptionsService.Setup(x => x.GetFieldCodeOptions(It.IsAny<int>()))
                                .ReturnsAsync(options.Select(x => x.ToDto()).ToList());

        _dbMock.Setup(x => x.FieldCode.GetAll(null)).ReturnsAsync(fields);
        var result = await _fieldCodeService.GetAllFieldCodes();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equivalent(fields.Select(x => x.ToDto()).ToList(), result);
    }

    [Fact]
    public async Task SaveFieldCodeTest()
    {
        var newFieldCodeDto = new FieldCode
        {
            Id = -1,
            Code = "AAA000",
            Name = "RandomValue",
            Description = "string",
            Regex = "string",
            Type = FieldCodeType.String,
            Status = ItemStatus.Active,
            Internal = false,
            InternalTable = "",
            Category = FieldCodeCategory.Profile,
            Required = false
        };

        var fields = new List<FieldCode> { _fieldCodeDto, _fieldCodeDto2, newFieldCodeDto };
        var options = new List<FieldCodeOptions> { _fieldCodeOptionsDto };
        _fieldCodeDto3.Options = options;
        newFieldCodeDto.Options = options;

        _dbMock.Setup(x => x.FieldCode.GetAll(null)).ReturnsAsync(fields);

        _dbMock.Setup(x => x.FieldCode.Add(It.IsAny<FieldCode>()))
               .ReturnsAsync(newFieldCodeDto);
        _dbMock.Setup(x => x.FieldCode.Add(It.IsAny<FieldCode>()))
               .ReturnsAsync(_fieldCodeDto3);
        _fieldCodeOptionsService.Setup(x => x.SaveFieldCodeOptions(It.IsAny<FieldCodeOptionsDto>()))
                                .ReturnsAsync(_fieldCodeOptionsDto.ToDto());
        _fieldCodeOptionsService.Setup(x => x.GetFieldCodeOptions(It.IsAny<int>()))
                                .ReturnsAsync(options.Select(x => x.ToDto()).ToList());

        var newSave = await _fieldCodeService.SaveFieldCode(newFieldCodeDto.ToDto());
        var result = await _fieldCodeService.SaveFieldCode(_fieldCodeDto3.ToDto());

        Assert.NotNull(result);
        Assert.Equivalent(_fieldCodeDto3.ToDto(), result);
        _dbMock.Verify(x => x.FieldCode.Add(It.IsAny<FieldCode>()), Times.Once);
        Assert.NotNull(newSave);
        Assert.Equivalent(newFieldCodeDto.ToDto(), newSave);
        _dbMock.Verify(x => x.FieldCode.Add(It.IsAny<FieldCode>()), Times.Once);
    }

    [Fact]
    public async Task UpdateFieldCodeTest()
    {
        var fields = new List<FieldCode> { _fieldCodeDto, _fieldCodeDto2 };
        var options = new List<FieldCodeOptions> { _fieldCodeOptionsDto, _fieldCodeOptionsDto2 };
        var optionsList2 = new List<FieldCodeOptions> { _fieldCodeOptionsDto };
        _dbMock.Setup(x => x.FieldCode.Update(It.IsAny<FieldCode>()))
               .ReturnsAsync(_fieldCodeDto);
        _dbMock.Setup(x => x.FieldCode.GetAll(null)).ReturnsAsync(fields);
        _fieldCodeOptionsService.Setup(x => x.UpdateFieldCodeOptions(It.IsAny<List<FieldCodeOptionsDto>>()))
                                .ReturnsAsync(optionsList2.Select(x => x.ToDto()).ToList());
        _fieldCodeOptionsService.Setup(x => x.GetAllFieldCodeOptions()).ReturnsAsync(options.Select(x => x.ToDto()).ToList());
        _fieldCodeOptionsService.Setup(x => x.GetFieldCodeOptions(It.IsAny<int>()))
                                .ReturnsAsync(options.Select(x => x.ToDto()).ToList());
        _dbMock.Setup(x => x.FieldCodeOptions.Delete(It.IsAny<int>())).ReturnsAsync(_fieldCodeOptionsDto2);
        _fieldCodeDto.Options = options;
        var result = await _fieldCodeService.UpdateFieldCode(_fieldCodeDto.ToDto());
        Assert.NotNull(result);
        Assert.Equivalent(_fieldCodeDto.ToDto(), result);
        _dbMock.Verify(x => x.FieldCode.Update(It.IsAny<FieldCode>()), Times.Once);
    }

    [Fact]
    public async Task DeleteFieldCodeTest()
    {
        var fields = new List<FieldCode> { _fieldCodeDto };
        _dbMock.Setup(x => x.FieldCode.GetAll(null)).ReturnsAsync(fields);

        _dbMock.Setup(x => x.FieldCode.Update(It.IsAny<FieldCode>()))
               .ReturnsAsync(_fieldCodeDto4);

        var result = await _fieldCodeService.DeleteFieldCode(_fieldCodeDto.ToDto());
        Assert.NotNull(result);
        _fieldCodeDto4.Options = null;
        Assert.Equivalent(_fieldCodeDto4.ToDto(), result);
        _dbMock.Verify(r => r.FieldCode.Update(It.IsAny<FieldCode>()), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetByCategoryPass(int categoryNumber)
    {
        var fieldCodes = new List<FieldCode>
        {
            _fieldCodeDto,
            _fieldCodeDto2
        };

        fieldCodes = new List<FieldCode>
        {
            _fieldCodeDto3
        };

        _dbMock.Setup(db => db.FieldCode.Get(It.IsAny<Expression<Func<FieldCode, bool>>>()))
               .Returns(fieldCodes.ToMockIQueryable());

        var result = await _fieldCodeService.GetByCategory(categoryNumber);

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task DeleteThrowNoFieldFoundException()
    {
        var newFieldCodeDto = new FieldCodeDto
        {
            Id = 1,
            Code = "AAA000",
            Name = "RandomValue",
            Description = "string",
            Regex = "string",
            Type = FieldCodeType.String,
            Status = ItemStatus.Active,
            Internal = false,
            InternalTable = "",
            Category = FieldCodeCategory.Profile,
            Required = false
        };

        var fields = new List<FieldCode> { _fieldCodeDto, _fieldCodeDto2 };

        _dbMock.Setup(x => x.FieldCode.GetAll(null)).ReturnsAsync(fields);
        _dbMock.Setup(x => x.FieldCode.Update(It.IsAny<FieldCode>()))
               .ReturnsAsync(_fieldCodeDto);

        await _fieldCodeService.GetAllFieldCodes();

        await Assert.ThrowsAsync<CustomException>(async () => await _fieldCodeService.DeleteFieldCode(newFieldCodeDto));
    }

    [Fact]
    public async Task GetByCategoryFail()
    {
        var invalid = 4;
        await Assert.ThrowsAsync<CustomException>(async () => await _fieldCodeService.GetByCategory(invalid));

        invalid = -1;
        await Assert.ThrowsAsync<CustomException>(async () => await _fieldCodeService.GetByCategory(invalid));
    }
}