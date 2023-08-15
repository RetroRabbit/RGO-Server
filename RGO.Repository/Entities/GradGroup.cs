using RGO.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.Repository.Entities;

[Table("GradGroup")]
public class GradGroup
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; }

    public GradGroup() { }

    public GradGroup(GradGroup gradGroup)
    {
        Id = gradGroup.Id;
        Title = gradGroup.Title;
    }

    public GradGroupDto ToDTO()
    {
        return new GradGroupDto(
            Id,
            Title
            );
    }
}
