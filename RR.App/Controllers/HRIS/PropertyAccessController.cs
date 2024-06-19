﻿using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RR.UnitOfWork;
using System.Security.Claims;

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
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var accessTokenEmail = claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value;
            var role = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value;

            if (!string.IsNullOrEmpty(accessTokenEmail))
            {
                if ("SuperAdmin" == role || "Admin" == role || "Talent" == role || "Journey" == role)
                {
                    var accessList = await _propertyAccessService.GetAccessListByEmployeeId(employeeId);
                    return Ok(accessList);
                }

                var userId = GlobalVariables.GetUserId();

                if (userId == employeeId)
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
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var accessTokenEmail = claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value;
            var role = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value;

            if (!string.IsNullOrEmpty(accessTokenEmail))
            {
                if (email == accessTokenEmail)
                {
                    var employee = await _employeeService.GetEmployee(email);
                    //TODO: find a better way to add auth0 user id to the db, perhaps employee id and auth id should be the same?
                    GlobalVariables.SetUserId(employee!.Id);
                    return Ok(employee!.Id);
                }

                if ("SuperAdmin" == role || "Admin" == role || "Talent" == role || "Journey" == role)
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