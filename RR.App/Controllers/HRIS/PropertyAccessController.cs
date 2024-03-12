using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Models.Update;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("access")]
public class PropertyAccessController : ControllerBase
{
    private readonly IPropertyAccessService _propertyAccessService;
    private readonly IEmployeeService _employeeService;

    public PropertyAccessController(IPropertyAccessService propertyAccessService,IEmployeeService employeeService)
    {
        _propertyAccessService = propertyAccessService;
        _employeeService = employeeService;
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet("role-access")]
    public async Task<IActionResult> GetPropertiesWithAccess(int employeeId)
    {
        try
        {
            var accessList = await _propertyAccessService.GetAccessListByEmployeeId(employeeId);
            return Ok(accessList);
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
            var employee = await _employeeService.GetEmployee(email);
            return Ok(employee.Id);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}