using HRIS.Models.Enums;
using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.Tests.Data.Models.HRIS;

public class FieldCodeTestData
{
    public static FieldCodeOptions _fieldCodeOptionsDto = new FieldCodeOptions { Id = 1, FieldCodeId = 1, Option = "string" };
    public static FieldCodeOptions _fieldCodeOptionsDto2 = new FieldCodeOptions { Id = 2, FieldCodeId = 1, Option = "string" };
    public static List<FieldCodeOptionsDto> fieldCodeOptionsList = new List<FieldCodeOptionsDto>
    {
        new FieldCodeOptionsDto { Id = 1, FieldCodeId = 1, Option = "string" },
    };

    public static FieldCode _fieldCodeDto = new()
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

    public static FieldCode _fieldCodeDto2 = new ()
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

    public static FieldCodeDto _fieldCodeDto3 = new()
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
        Required = false,
        Options = fieldCodeOptionsList
    };

    public static FieldCodeDto _fieldCodeDto4 = new()
    {
        Id = 1,
        Code = "CCC222",
        Name = "string3",
        Description = "string",
        Regex = "string",
        Type = FieldCodeType.String,
        Status = ItemStatus.Active,
        Internal = false,
        InternalTable = "",
        Category = FieldCodeCategory.Banking,
        Required = false,
        Options = fieldCodeOptionsList
    };

    public static FieldCode _fieldCode4 = new()
    {
        Id = 0,
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

    public static FieldCode newFieldCodeDto = new()
    {
        Id = 0,
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

    public static FieldCode newFieldCodeDto2 = new()
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

    public static FieldCodeDto newFieldCodeDto3 = new()
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
        Required = false,
        Options = fieldCodeOptionsList
    };
}
