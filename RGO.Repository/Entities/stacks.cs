using RGO.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RGO.Repository.Entities;

public class Stacks
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("title")]
    public string Title { get; set; } = null!;

    [Column("description")]
    public string Description { get; set; } = null!;

    [Column("url")]
    public string Url { get; set; } = null!;

    [Column("stackType")]
    public int StackType { get; set; }

    public Stacks()
    {
        
    }

    public Stacks(StacksDto stacks)
    {
        Id = stacks.Id;
        Title = stacks.Title;
        Description = stacks.Description;
        Url = stacks.Url;
        StackType = stacks.StackType;
        
    }
}