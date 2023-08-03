using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Interfaces.Services;
using RGO.Domain.Models;

namespace RGO.Domain.Services;

public class WorkshopService : IWorkshopService
{
    IWorkshopRepository _workshopRepository;
    public WorkshopService(IWorkshopRepository repository) // add as a parameter
    {
        IWorkshopRepository _workshopRepository;
        public WorkshopService(IWorkshopRepository repository)
        {
           _workshopRepository = repository;
        }

    public Task<WorkshopDto[]> GetWorkshops()
    {
        return _workshopRepository.GetAllWorkShops();
        
    }
}
