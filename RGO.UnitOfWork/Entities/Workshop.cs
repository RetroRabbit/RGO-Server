using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RGO.Models;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Entities;

[Table("Workshop")]
public class Workshop : IModel<WorkshopDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("gradEventId")]
    [ForeignKey("GradEvents")]
    public int EventId { get; set; }

    [Column("presenter")]
    public string Presenter { get; set; }

    [Column("viewable")]
    public bool Viewable { get; set; }

    public virtual GradEvents GradEvents { get; set; }

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
            GradEvents.ToDto(),
            Presenter,
            Viewable
        );
    }
}