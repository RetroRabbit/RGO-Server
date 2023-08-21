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

    [Column("roleId")]
    [ForeignKey("Role")]
    public int RoleId { get; set; }

    [Column("action")]
    public string Action { get; set; }

    [Column("view")]
    public bool View { get; set; }

    [Column("edit")]
    public bool Edit { get; set; }

    [Column("delete")]
    public bool Delete { get; set; }

    public virtual Role Role { get; set; }

    public RoleAccess() { }

    public RoleAccess(RoleAccessDto roleAccessDto)
    {
        Id = roleAccessDto.Id;
        RoleId = roleAccessDto.Role.Id;
        Action = roleAccessDto.Action;
        View = roleAccessDto.View;
        Edit = roleAccessDto.Edit;
        Delete = roleAccessDto.Delete;
    }

    public RoleAccessDto ToDto()
    {
        return new RoleAccessDto(
            Id,
            Role.ToDto(),
            Action,
            View,
            Edit,
            Delete);
    }
}
