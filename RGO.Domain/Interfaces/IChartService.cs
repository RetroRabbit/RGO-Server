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
        /// Create a chart
        /// </summary>
        /// <param name="dataType"></param>
        /// <param name="chartName"></param>
        /// <param name="chartType"></param>
        /// <returns></returns>
        Task<ChartDto> CreateChart(string dataType, string chartName, string chartType);

        /// <summary>
        ///Gets data point selected by the user
        /// </summary>
        /// <param name="dataType"></param>
        /// <returns></returns>
        Task<ChartDataDto> GetChartData(string dataType);

        /// <summary>
        /// Delete a Chart 
        /// </summary>
        /// <param name="chartId"></param>
        /// <returns></returns>
        Task<ChartDto> DeleteChart(int chartId);

        /// <summary>
        /// Update Chart
        /// </summary>
        /// <param name="chartDto"></param>
        /// <returns></returns>
        Task<ChartDto> UpdateChart(ChartDto chartDto);
    }
}
