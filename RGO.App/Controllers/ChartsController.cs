﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;

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
        public async Task<IActionResult> CreateChart([FromBody] ChartDto newChart)
        {
            try
            {
                await _chartService.CreateChart(newChart);

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
                var numOfEmployees = _chartService.GetTotalEmployees();
                return Ok(numOfEmployees);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
