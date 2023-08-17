using RGO.UnitOfWork.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RGO.Models;

namespace RGO.UnitOfWork.Entities;

[Table("Role")]
public class Role : IModel<RoleDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("Description")]
    public string Description { get; set; }

    public RoleDto ToDto()
    {
        //TODO : Requires userId?
        return new RoleDto(Id, 1, Description);
    }
}
