using System.Security.Claims;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
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
    public async Task<IActionResult> AddBankingInfo([FromBody] EmployeeBankingDto newEntry)
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        var employeeBankingDto =
            await _employeeBankingService.Save(newEntry, claimsIdentity!.FindFirst(ClaimTypes.Email)!.Value);
        return Ok(employeeBankingDto);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int status)
    {
        var entries = await _employeeBankingService.Get(status);
        return Ok(entries);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpDelete]
    public async Task<IActionResult> DeleteBankingInfo([FromQuery] int addressId)
    {
        var deletedBanking = await _employeeBankingService.Delete(addressId);
        return Ok(deletedBanking);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] EmployeeBankingDto updateEntry)
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        await _employeeBankingService.Update(updateEntry, claimsIdentity!.FindFirst(ClaimTypes.Email)!.Value);
        return Ok();
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet("details")]
    public async Task<IActionResult> GetBankingDetails([FromQuery] int id)
    {
        if (_identity.Role is not ("SuperAdmin" or "Admin") && _identity.EmployeeId != id)
            throw new CustomException("Unauthorized Action");

        var employeeBanking = await _employeeBankingService.GetBanking(id);
        return Ok(employeeBanking);
    }
}
