using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("Role")]
public class Role : IModel<RoleDto>
{
    public Role()
    {
    }

    public Role(RoleDto roleDto)
    {
        Id = roleDto.Id;
        Description = roleDto.Description;
    }

    [Column("Description")] public string Description { get; set; }

    [Key] [Column("id")] public int Id { get; set; }

    public RoleDto ToDto()
    {
        return new RoleDto { Id=Id, Description=Description};              
    }
}