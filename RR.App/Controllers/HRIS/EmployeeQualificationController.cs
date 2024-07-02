using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("employee-qualifications")]
[ApiController]
public class EmployeeQualificationController : ControllerBase
{
    private readonly AuthorizeIdentity _identity;
    private readonly IEmployeeQualificationService _employeeQualificationService;

    public EmployeeQualificationController(AuthorizeIdentity identity, IEmployeeQualificationService employeeQualificationService)
    {
        _identity = identity;
        _employeeQualificationService = employeeQualificationService;
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPost]
    public async Task<IActionResult> SaveEmployeeQualification([FromBody] EmployeeQualificationDto employeeQualificationDto)
    {
        if (_identity.Role is not ("SuperAdmin" or "Admin" or "Talent" or "Journey") && employeeQualificationDto.EmployeeId != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");

        var newQualification = await _employeeQualificationService.SaveEmployeeQualification(employeeQualificationDto, employeeQualificationDto.EmployeeId);
        return Ok(newQualification);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeQualification(int id, [FromBody] EmployeeQualificationDto employeeQualificationDto)
    {
        employeeQualificationDto.Id = id;
        var updatedQualification = await _employeeQualificationService.UpdateEmployeeQualification(employeeQualificationDto);
        return Ok(updatedQualification);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetAllEmployeeQualifications()
    {
        var qualifications = await _employeeQualificationService.GetAllEmployeeQualifications();
        return Ok(qualifications);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet("{employeeId}")]
    public async Task<ActionResult<List<EmployeeQualificationDto>>> GetEmployeeQualificationByEmployeeId(int employeeId)
    {
        if (_identity.Role is not ("SuperAdmin" or "Admin" or "Talent" or "Journey") && employeeId != _identity.EmployeeId)
            throw new CustomException("Unauthorized Access.");

        var data = await _employeeQualificationService.GetAllEmployeeQualificationsByEmployeeId(employeeId);
        return Ok(data);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeQualification(int id)
    {
        var deletedQualification = await _employeeQualificationService.DeleteEmployeeQualification(id);
        return Ok(deletedQualification);
    }
}
