using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Interfaces.Services;
using RGO.Domain.Models;

namespace RGO.Domain.Services;

public class WorkshopService : IWorkshopService
{
    IWorkshopRepository _workshopRepository;
    public WorkshopService(IWorkshopRepository repository)
    {
        _workshopRepository = repository;
    }
    public Task<List<WorkshopDto>> GetWorkshops()
    {
        return _workshopRepository.GetAllWorkShops();    
    }
}
