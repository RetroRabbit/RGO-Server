using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS
{
    public class DasboardController : ControllerBase
    {
        private readonly AuthorizeIdentity _identity;
        private readonly IDashboardService _dashboardService;

        public DasboardController(AuthorizeIdentity identity, IDashboardService dashboardService)
        {
            _identity = identity;
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
            try
            {
                var employeesCount = await _dashboardService.GenerateDataCardInformation();
                return Ok(employeesCount);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
        [HttpGet("churn-rate")]
        public async Task<IActionResult> GetChurnRate()
        {
            try
            {
                var churnRate = await _dashboardService.CalculateEmployeeChurnRate();
                return Ok(churnRate);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
