﻿using System.Security.Claims;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("employee-banking")]
[ApiController]
public class EmployeeBankingController : ControllerBase
{
    private readonly AuthorizeIdentity _identity;
    private readonly IEmployeeBankingService _employeeBankingService;

    public EmployeeBankingController(AuthorizeIdentity identity, IEmployeeBankingService employeeBankingService)
    {
        _identity = identity;
        _employeeBankingService = employeeBankingService;
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPost]
    public async Task<IActionResult> AddBankingInfo([FromBody] SimpleEmployeeBankingDto newEntry)
    {
        try
        {
            var BankingDto = new EmployeeBankingDto
            {
                Id = newEntry.Id,
                EmployeeId = newEntry.EmployeeId,
                BankName = newEntry.BankName,
                Branch = newEntry.Branch,
                AccountNo = newEntry.AccountNo,
                AccountType = newEntry.AccountType,
                Status = newEntry.Status,
                DeclineReason = newEntry.DeclineReason,
                File = newEntry.File,
                LastUpdateDate = DateOnly.FromDateTime(DateTime.Now),
                PendingUpdateDate = DateOnly.FromDateTime(DateTime.Now)
            };

            var claimsIdentity = User.Identity as ClaimsIdentity;
            var employeeBankingDto =
                await _employeeBankingService.Save(BankingDto, claimsIdentity!.FindFirst(ClaimTypes.Email)!.Value);
            return Ok(employeeBankingDto);
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

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpDelete]
    public async Task<IActionResult> DeleteBankingInfo([FromQuery] int addressId)
    {
        try
        {
            var deletedBanking = await _employeeBankingService.Delete(addressId);
            return Ok(deletedBanking);
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
        try
        {
            var Bankingdto = new EmployeeBankingDto
            {
                Id = updateEntry.Id,
                EmployeeId = updateEntry.EmployeeId,
                BankName = updateEntry.BankName,
                Branch = updateEntry.Branch,
                AccountNo = updateEntry.AccountNo,
                AccountType = updateEntry.AccountType,
                Status = updateEntry.Status,
                DeclineReason = updateEntry.DeclineReason,
                File = updateEntry.File,
                LastUpdateDate = DateOnly.FromDateTime(DateTime.Now),
                PendingUpdateDate = DateOnly.FromDateTime(DateTime.Now)
            };

            var claimsIdentity = User.Identity as ClaimsIdentity;
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
            if (!string.IsNullOrEmpty(_identity.Email))
            {
                if (_identity.Role is "SuperAdmin" or "Admin")
                {
                    var employeeBanking = await _employeeBankingService.GetBanking(id);
                    return Ok(employeeBanking);
                }

                if (_identity.EmployeeId == id)
                {
                    var employeeBanking = await _employeeBankingService.GetBanking(id);
                    return Ok(employeeBanking);
                }
            }

            return NotFound("Tampering found!");
        }

        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
