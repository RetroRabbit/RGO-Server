using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Interfaces.Services;
using RGO.Domain.Models;

namespace RGO.Domain.Services;

public class EventsService : IEventsService
{
    private IEventsRepository _eventsrepository;

    public EventsService(IEventsRepository eventsRepository) 
    {
        private IEventsRepository _eventsrepository;

        public EventsService(IEventsRepository eventsRepository) 
        {
            _eventsrepository = eventsRepository;
        }
        public Task<EventsDto[]> GetEvents()
        {
            return _eventsrepository.GetAllEvents();
        }

    }
    public Task<EventsDto[]> GetEvents()
    {
        return _eventsrepository.GetAllEventDtos();
    }

}
