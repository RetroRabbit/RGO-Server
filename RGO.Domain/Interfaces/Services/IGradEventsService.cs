using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Services;

public interface IGradEventsService
{
    Task<GradEventsDto[]> GetEvents();
}
