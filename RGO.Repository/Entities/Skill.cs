using RGO.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.Repository.Entities;

public class Skill
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("userId")]
    public int UserId { get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [ForeignKey("userId")]
    public virtual List<User> Skills { get; set; }

    public Skill()
    {

    }

    public Skill(SkillDto skill)
    {
        Id = skill.Id;
        UserId = skill.UserId;
        Title = skill.Title;
        Description = skill.Description;
    }

    public SkillDto ToDTO()
    {
        return new SkillDto 
        (
            Id,
            UserId,
            Title,
            Description
        );
    }


}

