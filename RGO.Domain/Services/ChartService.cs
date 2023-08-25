using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
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

        public async Task<int> GetTotalEmployeeEmployee()
        {
            return _db.Employee.GetAll().Result.Count();
        }
        
        public async Task<ChartDto> CreateChart(ChartDto chartDto)
        {
            ChartDto chart= await _db.Chart.Add(new Chart(chartDto));

            return chart;
        }

    }
}
