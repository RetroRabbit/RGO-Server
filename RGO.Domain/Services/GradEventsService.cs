using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Interfaces.Services;
using RGO.Domain.Models;

namespace RGO.Domain.Services;

public class GradEventsService : IGradEventsService
{
    private readonly IGradEventsRepository _gradEventsrepository;

    public GradEventsService(IGradEventsRepository gradEventsRepository)
    {
        _gradEventsrepository = gradEventsRepository;
    }

    public async Task<GradEventsDto[]> GetEvents()
    {
        return await _gradEventsrepository.GetAllEvents();
    }
}
