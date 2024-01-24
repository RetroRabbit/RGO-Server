using Microsoft.AspNetCore.Authorization;
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

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
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
            {
                return Problem("Unexceptable", "Unexceptable", 406, "User Exists");
            }
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
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
    public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDto employee)
    {
        try
        {
            var updatedEmployee = await _employeeService.UpdateEmployee(employee, employee.Email);

            return CreatedAtAction(nameof(UpdateEmployee), new { email = updatedEmployee.Email }, updatedEmployee);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
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

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
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
            var employeesCount = await _chartService.GetEmployeesCount();
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
            var churnRate = await _chartService.CalculateChurnRate();
            return Ok(churnRate);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

}
