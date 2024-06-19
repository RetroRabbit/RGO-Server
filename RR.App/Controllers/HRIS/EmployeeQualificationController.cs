using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RR.UnitOfWork;
using System.Security.Claims;

namespace RR.App.Controllers.HRIS;

[Route("employee-qualifications")]
[ApiController]
public class EmployeeQualificationController : ControllerBase
{
    private readonly IEmployeeQualificationService _employeeQualificationService;

    public EmployeeQualificationController(IEmployeeQualificationService employeeQualificationService, IEmployeeService employeeService, IErrorLoggingService errorLoggingService)
    {
        _employeeQualificationService = employeeQualificationService;
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPost]
    public async Task<IActionResult> SaveEmployeeQualification([FromBody] EmployeeQualificationDto employeeQualificationDto)
    {
        try
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var role = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value!;
            if ("SuperAdmin" == role || "Admin" == role || "Talent" == role || "Journey" == role)
            {
                var newQualification = await _employeeQualificationService.SaveEmployeeQualification(employeeQualificationDto, employeeQualificationDto.EmployeeId);
                return Ok(newQualification);
            }
            var userId = GlobalVariables.GetUserId();

            if (employeeQualificationDto.EmployeeId == userId)
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
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var role = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value!;
            if ("SuperAdmin" == role || "Admin" == role || "Talent" == role || "Journey" == role)
            {
                var qualifications = await _employeeQualificationService.GetAllEmployeeQualificationsByEmployeeId(employeeId);
                return Ok(qualifications);
            }
            var userId = GlobalVariables.GetUserId();

            if (employeeId == userId)
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
