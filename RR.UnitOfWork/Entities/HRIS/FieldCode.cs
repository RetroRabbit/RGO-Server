﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("FieldCode")]
public class FieldCode : IModel
{
    public FieldCode()
    {
        Internal = false;
    }

    public FieldCode(FieldCodeDto fieldCodeDto)
    {
        Id = fieldCodeDto.Id;
        Code = fieldCodeDto.Code;
        Name = fieldCodeDto.Name;
        Description = fieldCodeDto.Description;
        Regex = fieldCodeDto.Regex;
        Type = fieldCodeDto.Type;
        Status = fieldCodeDto.Status;
        Internal = fieldCodeDto.Internal;
        InternalTable = fieldCodeDto.InternalTable;
        Category = fieldCodeDto.Category;
        Required = fieldCodeDto.Required;
    }

    [Column("code")] public string? Code { get; set; }

    [Column("name")] public string? Name { get; set; }

    [Column("description")] public string? Description { get; set; }

    [Column("regex")] public string? Regex { get; set; }

    [Column("type")] public FieldCodeType Type { get; set; }

    [Column("status")] public ItemStatus Status { get; set; }

    [Column("internal")] public bool Internal { get; set; }

    [Column("internalTable")] public string? InternalTable { get; set; }

    [Column("category")] public FieldCodeCategory Category { get; set; }

    [Column("required")] public bool Required { get; set; }

    [Key] [Column("id")] public int Id { get; set; }

    public virtual List<FieldCodeOptions>? Options { get; set; }

    public FieldCodeDto ToDto()
    {
        return new FieldCodeDto
        {
            Id = Id,
            Code = Code,
            Name = Name,
            Description = Description,
            Regex = Regex,
            Type = Type,
            Status = Status,
            Internal = Internal,
            InternalTable = InternalTable,
            Category = Category,
            Required = Required,
            Options = Options?.Select(x => x.ToDto()).ToList() ?? new List<FieldCodeOptionsDto>()
        };
    }
}
