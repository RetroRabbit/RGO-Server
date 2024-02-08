using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.App.Controllers;

[Route("employee-banking")]
[ApiController]
public class EmployeeBankingController : ControllerBase
{
    private readonly IEmployeeBankingService _employeeBankingService;

    public EmployeeBankingController(IEmployeeBankingService employeeBankingService)
    {
        _employeeBankingService = employeeBankingService;
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpPost()]
    public async Task<IActionResult> AddBankingInfo([FromBody] SimpleEmployeeBankingDto newEntry)
    {
        try
        {
            EmployeeBankingDto Bankingdto = new EmployeeBankingDto
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
            var employee = await _employeeBankingService.Save(Bankingdto);
            return Ok(Bankingdto);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("exists"))
            {
                return Problem("Unexceptable", "Unexceptable", 406, "Details already exists");
            }
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet()]
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
    [HttpPut()]
    public async Task<IActionResult> Update([FromBody] SimpleEmployeeBankingDto updateEntry)
     {
        if(updateEntry.AccountHolderName.Length == 0)
        {
            return BadRequest("Invalid banking details");
        }
        try
        {
            EmployeeBankingDto Bankingdto = new EmployeeBankingDto
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
            var employee = await _employeeBankingService.Update(Bankingdto);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet()]
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