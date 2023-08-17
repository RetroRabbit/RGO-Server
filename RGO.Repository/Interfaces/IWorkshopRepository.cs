using RGO.Models;

namespace RGO.Repository.Interfaces;

public interface IWorkshopRepository
{
    Task<List<WorkshopDto>> GetAllWorkShops();
}
