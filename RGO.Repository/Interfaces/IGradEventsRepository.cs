using RGO.Models;

namespace RGO.Repository.Interfaces;

public interface IGradEventsRepository
{
    /// <summary>
    /// Gets all events
    /// </summary>
    /// <returns>all events</returns>
    Task<GradEventsDto[]> GetAllEvents();
}

