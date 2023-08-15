using RGO.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.Repository.Entities;

[Table("Workshop")]
public class Workshop
{
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("eventId")]
    [ForeignKey("Events")]
    public int EventId { get; set; }
    [Column("presenter")]
    public string Presenter { get; set; }
    [Column("viewable")]
    public bool Viewable { get; set; }
    public virtual GradEvents Events { get; set; }
    public Workshop() { }
    public Workshop(WorkshopDto workshopDto)
    {
        Id = workshopDto.Id;
        EventId = workshopDto.EventId.Id;
        Presenter = workshopDto.Presenter;
        Viewable = workshopDto.Viewable;
    }
    public WorkshopDto ToDto()
    {
        return new WorkshopDto
        (
            Id,
            Events.ToDto(),
            Presenter,
            Viewable
        );
    }
}