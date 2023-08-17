using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RGO.Models;

namespace RGO.UnitOfWork.Entities;

[Table("UserRole")]
public class UserRole : IModel<RoleDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("userId")]
    [ForeignKey("User")]
    public int UserId { get; set; }

    [Column("roleId")]
    [ForeignKey("Role")]
    public int RoleId { get; set; }

    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
    public RoleDto ToDto()
    {
        //TODO : Description??
        return new RoleDto(Id, UserId, "");
    }
}
