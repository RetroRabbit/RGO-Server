using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("RoleAccess")]
public class RoleAccess : IModel<RoleAccessDto>
{
    public RoleAccess()
    {
    }

    public RoleAccess(RoleAccessDto roleAccessDto)
    {
        Id = roleAccessDto.Id;
        Permission = roleAccessDto.Permission;
        Grouping = roleAccessDto.Grouping;
    }

    [Column("permission")] public string Permission { get; set; }

    [Column("grouping")] public string Grouping { get; set; }

    [Key] [Column("id")] public int Id { get; set; }

    public RoleAccessDto ToDto()
    {
        return new RoleAccessDto { 
                                 Id = Id,
                                 Permission = Permission,
                                 Grouping = Grouping};
    }
}