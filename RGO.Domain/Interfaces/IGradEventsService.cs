using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IGradEventsService
{
    /// <summary>
    /// Gets all events
    /// </summary>
    /// <returns>all events</returns>
    Task<List<GradEventsDto>> GetEvents();
}
