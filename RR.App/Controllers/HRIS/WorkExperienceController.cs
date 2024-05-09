using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var workExperience = await _workExperienceService.Save(newWorkExperience);
            return CreatedAtAction(nameof(SaveWorkExperience), workExperience);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("work experience exists"))
                return BadRequest("work experience exists");

            return Problem("An unexpected error occurred.", statusCode: 404);
        }

    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetWorkExperienceById([FromQuery] int id)
    {
        try
        {
            var workExperienceData = await _workExperienceService.GetWorkExperienceById(id);

            return Ok(workExperienceData);
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
            await _workExperienceService.Delete(id);
            return Ok();
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
            await _workExperienceService.Update(workExperience);
            return Ok(workExperience);
        }
        catch (Exception ex)
        { 
            return BadRequest("Work experience could not be updated");
        }
    }
}
