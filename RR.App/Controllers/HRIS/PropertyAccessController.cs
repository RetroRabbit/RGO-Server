using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("access")]
public class PropertyAccessController : ControllerBase
{
    private readonly AuthorizeIdentity _identity;
    private readonly IPropertyAccessService _propertyAccessService;
    private readonly IEmployeeService _employeeService;

    public PropertyAccessController(AuthorizeIdentity identity, IPropertyAccessService propertyAccessService, IEmployeeService employeeService)
    {
        _identity = identity;
        _propertyAccessService = propertyAccessService;
        _employeeService = employeeService;
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet("role-access")]
    public async Task<IActionResult> GetPropertiesWithAccess(int employeeId)
    {
        if (_identity.Role is not ("SuperAdmin" or "Admin" or "Talent" or "Journey") && employeeId != _identity.EmployeeId)
            throw new CustomException("Error retrieving employee.");

        var accessList = await _propertyAccessService.GetAccessListByEmployeeId(employeeId);
        return Ok(accessList);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllPropertyAccess()
    {
        var accessList = await _propertyAccessService.GetAll();
        return Ok(accessList);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdatePropertyRoleAccess(int propertyId, [FromBody] PropertyAccessLevel propertyAccess)
    {
        await _propertyAccessService.UpdatePropertyAccess(propertyId, propertyAccess);
        return Ok();
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [HttpGet("seed-properties")]
    public async Task<IActionResult> Seed()
    {
        await _propertyAccessService.CreatePropertyAccessEntries();
        return Ok();
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet("user-id")]
    public async Task<IActionResult> GetUserId(string email)
    {
        if (string.IsNullOrEmpty(_identity.Email))
            throw new CustomException("Error retrieving employee.");

        if (email == _identity.Email)
            return Ok(_identity.EmployeeId);

        if (_identity.Role is not ("SuperAdmin" or "Admin" or "Talent" or "Journey"))
            throw new CustomException("Error retrieving employee.");

        var employee = await _employeeService.GetEmployee(email);
        return employee == null
            ? throw new CustomException("Error retrieving employee.")
            : Ok(employee.Id);
    }
}