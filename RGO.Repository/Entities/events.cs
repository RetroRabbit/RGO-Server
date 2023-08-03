using RGO.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.Repository.Entities;

[Table("Events")]
public class Events {
    [Key]
    [Column("id")]
    public int Id { get; set; }/*

    [Column("groupId")]
    public int GroupId { get; set; }*/

    [Column("title")]
    public string Title { get; set; } = null!;

    [Column("description")]
    public string Description { get; set; } = null!;
  
    [Column("userType")]
    public int UserType { get; set; }
    
    [Column("startDate")]
    public DateTime StartDate { get; set; }
    
    [Column("endDate")]
    public DateTime EndDate { get; set; }
   
    [Column("eventType")]
    public int EventType { get; set; }

    [ForeignKey("groupId")]
    public virtual UserGroup GroupEvents { get; set; }


    public Events() { }
    public Events(EventsDto eventsDto)
    {
        Id = eventsDto.Id;/*
        GroupId = eventsDto.GroupId;*/
        Title = eventsDto.Title;
        Description = eventsDto.Description;
        UserType = eventsDto.UserType;
        StartDate = eventsDto.StartDate;
        EndDate = eventsDto.EndDate;
        EventType = eventsDto.EventType;
    }

    public EventsDto ToDto()
    {
        return new EventsDto
        (
            Id,
            GroupEvents.Id,
            Title,
            Description,
            UserType,
            StartDate,
            EndDate,
            EventType
        );
    }
}
    
