using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using System.Globalization;

namespace RGO.App.Controllers
{
    [Route("charts")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly IChartService _chartService;

        public ChartsController(IChartService chartService)
        {
            _chartService = chartService;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllCharts()
        {
            try
            {
                var chart = await _chartService.GetAllCharts();
                return Ok(chart);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost()]
        public async Task<IActionResult> CreateChart([FromQuery] List<string> dataType, [FromQuery] List<string> roles, string chartName,string chartType)
        {
            try
            {
                await _chartService.CreateChart(dataType, roles, chartName,chartType);

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

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

        [HttpPut()]
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

        [HttpDelete()]
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

        [HttpGet("column")]
        public IActionResult GetColumns()
        {
            try
            {
                var columns = _chartService.GetColumnsFromTable();
                return Ok(columns);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("report/export")]
        public async Task<IActionResult> ExportCsv([FromQuery] List<string> dataTypes)
        {
            try
            {
                var csvData = await _chartService.ExportCsvAsync(dataTypes);

                if (csvData == null || csvData.Length == 0)
                    return NotFound("No data found to export.");

                var fileName = $"Report.csv";

                return File(csvData, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
           
        }
    }
}
