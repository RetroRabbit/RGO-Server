using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("charts")]
[ApiController]
public partial class ChartsController : ControllerBase
{
    private readonly IChartService _chartService;

    public ChartsController(IChartService chartService)
    {
        _chartService = chartService;
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetAllCharts()
    {
        var chart = await _chartService.GetAllCharts();
        return Ok(chart);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("employee")]
    public async Task<IActionResult> GetEmployeeCharts([FromQuery] int employeeId)
    {
        var charts = await _chartService.GetEmployeeCharts(employeeId);
        return Ok(charts);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPost]
    public async Task<IActionResult> CreateChart([FromQuery] List<string> dataType, [FromQuery] List<string> roles, string chartName, string chartType, int employeeId)
    {
        var response = await _chartService.CreateChart(dataType, roles, chartName, chartType, employeeId);
        return Ok(response);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("data")]
    public async Task<IActionResult> GetChartData([FromQuery] List<string> dataTypes)
    {
        var chartData = await _chartService.GetChartData(dataTypes);
        return Ok(chartData);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateChartData(ChartDto chartDto)
    {
        var saveChart = await _chartService.UpdateChart(chartDto);
        return Ok(saveChart);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpDelete]
    public async Task<IActionResult> DeleteChart([FromQuery] int chartId)
    {
        var deletedChart = await _chartService.DeleteChart(chartId);
        return Ok(deletedChart);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("column")]
    public IActionResult GetColumns()
    {
        var columns = _chartService.GetColumnsFromTable();
        return Ok(columns);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("report/export")]
    public async Task<IActionResult> ExportCsv([FromQuery] List<string> dataTypes)
    {
        var csvData = await _chartService.ExportCsvAsync(dataTypes);
        return File(csvData, "text/csv", "Report.csv");
    }
}