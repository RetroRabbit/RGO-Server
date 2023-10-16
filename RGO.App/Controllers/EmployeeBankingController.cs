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
    [HttpGet("get")]
    public async Task<IActionResult> Get([FromQuery] int status)
    {
        try
        { 
            var entries = await _employeeBankingService.Get(status);
            return Ok(entries);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] EmployeeBankingDto updateEntry)
     {
        if(updateEntry.AccountHolderName.Length == 0)
        {
            return BadRequest("Invalid banking details");
        }
        try
        {
            var employee = await _employeeBankingService.Update(updateEntry);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}