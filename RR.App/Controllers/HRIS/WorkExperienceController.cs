using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;


[Route("employee-date")]
[ApiController]
public class WorkExperienceController : ControllerBase
{
    private readonly IWorkExperienceService _workExperienceService;
    
    public WorkExperienceController(IWorkExperienceService workExperienceService)
    {
        _workExperienceService = workExperienceService;
    }

    [HttpPost]
    public async Task<IActionResult> SaveWorkExperience([FromBody] WorkExperienceDto newWorkExperience)
    {
        try
        {
            var workExperience = await _workExperienceService.Save(newWorkExperience);
            return CreatedAtAction(nameof(workExperience), workExperience);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("exists"))
                return Problem("Unexceptable", "Unexceptable", 406, "Data already Exists");

            return NotFound(ex.Message);
        }
    }

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

    [HttpDelete]
    public async Task<IActionResult> DeleteWorkExperience([FromQuery] int id)
    {
        try
        {
            var deletedWorkExperience = await _workExperienceService.Delete(id);
            return Ok(deletedWorkExperience);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> UpdateWorkExperience([FromBody] WorkExperienceDto workExperience)
    {
        try
        {
            var workExperience1 = await _workExperienceService.GetWorkExperienceById(workExperience.Id);
            var workExperienceData = new WorkExperienceDto
            {
                Id = workExperience.Id,
                Title = workExperience.Title,
                EmploymentType = workExperience.EmploymentType,
                CompanyName = workExperience.CompanyName,
                Location = workExperience.Location,
                EmployeeId = workExperience.EmployeeId,
                StartDate = workExperience.StartDate,
                EndDate =workExperience.EndDate,
            };

            await _workExperienceService.Update(workExperienceData);

            return Ok();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
