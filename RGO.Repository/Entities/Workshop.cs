using RGO.Domain.Models;

namespace RGO.Repository.Entities;

public class Workshop
{
    public int id { get; set; }
    public int eventId { get; set; }
    public string presenter { get; set; } = null!;

    public Workshop()
    {
    }

    public Workshop(WorkshopDto workshopDto)
    {
        id = workshopDto.id;
        eventId = workshopDto.eventId;
        presenter = workshopDto.presenter;
    }

    public WorkshopDto ToDto()
    {
        return new WorkshopDto
        (
            id,
            eventId,
            presenter
        );
    }
}