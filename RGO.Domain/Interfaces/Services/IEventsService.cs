using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Services
{
    public interface IEventsService
    {
        Task<EventsDto[]> GetEvents();
    }
}
