using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Interfaces.Services;
using RGO.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Domain.Services
{
    public class WorkshopService : IWorkshopService
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
}
