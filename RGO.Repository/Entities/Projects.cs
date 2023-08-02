using RGO.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.Repository.Entities;

public class Projects
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("role")]
    public string Role { get; set; }

    [Column("userId")]
    public int UserId { get; set; }

    [ForeignKey("userId")]
    public virtual List<User> UserProjects { get; set; }

    public Projects()
    {

    }

    public Projects(ProjectsDto projects)
    {
        Id = projects.Id;
        Name = projects.Name;
        Description = projects.Description;
        Role = projects.Role;
    }

    public ProjectsDto ToDTO()
    {
        return new ProjectsDto(
            Id,
            Name,
            Description,
            Role
            );
    }
}
