using RGO.Models;
using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.UnitOfWork.Entities;

[Table("RoleAccessLink")]
public class RoleAccessLink : IModel<RoleAccessLinkDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("roleId")]
    [ForeignKey("Role")]
    public int RoleId { get; set; }

    [Column("roleAccessId")]
    [ForeignKey("RoleAccess")]
    public int RoleAccessId { get; set; }

    public virtual Role Role { get; set; }
    public virtual RoleAccess RoleAccess { get; set; }

    public RoleAccessLink() { }

    public RoleAccessLink(RoleAccessLinkDto roleAccessLinkDto)
    {
        Id = roleAccessLinkDto.Id;
        RoleId = roleAccessLinkDto.Role.Id;
        RoleAccessId = roleAccessLinkDto.RoleAccess.Id;
    }

    public RoleAccessLinkDto ToDto()
    {
        return new RoleAccessLinkDto(
            Id,
            Role?.ToDto(),
            RoleAccess?.ToDto());
    }
}
