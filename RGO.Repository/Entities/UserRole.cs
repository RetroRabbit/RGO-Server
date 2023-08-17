using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.Repository.Entities;

[Table("UserRole")]
public class UserRole
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
}
