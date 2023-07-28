using RGO.Domain.Models;
using System.Text.RegularExpressions;

namespace RGO.Repository.Entities;

public class Workshop {
    public int id { get; set; }
    public int eventId { get; set; }
    public string presenter { get; set; }

    public WorkshopDto ToDto(EventsDto workshopEvent)
    {
        return new WorkshopDto
        (
            workshopEvent,
            presenter
        );
    }
}