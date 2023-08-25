using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Services.Services
{
    public class ChartService : IChartService
    {
        private readonly IUnitOfWork _db;
        public ChartService(IUnitOfWork db) 
        {
            _db = db;
        }

        public  async Task<List<ChartDto>> GetAllCharts() 
        {
            return await _db.Chart.GetAll();
        }
    }
}
