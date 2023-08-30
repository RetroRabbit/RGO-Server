using RGO.Models;
using RGO.Models.Enums;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.UnitOfWork.Entities;

public class PropertyAccess : IModel<PropertyAccessDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("roleId")]
    [ForeignKey("Role")]
    public int RoleId { get; set; }

    [Column("condition")]
    public int Condition { get; set; }

    [Column("fieldCodeId")]
    [ForeignKey("FieldCode")]
    public int? FieldCodeId { get; set; }

    [Column("metaPropertyId")]
    [ForeignKey("MetaProperty")]
    public int? MetaPropertyId { get; set; }

    [Column("metaField")]
    public string MetaField { get; set; }

    public virtual Role Role { get; set; }

    public virtual FieldCode FieldCode { get; set; }

    public virtual MetaProperty MetaProperty { get; set; }

    public PropertyAccess()
    {
    }

    public PropertyAccess(MetaPropertyDto metaPropertyDto, FieldCodeDto fieldCodeDto ,PropertyAccessDto propertyAccessDto, RoleDto roleDto)
    {
        Id = propertyAccessDto.Id;
        RoleId = roleDto.Id;
        Condition = propertyAccessDto.Condition;
        FieldCodeId = fieldCodeDto.Id;
        MetaPropertyId = metaPropertyDto.Id;
        MetaField = propertyAccessDto.metaField;
    }

    public PropertyAccessDto ToDto()
    {
        return new PropertyAccessDto(
            Id,
            Role.ToDto(),
            Condition,
            FieldCode.ToDto(),
            MetaProperty.ToDto(),
            MetaField);
    }
}
