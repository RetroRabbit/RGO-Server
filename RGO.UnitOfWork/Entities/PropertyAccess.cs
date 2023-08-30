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

    public virtual Role Role { get; set; }

    public virtual FieldCode? FieldCode { get; set; }

    public PropertyAccess()
    {
    }

    public PropertyAccess(PropertyAccessDto propertyAccessDto)
    {
        Id = propertyAccessDto.Id;
        RoleId = propertyAccessDto.Role.Id;
        Condition = propertyAccessDto.Condition;
        FieldCodeId = propertyAccessDto.FieldCode?.Id;
    }

    public PropertyAccessDto ToDto()
    {
        return new PropertyAccessDto(
            Id,
            Role.ToDto(),
            Condition,
            FieldCode?.ToDto());
    }
}
