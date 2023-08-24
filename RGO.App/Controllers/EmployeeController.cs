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

            return Ok(updatedEmployee);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
