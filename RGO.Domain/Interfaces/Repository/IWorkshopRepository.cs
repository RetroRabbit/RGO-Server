using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Repository
{
     public interface IWorkshopRepository
    {
        Task<WorkshopDto[]> GetAllWorkShops();
    }
}
