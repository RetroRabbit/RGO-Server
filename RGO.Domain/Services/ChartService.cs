using Microsoft.AspNetCore.Mvc;
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

        Dictionary<string, int> dataDictionary = null;

        switch (dataType)
        {
            case "Gender":
                dataDictionary = employees
                    .GroupBy(x => x.Gender)
                    .ToDictionary(group => group.Key.ToString(), group => group.Count());
                break;
            case "Race":
                dataDictionary = employees
                    .GroupBy(x => x.Race)
                    .ToDictionary(group => group.Key.ToString(), group => group.Count());
                break;
            case "Level":
                dataDictionary = employees
                    .GroupBy(x => x.Level)
                    .ToDictionary(group => group.Key.ToString(), group => group.Count());
                break;
        }

        if (dataDictionary == null)
        {
            return null;
        }

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

    public async Task<ChartDataDto> GetChartData(string dataType)
    {
        var employees = await _employeeService.GetAll();

        Dictionary<string, int> chartData = null;

        switch (dataType)
        {
            case "Gender":
                chartData = employees
                    .GroupBy(x => x.Gender)
                    .ToDictionary(group => group.Key.ToString(), group => group.Count());
                break;
            case "Race":
                chartData = employees
                    .GroupBy(x => x.Race)
                    .ToDictionary(group => group.Key.ToString(), group => group.Count());
                break;
            case "Level":
                chartData = employees
                    .GroupBy(x => x.Level)
                    .ToDictionary(group => group.Key.ToString(), group => group.Count());
                break;
            default:
                return null;
        }

        if (chartData == null)
        {
            return null;
        }

        var labels = chartData.Keys.ToList();
        var data = chartData.Values.ToList();

        var chartDataDto = new ChartDataDto
        {
            Labels = labels,
            Data = data
        };

        return chartDataDto;
    }

}
