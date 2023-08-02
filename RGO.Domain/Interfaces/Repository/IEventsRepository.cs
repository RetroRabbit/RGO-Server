using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Repository
{
    public interface IEventsRepository
    {
        Task<EventsDto[]> GetAllEvents();
    }
}
