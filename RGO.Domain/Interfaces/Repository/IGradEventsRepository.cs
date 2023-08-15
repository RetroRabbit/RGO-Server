using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Repository;

public interface IGradEventsRepository
{
    /// <summary>
    /// Gets all events
    /// </summary>
    /// <returns>all events</returns>
    Task<GradEventsDto[]> GetAllEvents();
}

