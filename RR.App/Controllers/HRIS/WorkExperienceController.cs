using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RR.UnitOfWork;
using System.Security.Claims;

namespace RR.App.Controllers.HRIS;


[Route("work-experience")]
[ApiController]
public class WorkExperienceController : ControllerBase
{
    private readonly IWorkExperienceService _workExperienceService;
    
    public WorkExperienceController(IWorkExperienceService workExperienceService)
    {
        _workExperienceService = workExperienceService;
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPost]
    public async Task<IActionResult> SaveWorkExperience([FromBody] WorkExperienceDto newWorkExperience)
    {
        try
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var role = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value!;
            if ("SuperAdmin" == role || "Admin" == role || "Talent" == role || "Journey" == role)
            {
                var workExperience = await _workExperienceService.Save(newWorkExperience);
                return CreatedAtAction(nameof(SaveWorkExperience), workExperience);
            }

            var userId = GlobalVariables.GetUserId();
            if (newWorkExperience.Id == userId)
            {
                var workExperience = await _workExperienceService.Save(newWorkExperience);
                return CreatedAtAction(nameof(SaveWorkExperience), workExperience);
            }

            return NotFound("User data being accessed does not match user making the request.");
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("work experience exists"))
                return BadRequest("work experience exists");

            return Problem("Could not save data.", statusCode: 500);
        }

    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetWorkExperienceById([FromQuery] int id)
    {
        try
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var role = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value!;
            if ("SuperAdmin" == role || "Admin" == role || "Talent" == role || "Journey" == role)
            {
                var workExperienceData = await _workExperienceService.GetWorkExperienceByEmployeeId(id);
                return Ok(workExperienceData);
            }

            var userId = GlobalVariables.GetUserId();
            if (id == userId)
            {
                var workExperienceData = await _workExperienceService.GetWorkExperienceByEmployeeId(id);
                return Ok(workExperienceData);
            }

            return NotFound("User data being accessed does not match user making the request.");
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpDelete]
    public async Task<IActionResult> DeleteWorkExperience([FromQuery] int id)
    {
        try
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var role = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value!;
            if ("SuperAdmin" == role || "Admin" == role || "Talent" == role || "Journey" == role)
            {
                await _workExperienceService.Delete(id);
                return Ok();
            }

            var userId = GlobalVariables.GetUserId();
            if (id == userId)
            {
                await _workExperienceService.Delete(id);
                return Ok();
            }

            return NotFound("User data being accessed does not match user making the request.");
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateWorkExperience([FromBody] WorkExperienceDto workExperience)
    {
        try
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var role = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value!;
            if ("SuperAdmin" == role || "Admin" == role || "Talent" == role || "Journey" == role)
            {
                await _workExperienceService.Update(workExperience);
                return Ok(workExperience);
            }

            var userId = GlobalVariables.GetUserId();
            if (workExperience.Id == userId)
            {
                await _workExperienceService.Update(workExperience);
                return Ok(workExperience);
            }

            return NotFound("User data being accessed does not match user making the request.");
        }
        catch (Exception ex)
        { 
            return BadRequest("Work experience could not be updated");
        }
    }
}
