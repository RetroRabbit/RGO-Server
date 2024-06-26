﻿using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IChartService
{
    /// <summary>
    ///     Get all the charts
    /// </summary>
    /// <returns>List<ChartDto></returns>
    Task<List<ChartDto>> GetAllCharts();

    /// <summary>
    ///     Get all the charts for an employee
    /// </summary>
    /// <returns>List<ChartDto></returns>
    Task<List<ChartDto>> GetEmployeeCharts(int employeeId);

    /// <summary>
    ///     Create a chart
    /// </summary>
    /// <param name="dataTypes"></param>
    /// <param name="chartName"></param>
    /// <param name="chartType"></param>
    /// <param name="employeeId"></param>
    /// <returns>ChartDto</returns>
    Task<ChartDto> CreateChart(List<string> dataTypes, List<string> roles, string chartName, string chartType, int employeeId);

    /// <summary>
    ///     Gets data points selected by the user
    /// </summary>
    /// <param name="dataTypes"></param>
    /// <returns>ChartDataDto</returns>
    Task<ChartDataDto> GetChartData(List<string> dataTypes);

    /// <summary>
    ///     Delete a Chart
    /// </summary>
    /// <param name="chartId"></param>
    /// <returns>Chart Data</returns>
    Task<ChartDto> DeleteChart(int chartId);

    /// <summary>
    ///     Update Chart
    /// </summary>
    /// <param name="chartDto"></param>
    /// <returns></returns>
    Task<ChartDto> UpdateChart(ChartDto chartDto);

    /// <summary>
    ///     Returns column names as string from the employee table
    /// </summary>
    /// <returns></returns>
    string[] GetColumnsFromTable();

    /// <summary>
    ///     Returns a CSV file
    /// </summary>
    /// <param name="dataTypes"></param>
    /// <returns>Report CSV File</returns>
    Task<byte[]?> ExportCsvAsync(List<string> dataTypes);
}