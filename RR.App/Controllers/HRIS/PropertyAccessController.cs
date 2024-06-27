using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
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
        try
        {
            if (!string.IsNullOrEmpty(_identity.Email))
            {
                if (_identity.Role is "SuperAdmin" or "Admin" or "Talent" or "Journey")
                {
                    var accessList = await _propertyAccessService.GetAccessListByEmployeeId(employeeId);
                    return Ok(accessList);
                }

                if (employeeId == _identity.EmployeeId)
                {
                    var accessList = await _propertyAccessService.GetAccessListByEmployeeId(employeeId);
                    return Ok(accessList);
                }
            }

            return NotFound("Tampering found!");
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllPropertyAccess()
    {
        try
        {
            var accessList = await _propertyAccessService.GetAll();
            return Ok(accessList);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdatePropertyRoleAccess(int propertyId,[FromBody] PropertyAccessLevel propertyAccess)
    {
        try
        {
            await _propertyAccessService.UpdatePropertyAccess(propertyId, propertyAccess);
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [HttpGet("seed-properties")]
    public async Task<IActionResult> Seed()
    {
        try
        {
            await _propertyAccessService.CreatePropertyAccessEntries();
            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet("user-id")]
    public async Task<IActionResult> GetUserId(string email)
    {
        try
        {
            if (!string.IsNullOrEmpty(_identity.Email))
            {
                if (email == _identity.Email)
                {
                    return Ok(_identity.EmployeeId);
                }

                if (_identity.Role is "SuperAdmin" or "Admin" or "Talent" or "Journey")
                {
                    var employee = await _employeeService.GetEmployee(email);
                    return Ok(employee!.Id);
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