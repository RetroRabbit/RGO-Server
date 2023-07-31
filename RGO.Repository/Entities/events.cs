using RGO.Domain.Models;

namespace RGO.Repository.Entities;

public class Events {
    public int id { get; set; }
    public int groupid { get; set; }
    public string title { get; set; } = null!;
    public string description { get; set; } = null!;
    public int usertype { get; set; }
    public DateTime startdate { get; set; }
    public DateTime enddate { get; set; }
    public int eventtype { get; set; }

    public Events() { }
    public Events(EventsDto eventsDto)
    {
        id = eventsDto.id;
        groupid = eventsDto.groupid;
        title = eventsDto.title;
        description = eventsDto.description;
        usertype = eventsDto.userType;
        startdate = eventsDto.startDate;
        enddate = eventsDto.endDate;
        eventtype = eventsDto.eventType;
    }

    public EventsDto ToDto()
    {
        return new EventsDto
        (
            id,
            groupid,
            title,
            description,
            usertype,
            startdate,
            enddate,
            eventtype

        );
    }
}
    
