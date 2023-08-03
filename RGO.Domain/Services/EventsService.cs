using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Interfaces.Services;
using RGO.Domain.Models;

namespace RGO.Domain.Services;

public class EventsService : IEventsService
{
    private readonly IEventsRepository _eventsrepository;

    public EventsService(IEventsRepository eventsRepository)
    {
        _eventsrepository = eventsRepository;
    }

    public async Task<EventsDto[]> GetEvents()
    {
        return await _eventsrepository.GetAllEvents();
    }
}
