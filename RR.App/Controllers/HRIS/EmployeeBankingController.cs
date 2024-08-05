using System.Security.Claims;
using HRIS.Models.Employee.Commons;
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
        var employeeBankingDto =
            await _employeeBankingService.Create(newEntry);
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
    public async Task<IActionResult> DeleteBankingInfo([FromQuery] int id)
    {
        var deletedBanking = await _employeeBankingService.Delete(id);
        return Ok(deletedBanking);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] EmployeeBankingDto updateEntry)
    {
        await _employeeBankingService.Update(updateEntry);
        return Ok();
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet("details")]
    public async Task<IActionResult> GetBankingDetails([FromQuery] int id)
    {
        if (_identity.Role is not ("SuperAdmin" or "Admin") && _identity.EmployeeId != id)
            throw new CustomException("Unauthorized Access");

        var employeeBanking = await _employeeBankingService.GetBankingById(id);
        return Ok(employeeBanking);
    }
}
