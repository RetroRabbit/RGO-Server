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
        /// <summary>
        /// Get all the charts
        /// </summary>
        /// <returns>List<ChartDto></returns>
        Task<List<ChartDto>> GetAllCharts();

        /// <summary>
        /// Gets the count of all employees
        /// </summary>
        /// <returns>int</returns>
        Task<int> GetTotalEmployees();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chartDto"></param>
        /// <returns></returns>
        Task<ChartDto> CreateChart(ChartDto chartDto);
       



    }
}
