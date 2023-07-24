using RGO.Domain.Models;
using System.Reflection.Emit;

namespace RGO.Repository.Entities;

public class Events {
    public int id { get; set; }
    public int groupid { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public int usertype { get; set; }
    public DateTime startdate { get; set; }
    public DateTime enddate { get; set; }
    public int eventtype { get; set; }

    public Events() { }
    public Events(int id, EventsDto eventsDto)
    {
        this.id = id;
    }

    public EventsDto ToDto()
    {
        return new EventsDto
        (
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
    
