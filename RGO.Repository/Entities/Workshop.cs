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

    [Column("presenter")]
    public string Presenter { get; set; }
    [Column("viewable")]
    public bool Viewable { get; set; }

    [ForeignKey("eventId")]
    public virtual Events WorshopEvents { get; set; }
    
    public Workshop()
    {
    }

    public Workshop(WorkshopDto workshopDto)
    {
        Id = workshopDto.Id;
        Presenter = workshopDto.Presenter;
    }

    public WorkshopDto ToDto(EventsDto EventsDto)
    {
        return new WorkshopDto
        (
            Id,
            EventsDto,
            Presenter
        );
    }
}