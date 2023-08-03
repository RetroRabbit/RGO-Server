using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.Repository.Entities;
[Table("UserStacks")]
public class UserStacks
{
    [Key]
    [Column("id")]
    public int Id { get; set; }/*

    [Column("userId")]
    public int UserId { get; set; }

    [Column("backendId")]
    public int BackendId { get; set; }

    [Column("frontendId")]
    public int FrontendId { get; set; }

    [Column("databaseId")]
    public int DatabaseId { get; set; }*/

    [Column("createDate")]
    public DateTime CreateDate { get; set; }

    [ForeignKey("userId")]
    public virtual User User { get; set; }

    [ForeignKey("backendId")]
    public virtual Stacks BackendUserStack { get; set; }

    [ForeignKey("frontendId")]
    public virtual Stacks FrontendUserStack { get; set; }

    [ForeignKey("databaseId")]
    public virtual Stacks DatabaseUserStack { get; set; }
}