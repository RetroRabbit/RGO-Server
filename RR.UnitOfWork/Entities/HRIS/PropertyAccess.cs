using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("PropertyAccess")]
public class PropertyAccess : IModel<PropertyAccessDto>
{
    public PropertyAccess()
    {
    }

    public PropertyAccess(PropertyAccessDto propertyAccessDto)
    {
        Id = propertyAccessDto.Id;
        RoleId = propertyAccessDto.Role!.Id;
        Condition = propertyAccessDto.Condition;
        FieldCodeId = propertyAccessDto.FieldCode!.Id;
    }

    [Column("roleId")]
    [ForeignKey("Role")]
    public int RoleId { get; set; }

    [Column("condition")] public int Condition { get; set; }

    [Column("fieldCodeId")]
    [ForeignKey("FieldCode")]
    public int FieldCodeId { get; set; }

    public virtual Role? Role { get; set; }

    public virtual FieldCode? FieldCode { get; set; }

    [Key] [Column("id")] public int Id { get; set; }

    public PropertyAccessDto ToDto()
    {
        return new PropertyAccessDto {
                                     Id = Id,
                                     Role = Role?.ToDto(),
                                     Condition = Condition,
                                     FieldCode = FieldCode?.ToDto()};
    }
}