using RGO.Domain.Models;

namespace RGO.Repository.Entities;

public class Events {
    public int id { get; set; }
    public int groupId { get; set; }
    public string title { get; set; } = null!;
    public string description { get; set; } = null!;
    public int userType { get; set; }
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
    public int eventType { get; set; }

    public Events() { }
    public Events(EventsDto eventsDto)
    {
        id = eventsDto.id;
        groupId = eventsDto.groupId;
        title = eventsDto.title;
        description = eventsDto.description;
        userType = eventsDto.userType;
        startDate = eventsDto.startDate;
        endDate = eventsDto.endDate;
        eventType = eventsDto.eventType;
    }

    public EventsDto ToDto()
    {
        return new EventsDto
        (
            id,
            groupId,
            title,
            description,
            userType,
            startDate,
            endDate,
            eventType

        );
    }
}
    
