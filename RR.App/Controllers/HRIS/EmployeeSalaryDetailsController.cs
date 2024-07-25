using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HRIS.Services.Session;
using HRIS.Services.Services;

namespace RR.App.Controllers.HRIS;

[Route("employee-salary")]
[ApiController]
public class EmployeeSalaryDetailsController : ControllerBase
{
    private readonly AuthorizeIdentity _identity;
    private readonly IEmployeeSalaryDetailsService _employeeSalaryDetailsService;

    public EmployeeSalaryDetailsController(AuthorizeIdentity identity, IEmployeeSalaryDetailsService employeeSalaryDetailsService)
    {
        _identity = identity;
        _employeeSalaryDetailsService = employeeSalaryDetailsService;
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpDelete]
    public async Task<IActionResult> DeleteSalary(int employeeId)
    {
        var deletedEmployeeSalary = await _employeeSalaryDetailsService.DeleteEmployeeSalary(employeeId);
        return Ok(deletedEmployeeSalary);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllEmployeeSalaries()
    {
        var employeeSalaries = await _employeeSalaryDetailsService.GetAllEmployeeSalaries();
        return Ok(employeeSalaries);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet("{employeeId}")]
    public async Task<IActionResult> GetEmployeeSalary(int employeeId)
    {
        if (_identity.Role is not ("SuperAdmin" or "Admin" or "Talent" or "Journey") && (employeeId != _identity.EmployeeId))
            throw new CustomException("User data being accessed does not match user making the request.");

        var employeeSalaries = await _employeeSalaryDetailsService.GetEmployeeSalary(employeeId);
        return Ok(employeeSalaries);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPost()]
    public async Task<IActionResult> AddEmployeeSalary([FromBody] EmployeeSalaryDetailsDto employeeSalaryDetailsDto)
    {
        if (_identity.Role is not ("SuperAdmin" or "Admin" or "Talent" or "Journey") && (employeeSalaryDetailsDto.Id != _identity.EmployeeId))
            throw new CustomException("User data being accessed does not match user making the request.");

        var employeeSalaries = await _employeeSalaryDetailsService.SaveEmployeeSalary(employeeSalaryDetailsDto);
        return CreatedAtAction(nameof(AddEmployeeSalary), new { employeeId = employeeSalaries.EmployeeId }, employeeSalaries);

    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPut()]
    public async Task<IActionResult> UpdateSalary([FromBody] EmployeeSalaryDetailsDto employeeSalaryDetailsDto)
    {
        if (_identity.Role is not ("SuperAdmin" or "Admin" or "Talent" or "Journey") && (employeeSalaryDetailsDto.Id != _identity.EmployeeId))
            throw new CustomException("User data being accessed does not match user making the request.");

        var updatedEmployeeSalary = await _employeeSalaryDetailsService.UpdateEmployeeSalary(employeeSalaryDetailsDto);
        return Ok(updatedEmployeeSalary);
    }
}