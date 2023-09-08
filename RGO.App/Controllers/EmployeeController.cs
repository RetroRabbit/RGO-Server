using Microsoft.AspNetCore.Authorization;
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

    [Authorize(Roles = "AdminPolicy")]
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

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [HttpGet("get")]
    public async Task<IActionResult> GetEmployee([FromQuery] string? email)
    {
        try
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            string emailToUse = email ??
                claimsIdentity!.FindFirst(ClaimTypes.Email)!.Value;

            var employee = await _employeeService.GetEmployee(emailToUse);

            return Ok(employee);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
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

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("employees")]
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

    [Authorize(Policy = "AdminPolicy")]
    [HttpGet("employees/count")]
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
