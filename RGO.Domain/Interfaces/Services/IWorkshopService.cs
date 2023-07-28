using RGO.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Domain.Interfaces.Services
{
    public interface IWorkshopService
    {
        Task<WorkshopDto[]> GetWorkshops();
    }
}
