﻿using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("employee-types")]
[ApiController]
public class EmployeeTypeController : ControllerBase
{
    private readonly IEmployeeTypeService _employeeTypeService;

    public EmployeeTypeController(IEmployeeTypeService employeeTypeService)
    {
        _employeeTypeService = employeeTypeService;
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetAllEmployeeTypes()
    {
        try
        {
            var employeeTypes = await _employeeTypeService.GetAllEmployeeType();

            return Ok(employeeTypes);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}