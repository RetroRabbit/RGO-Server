using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Services.Interfaces;

namespace RGO.App.Controllers;

public class EmployeeBanking : ControllerBase
{
    private readonly IEmployeeBankingService _employeeBankingService;

    public EmployeeBanking(IEmployeeBankingService employeeBankingService)
    {
        _employeeBankingService = employeeBankingService;
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet("getPending")]
    public async Task<IActionResult> FetchPending()
    {
        try
        {
            var pendingEntries = await _employeeBankingService.GetPending();
            return Ok(pendingEntries);
        }catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}