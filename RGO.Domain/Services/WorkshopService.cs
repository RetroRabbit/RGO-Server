using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;

namespace RGO.Services.Services;

public class WorkshopService : IWorkshopService
{
    private readonly IUnitOfWork _db;

    public WorkshopService(IUnitOfWork db)
    {
        _db = db;
    }

    public Task<List<WorkshopDto>> GetWorkshops()
    {
        return _db.Workshop.GetAllWorkShops();    
    }
}
