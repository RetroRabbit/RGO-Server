using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HRIS.Services.Session;

namespace RR.App.Controllers.HRIS;

[Route("employee-salary")]
[ApiController]
public class EmployeeSalaryDetailsController : ControllerBase
{
    private readonly AuthorizeIdentity _identity;
    private readonly IEmployeeSalarayDetailsService _employeeSalarayDetailsService;

    public EmployeeSalaryDetailsController(AuthorizeIdentity identity, IEmployeeSalarayDetailsService employeeSalarayDetailsService)
    {
        _identity = identity;
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
            if (_identity.Role is "SuperAdmin" or "Admin" or "Talent" or "Journey")
            {
                var employeeSalaries = await _employeeSalarayDetailsService.GetEmployeeSalary(employeeId);
                return Ok(employeeSalaries);
            }

            if (employeeId == _identity.EmployeeId)
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
            if (_identity.Role is "SuperAdmin" or "Admin" or "Talent" or "Journey")
            {
                var employeeSalaries = await _employeeSalarayDetailsService.SaveEmployeeSalary(employeeSalaryDetailsDto);
                return CreatedAtAction(nameof(AddEmployeeSalary), new { employeeId = employeeSalaries.EmployeeId }, employeeSalaries);
            }

            if (employeeSalaryDetailsDto.Id == _identity.EmployeeId)
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
            if (_identity.Role is "SuperAdmin" or "Admin" or "Talent" or "Journey")
            {
                var updatedEmployeeSalary = await _employeeSalarayDetailsService.UpdateEmployeeSalary(employeeSalaryDetailsDto);
                return Ok(updatedEmployeeSalary);
            }

            if (employeeSalaryDetailsDto.Id == _identity.EmployeeId)
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