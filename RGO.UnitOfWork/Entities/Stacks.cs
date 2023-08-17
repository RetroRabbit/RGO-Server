using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RGO.Models;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Entities;

[Table("Stacks")]
public class Stacks : IModel<StacksDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("title")]
    public string Title { get; set; }
    [Column("description")]
    public string Description { get; set; }
    [Column("url")]
    public string Url { get; set; }
    [Column("stackType")]
    public int StackType { get; set; }
    public Stacks() { }
    public Stacks(StacksDto stacks)
    {
        Id = stacks.Id;
        Title = stacks.Title;
        Description = stacks.Description;
        Url = stacks.Url;
        StackType = stacks.StackType;
    }
    public StacksDto ToDto()
    {
        return new StacksDto(
            Id,
            Title,
            Description,
            Url,
            StackType);
    }
}