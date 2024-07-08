using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("dashboard")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("growth-rate")]
    public async Task<IActionResult> CalculateEmployeeGrowthRate()
    {
        var growthRate = await _dashboardService.CalculateEmployeeGrowthRate();
        return Ok(growthRate);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("card-count")]
    public async Task<IActionResult> GetEmployeesCount()
    {
        var employeesCount = await _dashboardService.GenerateDataCardInformation();
        return Ok(employeesCount);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("churn-rate")]
    public async Task<IActionResult> GetChurnRate()
    {
        var churnRate = await _dashboardService.CalculateEmployeeChurnRate();
        return Ok(churnRate);
    }
}
