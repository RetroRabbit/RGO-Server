using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System.ComponentModel.DataAnnotations;

namespace RGO.Services.Services;

public class ChartService : IChartService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeService _employeeService;
    public ChartService(IUnitOfWork db, IEmployeeService employeeService) 
    {
        _db = db;
        _employeeService = employeeService;
    }

    public  async Task<List<ChartDto>> GetAllCharts() 
    {
        return await _db.Chart.GetAll();
    }

    public async Task<int> GetTotalEmployees()
    {
        try
        {
            var employees = await _employeeService.GetAll();
            return employees.Count;
        }
        catch (Exception)
        {
            return 0;
        }
    }

    public async Task<ChartDto> CreateChart(string dataType, string chartName, string chartType)
    {
        var employees = await _employeeService.GetAll();

        var propertyInfo = typeof(EmployeeDto).GetProperty(dataType);

        if (propertyInfo == null)
        {
            throw new ArgumentException($"Invalid dataType: {dataType}");
        }

        try
        {
            var dataDictionary = employees
                .GroupBy(x => propertyInfo.GetValue(x))
                .ToDictionary(group => group.Key?.ToString() ?? "Unknown", group => group.Count());

            var labels = dataDictionary.Keys.ToList();
            var data = dataDictionary.Values.ToList();

            var chart = new Chart
            {
                Name = chartName,
                Type = chartType,
                Labels = labels,
                Data = data
            };

            return await _db.Chart.Add(chart);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while creating the chart.", ex);
        }
    }

    public async Task<ChartDataDto> GetChartData(string dataType)
    {
        var employees = await _employeeService.GetAll();

        var propertyInfo = typeof(EmployeeDto).GetProperty(dataType);

        if (propertyInfo == null)
        {
            throw new ArgumentException($"Invalid dataType: {dataType}");
        }

        try
        {
            var chartData = employees
                .GroupBy(x => propertyInfo.GetValue(x))
                .ToDictionary(group => group.Key?.ToString() ?? "Unknown", group => group.Count());

            var labels = chartData.Keys.ToList();
            var data = chartData.Values.ToList();

            var chartDataDto = new ChartDataDto
            {
                Labels = labels,
                Data = data
            };

            return chartDataDto;
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while processing the data.", ex);
        }
    }

    public async Task<ChartDto> DeleteChart(int chartId)
    {
        return await _db.Chart.Delete(chartId);
    }

    public async Task<ChartDto> UpdateChart(ChartDto chartDto)
    {

        var charts = await _db.Chart.GetAll();
        var chartData = charts
            .Where(chartData => chartData.Id == chartDto.Id)
            .Select(chartData => chartData)
            .FirstOrDefault();

        if (chartData == null) { throw new Exception("No chart data record found"); }
        var updatedChart = await _db.Chart.Update(new Chart(chartDto));

        return updatedChart;
    }

    public string[] GetColumnsFromTable()
    {
        var entityType = typeof(Employee);

        if (entityType != null)
        { 
            var quantifiableColumnNames = entityType.GetProperties()
                .Where(p => IsQuantifiableType(p.PropertyType))
                .Select(p => p.Name)
                .ToArray();

            if (quantifiableColumnNames == null || quantifiableColumnNames.Length == 0)
            {
                throw new Exception("No quantifiable column names found");
            }

            return quantifiableColumnNames;
        }

        throw new Exception("Employee table not found");
    }

    private bool IsQuantifiableType(Type type)
    {
        return typeof(IConvertible).IsAssignableFrom(type) && type != typeof(string);
    }
}
