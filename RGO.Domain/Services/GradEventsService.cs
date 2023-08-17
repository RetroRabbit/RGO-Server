using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;

namespace RGO.Services.Services;

public class GradEventsService : IGradEventsService
{
    private readonly IUnitOfWork _db;

    public GradEventsService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<List<GradEventsDto>> GetEvents()
    {
        return await _db.GradEvents.GetAll();
    }
}
