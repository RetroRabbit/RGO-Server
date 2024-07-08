using System.Security.Claims;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RR.UnitOfWork.Entities.HRIS;

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

            var employee = await _employeeService.SaveEmployee(newEmployee);
            var employeeExists = await _employeeService.CheckUserExist(employee.Email);
            return CreatedAtAction(nameof(AddEmployee), new { email = employee.Email }, employee);

    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpDelete]
    public async Task<IActionResult> DeleteEmployee([FromQuery] String email)
    {
        
            var deletedEmployee = await _employeeService.DeleteEmployee(email);
            return Ok(deletedEmployee);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetEmployeeById([FromQuery] int id)
    {
        
            var employee = await _employeeService.GetEmployeeById(id);
            return Ok(employee);
        
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("by-email")]
    public async Task<IActionResult> GetEmployeeByEmail([FromQuery] string? email)
    {

            var employee = await _employeeService.GetEmployee(_identity.Email);
            return Ok(employee);

    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDto employee, [FromQuery] string userEmail)
    {

            if (_identity.Role is not "SuperAdmin" or "Admin" or "Talent" or "Journey" && employee.Id != _identity.EmployeeId)
            throw new CustomException("Unauthorized action.");
            var updatedEmployee = await _employeeService.UpdateEmployee(employee, employee.Email);
            return CreatedAtAction(nameof(UpdateEmployee), new { email = updatedEmployee.Email }, updatedEmployee);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllEmployees()
    {

            var employees = await _employeeService.GetAll(_identity.Email);
            return Ok(employees);

    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet("simple-profile")]
    public async Task<IActionResult> GetSimpleEmployee([FromQuery] string employeeEmail)
    {

            if (_identity.Role is not "SuperAdmin" or "Admin" or "Talent" or "Journey" && employeeEmail != _identity.Email)
                 throw new CustomException("User data being accessed does not match user making the request.");
            var simpleProfile = await _employeeService.GetSimpleProfile(employeeEmail);
            return Ok(simpleProfile);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("filter-employees")]
    public async Task<IActionResult> FilterEmployees(int peopleChampId, int employeetype,bool activeStatus = true )
    {

            var employees = await _employeeService.FilterEmployees(peopleChampId, employeetype, activeStatus);
            return Ok(employees);

    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet("id-number")]
    public async Task<IActionResult> CheckIdNumber([FromQuery] string idNumber, [FromQuery] int employeeId)
    {
        if (_identity.Role is not "SuperAdmin" or "Admin" or "Talent" or "Journey" && employeeId != _identity.EmployeeId)
            throw new CustomException("User data being accessed does not match user making the request.");
        var isExisting = await _employeeService.CheckDuplicateIdNumber(idNumber, employeeId);
        return Ok(isExisting);
    }
}