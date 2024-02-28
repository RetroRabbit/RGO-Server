using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class FieldCodeOptionsUnitTests
{
    [Fact]
    public void FieldCodeOptionsTest()
    {
        var fieldCodeOptions = new FieldCodeOptions();
        Assert.IsType<FieldCodeOptions>(fieldCodeOptions);
        Assert.NotNull(fieldCodeOptions);
    }

    [Fact]
    public void FieldCodeOptionsToDtoTest()
    {
        var fieldCodeOptionsDto = new FieldCodeOptionsDto(1, 1, "Option");
        var fieldCodeOptions = new FieldCodeOptions(fieldCodeOptionsDto);
        fieldCodeOptions.FieldCode = new FieldCode();
        var dto = fieldCodeOptions.ToDto();

        Assert.NotNull(fieldCodeOptions.FieldCode);
        Assert.Equal(dto.Id, fieldCodeOptions.Id);
        Assert.Equal(dto.FieldCodeId, fieldCodeOptions.FieldCodeId);
        Assert.Equal(dto.Option, fieldCodeOptions.Option);

        fieldCodeOptions = new FieldCodeOptions(dto);
        Assert.Null(fieldCodeOptions.FieldCode);
    }
}