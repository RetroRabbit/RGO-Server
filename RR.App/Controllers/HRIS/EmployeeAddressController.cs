using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("employee-address")]
[ApiController]
public class EmployeeAddressController : ControllerBase
{
    private readonly IEmployeeAddressService _employeeAddressService;
    private readonly AuthorizeIdentity _identity;

    public EmployeeAddressController(AuthorizeIdentity identity, IEmployeeAddressService employeeAddressService)
    {
        _identity = identity;
        _employeeAddressService = employeeAddressService;
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
            var addresses = await _employeeAddressService.GetAll();
            return Ok(addresses);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet("Employee-Address-By-Id")]
    public async Task<IActionResult> GetEmployeeAddressById(int employeeId)
    {
        if (_identity.IsSupport == false && employeeId != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");

        var employeeAddress = await _employeeAddressService.GetById(employeeId);
        return Ok(employeeAddress);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPost]
    public async Task<IActionResult> SaveEmployeeAddress([FromBody] EmployeeAddressDto address)
    {
        if (_identity.IsSupport == false && address.EmployeeId != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");

        var savedAddress = await _employeeAddressService.Create(address);
            return Ok(savedAddress);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateEmployeeAddress([FromBody] EmployeeAddressDto address)
    {
        if (_identity.IsSupport == false && address.EmployeeId != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");

        var updatedAddress = await _employeeAddressService.Update(address);
            return Ok(updatedAddress);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpDelete]
    public async Task<IActionResult> DeleteEmployeeAddress(int employeeId)
    {
            var deletedAddress = await _employeeAddressService.Delete(employeeId);
            return Ok(deletedAddress);
    }    
}
