using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using System.Globalization;

namespace RGO.App.Controllers
{
    [Route("/chart/")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly IChartService _chartService;

        public ChartsController(IChartService chartService)
        {
            _chartService = chartService;
        }

        [HttpGet("get")]
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

        [HttpPost("create")]
        public async Task<IActionResult> CreateChart([FromQuery] string dataType,string chartName,string chartType)
        {
            try
            {
                await _chartService.CreateChart(dataType,chartName,chartType);

                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("employees/total")]
        public async Task<IActionResult> GetNumberOfEmployees()
        {
            try
            {
                var numOfEmployees = await _chartService.GetTotalEmployees();
                return Ok(numOfEmployees);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("data")]
        public async Task<IActionResult> GetChartData([FromQuery] string dataType)
        {
            try
            {
                var chartData = await _chartService.GetChartData(dataType);
                return Ok(chartData);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("update")]
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

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteChart([FromQuery] int Id)
        {
            try
            {
                var deletedChart = await _chartService.DeleteChart(Id);
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
        public async Task<IActionResult> ExportCsv([FromQuery] string dataType)
        {
            try
            {
                var csvData = await _chartService.ExportCsvAsync(dataType);

                if (csvData == null || csvData.Length == 0)
                    return NotFound("No data found to export.");

                var fileName = $"Report_{DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture)}.csv";

                return File(csvData, "text/csv", fileName);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
           
        }
    }
}
