using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.Services.Services;
using System.Security.Claims;

namespace RGO.App.Controllers;

[Route("/employee/")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IChartService _chartService;

    public EmployeeController(IEmployeeService employeeService, IChartService chartService)
    {
        _employeeService = employeeService;
        _chartService = chartService;
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
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
            if (ex.Message.Contains("exists"))
                return Problem("Unexceptable", "Unexceptable", 406, "User Exists");
            
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("id")]
    public async Task<IActionResult> GetEmployeeById([FromQuery] int id)
    {
        try
        {
            var employee = await _employeeService.GetEmployeeById(id);

            return Ok(employee);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
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


    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDto employee, [FromQuery] string userEmail)
    {
        try
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var updatedEmployee = await _employeeService.UpdateEmployee(employee, claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value);

            return CreatedAtAction(nameof(UpdateEmployee), new { email = updatedEmployee.Email }, updatedEmployee);
        }
        catch (Exception ex)
        {
            if(ex.Message.Contains("Unauthorized action"))
                return StatusCode(403, $"Forbidden: {ex.Message}");
            else
                return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("employees")]
    public async Task<IActionResult> GetAllEmployees()
    {
        try
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var employees = await _employeeService.GetAll(claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value);

            return Ok(employees);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("employees/count")]
    public async Task<IActionResult> CountAllEmployees()
    {
        try
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var employees = await _employeeService.GetAll(claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value);

            return Ok(employees.Count);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("employees/filterbytype")]
    public async Task<IActionResult> FilterByType(string type)
    {
        try
        {
            var employees= await _employeeService.GetEmployeesByType(type);

            return Ok(employees);
            
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
            
        }
    }

    [HttpGet("employees/data/count")]
    public async Task<IActionResult> GetEmployeesCount()
    {
        try
        {
            var employeesCount = await _employeeService.GenerateDataCardInformation();
            return Ok(employeesCount);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("employees/churnrate")]
    public async Task<IActionResult> GetChurnRate()
    {
        try
        {
            var churnRate = await _employeeService.CalculateEmployeeChurnRate();
            return Ok(churnRate);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("simple-profile")]
    public async Task<IActionResult> GetSimpleEmployee([FromQuery] string employeeEmail)
    {
        try
        {
            var simpleProfile = await _employeeService.GetSimpleProfile(employeeEmail);

            return Ok(simpleProfile);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
