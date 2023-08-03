using RGO.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.Repository.Entities;

[Table("Workshop")]
public class Workshop
{
    [Key]
    [Column("id")]
    public int Id { get; set; }/*

    [Column("eventId")]
    public int EventId { get; set; }*/

    [Column("presenter")]
    public string Presenter { get; set; } = null!;

    [ForeignKey("eventId")]
    public virtual Events WorshopEvents { get; set; }
    
    public Workshop()
    {
    }

    public Workshop(WorkshopDto workshopDto)
    {
        Id = workshopDto.Id;/*
        EventId = workshopDto.EventId.Id;*/
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