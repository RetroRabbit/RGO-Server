using RGO.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.Repository.Entities;

[Table("Projects")]
public class Projects
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
    public ProjectsDto ToDTO()
    {
        return new ProjectsDto(
            Id,
            UserId,
            Name,
            Description,
            Role);
    }
}
