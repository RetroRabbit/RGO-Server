using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork;
using System.Security.Claims;

namespace RR.App.Controllers.HRIS;

[Route("employee-salary")]
[ApiController]
public class EmployeeSalaryDetailsController : ControllerBase
{
    private readonly IEmployeeSalarayDetailsService _employeeSalarayDetailsService;
    public EmployeeSalaryDetailsController(IEmployeeSalarayDetailsService employeeSalarayDetailsService) 
    {
        _employeeSalarayDetailsService = employeeSalarayDetailsService;
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpDelete]
    public async Task<IActionResult> DeleteSalary(int employeeId)
    {
        try
        {
            var deletedEmployeeSalary = await _employeeSalarayDetailsService.DeleteEmployeeSalary(employeeId);
            return Ok(deletedEmployeeSalary);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllEmployeeSalaries()
    {
        try
        {
            var employeeSalaries = await _employeeSalarayDetailsService.GetAllEmployeeSalaries();
            return Ok(employeeSalaries);
        }
        catch (Exception)
        {
            return StatusCode(500, "An error occurred while fetching the employee salaries.");
        }
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet("{employeeId}")]
    public async Task<IActionResult> GetEmployeeSalary(int employeeId)
    {
        try
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var role = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value!;
            if ("SuperAdmin" == role || "Admin" == role || "Talent" == role || "Journey" == role)
            {
                var employeeSalaries = await _employeeSalarayDetailsService.GetEmployeeSalary(employeeId);
                return Ok(employeeSalaries);
            }

            var userId = GlobalVariables.GetUserId();
            if (employeeId == userId)
            {
                var employeeSalaries = await _employeeSalarayDetailsService.GetEmployeeSalary(employeeId);
                return Ok(employeeSalaries);
            }

            return NotFound("User data being accessed does not match user making the request.");
        }
        catch (Exception x)
        {
            return StatusCode(500, "An error occurred while fetching employee salaries.");
        }
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPost()]
    public async Task<IActionResult> AddEmployeeSalary([FromBody] EmployeeSalaryDetailsDto employeeSalaryDetailsDto)
    {
        try
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var role = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value!;
            if ("SuperAdmin" == role || "Admin" == role || "Talent" == role || "Journey" == role)
            {
                var employeeSalaries = await _employeeSalarayDetailsService.SaveEmployeeSalary(employeeSalaryDetailsDto);
                return CreatedAtAction(nameof(AddEmployeeSalary), new { employeeId = employeeSalaries.EmployeeId }, employeeSalaries);
            }

            var userId = GlobalVariables.GetUserId();
            if (employeeSalaryDetailsDto.Id == userId)
            {
                var employeeSalaries = await _employeeSalarayDetailsService.SaveEmployeeSalary(employeeSalaryDetailsDto);
                return CreatedAtAction(nameof(AddEmployeeSalary), new { employeeId = employeeSalaries.EmployeeId }, employeeSalaries);
            }

            return NotFound("User data being accessed does not match user making the request.");
         }
        catch (Exception ex)
        {
            if (ex.Message.Contains("exists"))
                return Problem("Unexceptable", "Unexceptable", 406, "User Salary Exists");

            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPut()]
    public async Task<IActionResult> UpdateSalary([FromBody] EmployeeSalaryDetailsDto employeeSalaryDetailsDto)
    {
        try
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var role = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value!;
            if ("SuperAdmin" == role || "Admin" == role || "Talent" == role || "Journey" == role)
            {
                var updatedEmployeeSalary = await _employeeSalarayDetailsService.UpdateEmployeeSalary(employeeSalaryDetailsDto);
                return Ok(updatedEmployeeSalary);
            }

            var userId = GlobalVariables.GetUserId();
            if (employeeSalaryDetailsDto.Id == userId)
            {
                var updatedEmployeeSalary = await _employeeSalarayDetailsService.UpdateEmployeeSalary(employeeSalaryDetailsDto);
                return Ok(updatedEmployeeSalary);
            }

            return NotFound("User data being accessed does not match user making the request.");
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}