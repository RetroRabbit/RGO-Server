using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RR.App.Controllers.HRIS;

[Route("employees")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly AuthorizeIdentity _identity;
    private readonly IEmployeeService _employeeService;

    public EmployeeController(AuthorizeIdentity identity, IEmployeeService employeeService)
    {
        _identity = identity;
        _employeeService = employeeService;
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPost]
    public async Task<IActionResult> AddEmployee([FromBody] EmployeeDto newEmployee)
    {
        var employee = await _employeeService.CreateEmployee(newEmployee);
        return CreatedAtAction(nameof(AddEmployee), new { email = employee.Email }, employee);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeProfileDto employee)
    {
        if (!_identity.IsSupport && employee.Id != _identity.EmployeeId)
            throw new CustomException("Unauthorized action.");
        var updatedEmployee = await _employeeService.UpdateEmployee(employee);
        return CreatedAtAction(nameof(UpdateEmployee), new { email = updatedEmployee.Email }, updatedEmployee);
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
   
    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetEmployeeProfile([FromQuery] string identifier)
    {
        var simpleProfile = await _employeeService.GetEmployeeProfile(identifier);
        return Ok(simpleProfile);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllEmployeeProfiles()
    {
        var employees = await _employeeService.GetAllEmployeeProfiles();
        return Ok(employees);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("filter-employees")]
    public async Task<IActionResult> FilterEmployees(int peopleChampId, int employeetype, bool activeStatus = true)
    {
        var employees = await _employeeService.FilterEmployees(peopleChampId, employeetype, activeStatus);
        return Ok(employees);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet("id-number")]
    public async Task<IActionResult> CheckIdNumber([FromQuery] string idNumber, [FromQuery] int employeeId)
    {
        if (_identity.IsSupport == false && employeeId != _identity.EmployeeId)
            throw new CustomException("No permission or user id already exists.");
        var isExisting = await _employeeService.CheckDuplicateIdNumber(idNumber, employeeId, true);
        return Ok(isExisting);
    }
}