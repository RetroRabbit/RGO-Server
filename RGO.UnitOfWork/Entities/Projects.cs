using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RGO.Models;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Entities;

[Table("Projects")]
public class Projects : IModel<ProjectsDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("userId")]
    [ForeignKey("User")]
    public int UserId { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("role")]
    public string Role { get; set; }

    public virtual User User { get; set; }

    public Projects() { }

    public Projects(ProjectsDto projects)
    {
        Id = projects.Id;
        UserId = projects.UserId;
        Name = projects.Name;
        Description = projects.Description;
        Role = projects.Role;
    }

    public ProjectsDto ToDto()
    {
        return new ProjectsDto(
            Id,
            UserId,
            Name,
            Description,
            Role);
    }
}
