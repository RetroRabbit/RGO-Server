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

        _fieldCodeOptionsService.Setup(x => x.GetFieldCodeOptions(It.IsAny<int>()))
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
        var fields = new List<FieldCode> { _fieldCodeDto, _fieldCodeDto2 };
        var options = new List<FieldCodeOptions> { _fieldCodeOptionsDto };

        _db.Setup(x => x.FieldCode.GetAll(null)).ReturnsAsync(fields);
        _db.Setup(x => x.FieldCode.Add(It.IsAny<FieldCode>()))
           .ReturnsAsync(FieldCodeTestData.newFieldCodeDto);
        _db.Setup(x => x.FieldCode.Add(It.IsAny<FieldCode>()))
           .ReturnsAsync(_fieldCodeDto2);

        _fieldCodeOptionsService.Setup(x => x.SaveFieldCodeOptions(It.IsAny<FieldCodeOptionsDto>()))
                                .ReturnsAsync(_fieldCodeOptionsDto.ToDto());

        _fieldCodeOptionsService.Setup(x => x.GetFieldCodeOptions(It.IsAny<int>()))
                                .ReturnsAsync(options.Select(x => x.ToDto()).ToList());

        var newSave = await _fieldCodeService.CreateFieldCode(FieldCodeTestData.newFieldCodeDto.ToDto());
        var result = await _fieldCodeService.CreateFieldCode(_fieldCodeDto3);

        Assert.NotNull(newSave);
        Assert.Equivalent(FieldCodeTestData.newFieldCodeDto2.ToDto(), newSave);

        Assert.NotNull(result);
        Assert.Equivalent(_fieldCodeDto2.ToDto(), result);

        _db.Verify(x => x.FieldCode.Add(It.IsAny<FieldCode>()), Times.Exactly(2));
    }

    [Fact]
    public async Task SaveFieldCodeFailTest()
    {
        await Assert.ThrowsAsync<CustomException>(() => _nonSupportFieldCodeService.CreateFieldCode(_fieldCodeDto.ToDto()));
    }

    //[Fact]
    //public async Task UpdateFieldCodeTest()
    //{
    //    var fields = new List<FieldCode> { _fieldCodeDto, _fieldCodeDto2 };
    //    var options = new List<FieldCodeOptions> { _fieldCodeOptionsDto, _fieldCodeOptionsDto2 };
    //    var optionsList2 = new List<FieldCodeOptions> { _fieldCodeOptionsDto };
    //    _db.Setup(x => x.FieldCode.Update(It.IsAny<FieldCode>()))
    //           .ReturnsAsync(_fieldCodeDto);
    //    _db.Setup(x => x.FieldCode.GetAll(null)).ReturnsAsync(fields);
    //    _fieldCodeOptionsService.Setup(x => x.UpdateFieldCodeOptions(It.IsAny<List<FieldCodeOptionsDto>>()))
    //                            .ReturnsAsync(optionsList2.Select(x => x.ToDto()).ToList());
    //    _fieldCodeOptionsService.Setup(x => x.GetAllFieldCodeOptions()).ReturnsAsync(options.Select(x => x.ToDto()).ToList());
    //    _fieldCodeOptionsService.Setup(x => x.GetFieldCodeOptions(It.IsAny<int>()))
    //                            .ReturnsAsync(options.Select(x => x.ToDto()).ToList());
    //    _db.Setup(x => x.FieldCodeOptions.Delete(It.IsAny<int>())).ReturnsAsync(_fieldCodeOptionsDto2);
    //    _fieldCodeDto.Options = options;
    //    var result = await _fieldCodeService.UpdateFieldCode(_fieldCodeDto.ToDto());
    //    Assert.NotNull(result);
    //    Assert.Equivalent(_fieldCodeDto.ToDto(), result);
    //    _db.Verify(x => x.FieldCode.Update(It.IsAny<FieldCode>()), Times.Once);
    //}

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

        var result = await _fieldCodeService.GetByCategory(categoryNumber);

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
        await Assert.ThrowsAsync<CustomException>(() => _nonSupportFieldCodeService.GetByCategory(categoryNumber));
    }

    [Fact]
    public async Task DeleteThrowNoFieldFoundException()
    {
        var fields = new List<FieldCode> { _fieldCodeDto, _fieldCodeDto2 };

        _db.Setup(x => x.FieldCode.GetAll(null)).ReturnsAsync(fields);
        _db.Setup(x => x.FieldCode.Update(It.IsAny<FieldCode>()))
               .ReturnsAsync(_fieldCodeDto);

        await _fieldCodeService.GetAllFieldCodes();

        await Assert.ThrowsAsync<CustomException>(async () => await _fieldCodeService.DeleteFieldCode(FieldCodeTestData._fieldCodeDto3));
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
