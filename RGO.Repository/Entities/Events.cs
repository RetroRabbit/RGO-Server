using RGO.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RGO.Repository.Entities;

[Table("Events")]
public class Events
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("groupId")]
    [ForeignKey("UserGroup")]
    public int? GroupId { get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("description")]
    public string Description { get; set; }

    [Column("userType")]
    public int UserType { get; set; }

    [Column("startDate")]
    public DateTime StartDate { get; set; }

    [Column("endDate")]
    public DateTime EndDate { get; set; }

    [Column("eventType")]
    public int EventType { get; set; }

    public virtual GradGroup? GradGroup { get; set; }

    public Events() { }

    public Events(EventsDto eventsDto)
    {
        Id = eventsDto.Id;
        GroupId = eventsDto.GroupId;
        Title = eventsDto.Title;
        Description = eventsDto.Description;
        UserType = eventsDto.UserType;
        StartDate = eventsDto.StartDate;
        EndDate = eventsDto.EndDate;
        EventType = eventsDto.EventType;
    }

    public EventsDto ToDto()
    {
        return new EventsDto(
            Id,
            GroupId,
            Title,
            Description,
            UserType,
            StartDate,
            EndDate,
            EventType);
    }
}