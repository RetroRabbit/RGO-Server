using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;


[Route("work-experience")]
[ApiController]
public class WorkExperienceController : ControllerBase
{
    private readonly AuthorizeIdentity _identity;
    private readonly IWorkExperienceService _workExperienceService;

    public WorkExperienceController(AuthorizeIdentity identity, IWorkExperienceService workExperienceService)
    {
        _identity = identity;
        _workExperienceService = workExperienceService;
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPost]
    public async Task<IActionResult> SaveWorkExperience([FromBody] WorkExperienceDto newWorkExperience)
    {
        if (!_identity.IsSupport && newWorkExperience.EmployeeId != _identity.EmployeeId)
            throw new CustomException("User data being accessed does not match user making the request.");

        var workExperience = await _workExperienceService.Save(newWorkExperience);
        return CreatedAtAction(nameof(SaveWorkExperience), workExperience);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetWorkExperienceById([FromQuery] int id)
    {
        if (!_identity.IsSupport && id != _identity.EmployeeId)
            throw new CustomException("User data being accessed does not match user making the request.");

        var workExperienceData = await _workExperienceService.GetWorkExperienceByEmployeeId(id);
        return Ok(workExperienceData);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpDelete]
    public async Task<IActionResult> DeleteWorkExperience([FromQuery] int id)
    {
        if (!_identity.IsSupport && id != _identity.EmployeeId)
            throw new CustomException("User data being accessed does not match user making the request.");

        await _workExperienceService.Delete(id);
        return Ok();
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateWorkExperience([FromBody] WorkExperienceDto workExperience)
    {
        try
        {
            if (_identity.Role is "SuperAdmin" or "Admin" or "Talent" or "Journey")
            {
                await _workExperienceService.Update(workExperience);
                return Ok(workExperience);
            }

            if (workExperience.Id == _identity.EmployeeId)
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
