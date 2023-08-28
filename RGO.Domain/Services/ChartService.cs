using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

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
        
    public async Task<ChartDto> CreateChart(ChartDto chartDto)
    {
        ChartDto chart= await _db.Chart.Add(new Chart(chartDto));

        return chart;
    }

}
