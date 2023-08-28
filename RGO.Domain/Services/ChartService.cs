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

    public async Task<ChartDto> CreateChart(string dataType,string chartName,string chartType)
    {
        if (dataType == "Gender")
        {
            var employess = await _employeeService.GetAll();
            var genderCounts = employess.GroupBy(x => x.Gender).ToList();

            var labels = genderCounts.Select(group => group.Key.ToString()).ToList();
            var data = genderCounts.Select(group => group.Count()).ToList();

            var chart = new Chart
            { 
                Name= chartName,
                Type= chartType,
                Labels= labels,
                Data= data
            };

            return await _db.Chart.Add(chart);
        }
        else
        {
            return null;
        }
    }

}
