using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IWorkshopService
{
    Task<List<WorkshopDto>> GetWorkshops();
}
