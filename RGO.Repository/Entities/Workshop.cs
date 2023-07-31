using RGO.Domain.Models;

namespace RGO.Repository.Entities;

public class Workshop
{
    public int id { get; set; }
    public Events eventId { get; set; } = null!;
    public string presenter { get; set; } = null!;

    public Workshop()
    {
    }

    public Workshop(WorkshopDto workshopDto)
    {
        id = workshopDto.id;
        eventId = new Events(workshopDto.eventId);
        presenter = workshopDto.presenter;
    }

    public WorkshopDto ToDto()
    {
        return new WorkshopDto
        (
            id,
            eventId.ToDto(),
            presenter
        );
    }
}