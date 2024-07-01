using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("PropertyAccess")]
public class PropertyAccess : IModel
{
    public PropertyAccess()
    {
    }

    public PropertyAccess(PropertyAccessDto propertyAccessDto)
    {
        Id = propertyAccessDto.Id;
        RoleId = propertyAccessDto.Role.Id;
        Table = propertyAccessDto.Table;
        Field = propertyAccessDto.Field;
        AccessLevel = propertyAccessDto.AccessLevel;
    }

    [Key][Column("id")] public int Id { get; set; }

    [Column("roleId")]
    [ForeignKey("Role")]
    public int RoleId { get; set; }

    public virtual Role Role { get; set; }

    public string Table { get; set; }

    public string Field { get; set; }

    public PropertyAccessLevel AccessLevel { get; set; }

    public PropertyAccessDto ToDto()
    {
        return new PropertyAccessDto
        {
            Id = Id,
            RoleId = RoleId,
            Role = Role?.ToDto(),
            Table = Table,
            Field =Field,
            AccessLevel = AccessLevel
        };
    }
}