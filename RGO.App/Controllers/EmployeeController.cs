using Microsoft.AspNetCore.Mvc;
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
}
