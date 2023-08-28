﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;
using System.Security.Claims;

namespace RGO.App.Controllers;

[Route("/employee/")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("add")]
    public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto newEmployee)
    {
        try
        {
            var employee = await _employeeService.SaveEmployee(newEmployee);

            return CreatedAtAction(nameof(AddEmployee), new { email = employee.Email }, employee);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetEmployee()
    {
        try
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            var email = claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(email)) return Unauthorized("Token is missing claim(s)[e.g: email]");

            var employee = await _employeeService.GetEmployee(email);

            return Ok(employee);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDto employee, [FromQuery] string email)
    {
        try
        {
            var updatedEmployee = await _employeeService.UpdateEmployee(employee, email);

            return CreatedAtAction(nameof(UpdateEmployee), new { email = updatedEmployee.Email }, updatedEmployee);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllEmployees()
    {
        try
        {
            var employees = await _employeeService.GetAll();

            return Ok(employees);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("count-all")]
    public async Task<IActionResult> CountAllEmployees()
    {
        try
        {
            var employees = await _employeeService.GetAll();

            return Ok(employees.Count);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
