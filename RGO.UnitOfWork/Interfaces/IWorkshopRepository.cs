using RGO.Models;
using RGO.UnitOfWork.Entities;

namespace RGO.UnitOfWork.Interfaces;

public interface IWorkshopRepository : IRepository<Workshop, WorkshopDto>
{
    Task<List<WorkshopDto>> GetAllWorkShops();
}
