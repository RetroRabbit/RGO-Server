using System.Security.Claims;
using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("employee-banking")]
[ApiController]
public class EmployeeBankingController : ControllerBase
{
    private readonly IEmployeeBankingService _employeeBankingService;

    public EmployeeBankingController(IEmployeeBankingService employeeBankingService)
    {
        _employeeBankingService = employeeBankingService;
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPost]
    public async Task<IActionResult> AddBankingInfo([FromBody] SimpleEmployeeBankingDto newEntry)
    {
        try
        {
            var Bankingdto = new EmployeeBankingDto
                (
                 newEntry.Id,
                 newEntry.EmployeeId,
                 newEntry.BankName,
                 newEntry.Branch,
                 newEntry.AccountNo,
                 newEntry.AccountType,
                 newEntry.AccountHolderName,
                 newEntry.Status,
                 newEntry.DeclineReason,
                 newEntry.File,
                 DateOnly.FromDateTime(DateTime.Now),
                 DateOnly.FromDateTime(DateTime.Now)
                );
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var employee =
                await _employeeBankingService.Save(Bankingdto, claimsIdentity!.FindFirst(ClaimTypes.Email)!.Value);
            return Ok(Bankingdto);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("exists"))
                return Problem("Unexceptable", "Unexceptable", 406, "Details already exists");
            if (ex.Message.Contains("Unauthorized access")) return StatusCode(403, $"Forbidden: {ex.Message}");
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet]
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

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] SimpleEmployeeBankingDto updateEntry)
    {
        if (updateEntry.AccountHolderName.Length == 0)
            return BadRequest("Invalid banking details");

        try
        {
            var Bankingdto = new EmployeeBankingDto
                (
                 updateEntry.Id,
                 updateEntry.EmployeeId,
                 updateEntry.BankName,
                 updateEntry.Branch,
                 updateEntry.AccountNo,
                 updateEntry.AccountType,
                 updateEntry.AccountHolderName,
                 updateEntry.Status,
                 updateEntry.DeclineReason,
                 updateEntry.File,
                 DateOnly.FromDateTime(DateTime.Now),
                 DateOnly.FromDateTime(DateTime.Now)
                );

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var employee =
                await _employeeBankingService.Update(Bankingdto, claimsIdentity!.FindFirst(ClaimTypes.Email)!.Value);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet("details")]
    public async Task<IActionResult> GetBankingDetails([FromQuery] int id)
    {
        try
        {
            var employeeBanking = await _employeeBankingService.GetBanking(id);
            return Ok(employeeBanking);
        }

        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}