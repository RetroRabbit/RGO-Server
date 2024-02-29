﻿using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RR.UnitOfWork.Tests.Entities;

public class FieldCodeUnitTests
{
    [Fact]
    public void FieldCodeTest()
    {
        var fieldCode = new FieldCode();
        Assert.IsType<FieldCode>(fieldCode);
        Assert.NotNull(fieldCode);
    }

    [Fact]
    public async Task FieldCodeToDtoTest()
    {
        var fieldCodeDto = new FieldCodeDto(1, "Code", "Name", "Description", "Regex", FieldCodeType.String,
                                            ItemStatus.Active, false, "InternalTable", 0, false);
        var fieldCode = new FieldCode(fieldCodeDto);
        var dto = fieldCode.ToDto();
        Assert.NotNull(dto);
        Assert.Equal(dto.Id, fieldCode.Id);
        Assert.Equal(dto.Code, fieldCode.Code);
        Assert.Equal(dto.Name, fieldCode.Name);
        Assert.Equal(dto.Description, fieldCode.Description);
        Assert.Equal(dto.Regex, fieldCode.Regex);
        Assert.Equal(dto.Type, fieldCode.Type);
        Assert.Equal(dto.Status, fieldCode.Status);
        Assert.Equal(dto.Internal, fieldCode.Internal);
        Assert.Equal(dto.InternalTable, fieldCode.InternalTable);
        Assert.Equal(dto.Category, fieldCode.Category);
        Assert.Equal(dto.Required, fieldCode.Required);
    }
}