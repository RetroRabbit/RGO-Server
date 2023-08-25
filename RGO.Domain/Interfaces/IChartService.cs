using RGO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGO.Services.Interfaces
{
    public interface IChartService
    {
        Task<List<ChartDto>> GetAllCharts();
        Task<ChartDto> CreateChart(ChartDto chartDto);
    }
}
