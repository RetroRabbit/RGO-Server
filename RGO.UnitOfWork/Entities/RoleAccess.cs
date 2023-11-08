using RGO.Models;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.UnitOfWork.Entities;

[Table("RoleAccess")]
public class RoleAccess : IModel<RoleAccessDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("permission")]
    public string Permission { get; set; }

    [Column("grouping")]
    public string Grouping { get; set; }

    public RoleAccess() { }

    public RoleAccess(RoleAccessDto roleAccessDto)
    {
        Id = roleAccessDto.Id;
        Permission = roleAccessDto.Permission;
        Grouping = roleAccessDto.Grouping;
    }

    public RoleAccessDto ToDto()
    {
        return new RoleAccessDto(
            Id,
            Permission,
            Grouping);
    }
}
