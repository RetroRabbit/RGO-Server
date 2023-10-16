using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.App.Controllers;

[Route("/employeebanking/")]
[ApiController]
public class EmployeeBankingController : ControllerBase
{
    private readonly IEmployeeBankingService _employeeBankingService;

    public EmployeeBankingController(IEmployeeBankingService employeeBankingService)
    {
        _employeeBankingService = employeeBankingService;
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet("pending")]
    public async Task<IActionResult> FetchPending()
    {
        try
        {
            var pendingEntries = await _employeeBankingService.GetPending();
            return Ok(pendingEntries);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpPut("pending")]
    public async Task<IActionResult> UpdatePending([FromBody] EmployeeBankingDto updateEntry)
     {
        if(updateEntry.AccountHolderName.Length == 0)
        {
            return BadRequest("Invalid banking details");
        }
        try
        {
            var employee = await _employeeBankingService.UpdatePending(updateEntry);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}