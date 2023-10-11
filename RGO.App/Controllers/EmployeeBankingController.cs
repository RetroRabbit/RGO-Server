using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.App.Controllers;

[Route("/employeeBanking/")]
[ApiController]
public class EmployeeBankingController : ControllerBase
{
    private readonly IEmployeeBankingService _employeeBankingService;

    public EmployeeBankingController(IEmployeeBankingService employeeBankingService)
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

    //[Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpPut("updatePending")]
    public async Task<IActionResult> UpdatePending([FromBody] EmployeeBankingDto updateEntry)
    {
        try
        {
            var employee = _employeeBankingService.UpdatePending(updateEntry);
            return Ok("Success");
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}