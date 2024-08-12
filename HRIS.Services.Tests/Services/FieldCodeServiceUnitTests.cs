using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class FieldCodeServiceUnitTests
{
    private readonly Mock<IUnitOfWork> _db;
    private readonly FieldCode _fieldCodeDto = FieldCodeTestData._fieldCodeDto;
    private readonly FieldCode _fieldCodeDto2 = FieldCodeTestData._fieldCodeDto2;
    private readonly FieldCodeDto _fieldCodeDto3 = FieldCodeTestData._fieldCodeDto3;
    private readonly FieldCodeDto _fieldCodeDto4 = FieldCodeTestData._fieldCodeDto4;
    private readonly FieldCodeDto _fieldCodeDtoWithNonZeroId = FieldCodeTestData.fieldCodeDtoWithNonZeroId;
    private readonly FieldCodeOptions _fieldCodeOptionsDto = FieldCodeTestData._fieldCodeOptionsDto;
    private readonly FieldCodeOptions _fieldCodeOptionsDto2 = FieldCodeTestData._fieldCodeOptionsDto2;
    private readonly Mock<IFieldCodeOptionsService> _fieldCodeOptionsService;
    private readonly FieldCodeService _fieldCodeService;
    private readonly FieldCodeService _nonSupportFieldCodeService;

    public FieldCodeServiceUnitTests()
    {
        _db = new Mock<IUnitOfWork>();
        _fieldCodeOptionsService = new Mock<IFieldCodeOptionsService>();
        _fieldCodeService = new FieldCodeService(_db.Object, _fieldCodeOptionsService.Object,new AuthorizeIdentityMock("test@gmail.com", "test", "Admin", 1));
        _nonSupportFieldCodeService = new FieldCodeService(_db.Object, _fieldCodeOptionsService.Object, new AuthorizeIdentityMock("test@gmail.com", "test", "User", 1));
    }

    [Fact]
    public async Task GetAllFieldCodesTest()
    {
        _fieldCodeDto.Options = _fieldCodeOptionsDto.EntityToList();
        _fieldCodeDto2.Options = _fieldCodeOptionsDto.EntityToList();

        var fields = new List<FieldCode> { _fieldCodeDto, _fieldCodeDto2 };
        var options = new List<FieldCodeOptions> { _fieldCodeOptionsDto };

        _fieldCodeOptionsService.Setup(x => x.GetFieldCodeOptionsById(It.IsAny<int>()))
                                .ReturnsAsync(options.Select(x => x.ToDto()).ToList());

        _db.Setup(x => x.FieldCode.GetAll(null)).ReturnsAsync(fields);
        var result = await _fieldCodeService.GetAllFieldCodes();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equivalent(fields.Select(x => x.ToDto()).ToList(), result);
    }

    [Fact]
    public async Task CreateFieldCodeTest()
    {
        var newFieldCodeDto = FieldCodeTestData.newFieldCodeDto.ToDto();
        var newFieldCodeEntity = FieldCodeTestData.newFieldCodeDto;

        _db.Setup(x => x.FieldCode.Get(It.IsAny<Expression<Func<FieldCode, bool>>>()))
           .Returns(Enumerable.Empty<FieldCode>().AsQueryable().BuildMock());

        _db.Setup(x => x.FieldCode.Add(It.IsAny<FieldCode>()))
           .ReturnsAsync(newFieldCodeEntity);

        _fieldCodeOptionsService.Setup(x => x.CreateFieldCodeOptions(It.IsAny<FieldCodeOptionsDto>()))
                                .ReturnsAsync(new FieldCodeOptionsDto());

        _fieldCodeOptionsService.Setup(x => x.GetFieldCodeOptionsById(It.IsAny<int>()))
                                .ReturnsAsync(new List<FieldCodeOptionsDto>());

        var result = await _fieldCodeService.CreateFieldCode(newFieldCodeDto);

        Assert.NotNull(result);
        Assert.Equal(newFieldCodeDto.Name, result.Name);
        Assert.Equal(newFieldCodeDto.Description, result.Description);
        Assert.Equal(newFieldCodeDto.Regex, result.Regex);
        Assert.Equal(newFieldCodeDto.Type, result.Type);
        Assert.Equal(newFieldCodeDto.Status, result.Status);
        Assert.Equal(newFieldCodeDto.Internal, result.Internal);
        Assert.Equal(newFieldCodeDto.InternalTable, result.InternalTable);
        Assert.Equal(newFieldCodeDto.Category, result.Category);
        Assert.Equal(newFieldCodeDto.Required, result.Required);

        _db.Verify(x => x.FieldCode.Add(It.IsAny<FieldCode>()), Times.Once);
    }

    [Fact]
    public async Task UpdateFieldCodeTest()
    {
        var updatedFieldCodeDto = _fieldCodeDto.ToDto();
        var updatedFieldCodeEntity = new FieldCode(updatedFieldCodeDto);
        var fields = new List<FieldCode> { _fieldCodeDto, _fieldCodeDto2 }.AsQueryable().BuildMock();
        var options = new List<FieldCodeOptions> { _fieldCodeOptionsDto, _fieldCodeOptionsDto2 };

        _db.Setup(x => x.FieldCode.Get(It.IsAny<Expression<Func<FieldCode, bool>>>()))
           .Returns(fields);

        _db.Setup(x => x.FieldCode.Update(It.IsAny<FieldCode>()))
           .ReturnsAsync(updatedFieldCodeEntity);

        _fieldCodeOptionsService.Setup(x => x.UpdateFieldCodeOptions(It.IsAny<List<FieldCodeOptionsDto>>()))
                                .ReturnsAsync(options.Select(x => x.ToDto()).ToList());

        _fieldCodeOptionsService.Setup(x => x.GetFieldCodeOptionsById(It.IsAny<int>()))
                                .ReturnsAsync(options.Select(x => x.ToDto()).ToList());

        var result = await _fieldCodeService.UpdateFieldCode(updatedFieldCodeDto);

        Assert.NotNull(result);
        Assert.Equivalent(updatedFieldCodeDto, result);
        _db.Verify(x => x.FieldCode.Update(It.IsAny<FieldCode>()), Times.Once);
    }

    [Fact]
    public async Task SaveFieldCodeFailTest()
    {
        await Assert.ThrowsAsync<CustomException>(() => _nonSupportFieldCodeService.CreateFieldCode(_fieldCodeDto.ToDto()));
    }

    [Fact]
    public async Task UpdateFieldCodeFailTest()
    {
        await Assert.ThrowsAsync<CustomException>(() => _nonSupportFieldCodeService.UpdateFieldCode(_fieldCodeDto.ToDto()));
    }

    [Fact]
    public async Task DeleteFieldCodeTest()
    {
        var fields = new List<FieldCode> { _fieldCodeDto }.AsQueryable().BuildMock();

        _db.Setup(x => x.FieldCode.Get(It.IsAny<Expression<Func<FieldCode, bool>>>()))
           .Returns(fields);

        _db.Setup(x => x.FieldCode.Update(It.IsAny<FieldCode>()))
           .ReturnsAsync(_fieldCodeDto);

        var result = await _fieldCodeService.DeleteFieldCode(_fieldCodeDto.ToDto());
        Assert.NotNull(result);
        _fieldCodeDto4.Options = null;
        Assert.Equivalent(_fieldCodeDto.ToDto(), result);
        _db.Verify(r => r.FieldCode.Update(It.IsAny<FieldCode>()), Times.Once);
    }

    [Fact]
    public async Task DeleteFieldCodeFailTest()
    {
        await Assert.ThrowsAsync<CustomException>(() => _nonSupportFieldCodeService.DeleteFieldCode(_fieldCodeDto.ToDto()));
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
            _fieldCodeDto2
        };

        _db.Setup(db => db.FieldCode.Get(It.IsAny<Expression<Func<FieldCode, bool>>>()))
               .Returns(fieldCodes.ToMockIQueryable());

        var result = await _fieldCodeService.GetFieldCodeByCategoryIndex(categoryNumber);

        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetByCategoryFailUnauthorized(int categoryNumber)
    {
        await Assert.ThrowsAsync<CustomException>(() => _nonSupportFieldCodeService.GetFieldCodeByCategoryIndex(categoryNumber));
    }

    [Fact]
    public async Task GetByCategoryFail()
    {
        var invalid = 4;
        await Assert.ThrowsAsync<CustomException>(async () => await _fieldCodeService.GetFieldCodeByCategoryIndex(invalid));

        invalid = -1;
        await Assert.ThrowsAsync<CustomException>(async () => await _fieldCodeService.GetFieldCodeByCategoryIndex(invalid));
    }
}
