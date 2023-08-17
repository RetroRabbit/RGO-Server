using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RGO.Models;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Entities;

[Table("GradGroup")]
public class GradGroup : IModel<GradGroupDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; }

    public GradGroup() { }

    public GradGroup(GradGroupDto gradGroupDto)
    {
        Id = gradGroupDto.Id;
        Title = gradGroupDto.Title;
    }

    public GradGroupDto ToDto()
    {
        return new GradGroupDto(Id, Title);
    }
}
