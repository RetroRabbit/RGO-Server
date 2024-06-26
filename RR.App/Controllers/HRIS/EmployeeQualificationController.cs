using HRIS.Models;
using HRIS.Services.Interfaces;
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
        try
        {
            if (_identity.Role is "SuperAdmin" or "Admin" or "Talent" or "Journey")
            {
                var newQualification = await _employeeQualificationService.SaveEmployeeQualification(employeeQualificationDto, employeeQualificationDto.EmployeeId);
                return Ok(newQualification);
            }
            
            if (employeeQualificationDto.EmployeeId == _identity.EmployeeId)
            {
                var newQualification = await _employeeQualificationService.SaveEmployeeQualification(employeeQualificationDto, employeeQualificationDto.EmployeeId);
                return Ok(newQualification);
            }
            return NotFound("User data being accessed does not match user making the request.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmployeeQualification(int id, [FromBody] EmployeeQualificationDto employeeQualificationDto)
    {
        try
        {
            employeeQualificationDto.Id = id;
            var updatedQualification = await _employeeQualificationService.UpdateEmployeeQualification(employeeQualificationDto);
            return Ok(updatedQualification);
        }
        catch (KeyNotFoundException knf)
        {
            return NotFound(knf.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet]
    public async Task<ActionResult<List<EmployeeQualificationDto>>> GetAllEmployeeQualifications()
    {
        try
        {
            var qualifications = await _employeeQualificationService.GetAllEmployeeQualifications();
            return Ok(qualifications);
        }
        catch (Exception ex)
        {
            return BadRequest("An error occurred while retrieving qualifications: " + ex.Message);
        }
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet("{employeeId}")]
    public async Task<ActionResult<List<EmployeeQualificationDto>>> GetEmployeeQualificationByEmployeeId(int employeeId)
    {
        try
        {
            if (_identity.Role is "SuperAdmin" or "Admin" or "Talent" or "Journey")
            {
                var qualifications = await _employeeQualificationService.GetAllEmployeeQualificationsByEmployeeId(employeeId);
                return Ok(qualifications);
            }
            
            if (employeeId == _identity.EmployeeId)
            {
                var qualifications = await _employeeQualificationService.GetAllEmployeeQualificationsByEmployeeId(employeeId);
                return Ok(qualifications);
            }
            return NotFound("User data being accessed does not match user making the request.");
        }
        catch (KeyNotFoundException knf)
        {
            return NotFound(knf.Message);
        }
        catch (Exception ex)
        {
            return BadRequest("An error occurred while retrieving qualifications: " + ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmployeeQualification(int id)
    {
        try
        {
            var deletedQualification = await _employeeQualificationService.DeleteEmployeeQualification(id);
            return Ok(deletedQualification);
        }
        catch (KeyNotFoundException knf)
        {
            return NotFound(knf.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while deleting the qualification: " + ex.Message);
        }
    }
}
