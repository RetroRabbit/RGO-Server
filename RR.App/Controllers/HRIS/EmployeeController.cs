using System.Security.Claims;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RR.UnitOfWork;

namespace RR.App.Controllers.HRIS;

[Route("employees")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService, IChartService chartService)
    {
        _employeeService = employeeService;
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPost]
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
    [HttpDelete]
    public async Task<IActionResult> DeleteEmployee([FromQuery] String email)
    {
        try
        {
            var deletedEmployee = await _employeeService.DeleteEmployee(email);
            return Ok(deletedEmployee);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet]
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
    [HttpGet("by-email")]
    public async Task<IActionResult> GetEmployeeByEmail([FromQuery] string? email)
    {
        try
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;

            var emailToUse = email ??
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
    [HttpPut]
    public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDto employee, [FromQuery] string userEmail)
    {
        try
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var role = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value!;
            if ("SuperAdmin" == role || "Admin" == role || "Talent" == role || "Journey" == role)
            {
                var updatedEmployee = await _employeeService.UpdateEmployee(employee, claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value!);
                return CreatedAtAction(nameof(UpdateEmployee), new { email = updatedEmployee.Email }, updatedEmployee);
            }

            var userId = GlobalVariables.GetUserId();
            if (employee.Id == userId)
            {
                var updatedEmployee = await _employeeService.UpdateEmployee(employee, claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value!);
                return CreatedAtAction(nameof(UpdateEmployee), new { email = updatedEmployee.Email }, updatedEmployee);
            }

            return NotFound("User data being accessed does not match user making the request.");
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Unauthorized action"))
                return StatusCode(403, $"Forbidden: {ex.Message}");
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllEmployees()
    {
        try
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var employees = await _employeeService.GetAll(claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value!);

            return Ok(employees);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("count")]
    public async Task<IActionResult> CountAllEmployees()
    {
        try
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var employees = await _employeeService.GetAll(claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value!);

            return Ok(employees.Count);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("card-count")]
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

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("churn-rate")]
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
    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
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

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("filter-employees")]
    public async Task<IActionResult> FilterEmployees(int peopleChampId, int employeetype)
    {
        try
        {
            var employees = await _employeeService.FillerEmployees(peopleChampId, employeetype);

            return Ok(employees);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("id-number")]
    public async Task<IActionResult> CheckIdNumber([FromQuery] string idNumber, [FromQuery] int employeeId)
    {
        try
        {
            var isExisting = await _employeeService.CheckDuplicateIdNumber(idNumber,employeeId);

            return Ok(isExisting);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
