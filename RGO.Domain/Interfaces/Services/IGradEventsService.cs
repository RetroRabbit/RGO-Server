using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Services;

public interface IGradEventsService
{
    /// <summary>
    /// Gets all events
    /// </summary>
    /// <returns>all events</returns>
    Task<GradEventsDto[]> GetEvents();
}
