using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RGO.Models;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Entities;

[Table("Skill")]
public class Skill : IModel<SkillDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("userId")]
    [ForeignKey("User")]
    public int UserId { get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("description")]
    public string Description { get; set; }

    public virtual User User { get; set; }

    public Skill() { }

    public Skill(SkillDto skill)
    {
        Id = skill.Id;
        UserId = skill.UserId;
        Title = skill.Title;
        Description = skill.Description;
    }

    public SkillDto ToDto()
    {
        return new SkillDto(
            Id,
            UserId,
            Title,
            Description);
    }
}

