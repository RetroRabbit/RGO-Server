using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Repository;

public interface IGradEventsRepository
{
    Task<GradEventsDto[]> GetAllEvents();
}

