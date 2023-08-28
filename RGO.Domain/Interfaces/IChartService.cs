﻿using RGO.Models;
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





    }
}
