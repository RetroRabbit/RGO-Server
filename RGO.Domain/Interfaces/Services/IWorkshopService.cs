using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Services;

public interface IWorkshopService
{
    Task<List<WorkshopDto>> GetWorkshops();
}
