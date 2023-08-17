using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RGO.Models;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Entities;

[Table("GradEvents")]
public class GradEvents : IModel<GradEventsDto>
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("gradGroupId")]
    [ForeignKey("GradGroup")]
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

    public GradEvents() { }

    public GradEvents(GradEventsDto gradEventsDto)
    {
        Id = gradEventsDto.Id;
        GroupId = gradEventsDto.GroupId;
        Title = gradEventsDto.Title;
        Description = gradEventsDto.Description;
        UserType = gradEventsDto.UserType;
        StartDate = gradEventsDto.StartDate;
        EndDate = gradEventsDto.EndDate;
        EventType = gradEventsDto.EventType;
    }

    public GradEventsDto ToDto()
    {
        return new GradEventsDto(
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