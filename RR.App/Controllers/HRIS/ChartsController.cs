using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

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
        try
        {
            var chart = await _chartService.GetAllCharts();

            for(int i = 0; i < chart.Count; i++)
            {
                for(int j = 0; j < chart[i].DataTypes!.Count; j++)
                {
                    chart[i].DataTypes![j] = CapitalLetters().Replace(chart[i].DataTypes![j], "$1 $2");
                }
            }

            return Ok(chart);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("employee")]
    public async Task<IActionResult> GetEmployeeCharts([FromQuery] int employeeId)
    {
        try
        {
            var charts = await _chartService.GetEmployeeCharts(employeeId);

            for (int i = 0; i < charts.Count; i++)
            {
                for (int j = 0; j < charts[i].DataTypes!.Count; j++)
                {
                    charts[i].DataTypes![j] = CapitalLetters().Replace(charts[i].DataTypes![j], "$1 $2");
                }
            }

            return Ok(charts);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPost]
    public async Task<IActionResult> CreateChart([FromQuery] List<string> dataType, [FromQuery] List<string> roles,
                                                 string chartName, string chartType, int employeeId)
    {
        try
        {
            var response = await _chartService.CreateChart(dataType, roles, chartName, chartType, employeeId);

            return Ok(response);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("data")]
    public async Task<IActionResult> GetChartData([FromQuery] List<string> dataTypes)
    {
        try
        {
            var chartData = await _chartService.GetChartData(dataTypes);
            return Ok(chartData);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateChartData(ChartDto chartDto)
    {
        try
        {
            var saveChart = await _chartService.UpdateChart(chartDto);
            if (saveChart == null) throw new Exception("Chart data not updated");

            return Ok(saveChart);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpDelete]
    public async Task<IActionResult> DeleteChart([FromQuery] int chartId)
    {
        try
        {
            var deletedChart = await _chartService.DeleteChart(chartId);
            return Ok(deletedChart);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("column")]
    public IActionResult GetColumns()
    {
        try
        {
            var columns = _chartService.GetColumnsFromTable();

            for(int i = 0; i < columns.Length;  i++)
            {
                columns[i] = CapitalLetters().Replace(columns[i], "$1 $2");
            }

            return Ok(columns);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("report/export")]
    public async Task<IActionResult> ExportCsv([FromQuery] List<string> dataTypes)
    {
        try
        {
            var csvData = await _chartService.ExportCsvAsync(dataTypes);

            if (csvData == null || csvData.Length == 0)
                return NotFound("No data found to export.");

            var fileName = "Report.csv";

            return File(csvData, "text/csv", fileName);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex CapitalLetters();
}