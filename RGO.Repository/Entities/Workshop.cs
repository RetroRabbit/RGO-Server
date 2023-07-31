using RGO.Domain.Models;

namespace RGO.Repository.Entities;

public class Workshop
{
    public int id { get; set; }
    public int eventId { get; set; } = 0;
    public string presenter { get; set; } = null!;

    public Workshop()
    {
    }

    public Workshop(WorkshopDto workshopDto)
    {
        id = workshopDto.id;
        eventId = workshopDto.eventId.id;
        presenter = workshopDto.presenter;
    }

    public WorkshopDto ToDto(EventsDto eventsDto)
    {
        return new WorkshopDto
        (
            id,
            eventsDto,
            presenter
        );
    }
}