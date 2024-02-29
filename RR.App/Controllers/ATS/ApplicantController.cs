using ATS.Models;
using ATS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.ATS;

[Route("applicants")]
[ApiController]
public class ApplicantController : ControllerBase
{
    private readonly IApplicantService _applicantService;

    public ApplicantController(IApplicantService applicantService)
    {
        _applicantService = applicantService;
    }

    [Authorize(Policy = "Talent")]
    [HttpPost]
    public async Task<IActionResult> AddApplicant([FromBody] ApplicantDto applicant)
    {
        try
        {
            var newApplicant = await _applicantService.SaveApplicant(applicant);
            return Ok(newApplicant);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("exists"))
                return Problem("Unexceptable", "Unexceptable", 406, "User Exists");

            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "Talent")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            List<ApplicantDto> applicantList = await _applicantService.GetAllApplicants();
            return Ok(applicantList);
        }
        catch (Exception ex)
        {
            return Problem("Unexceptable", "Unexceptable", 500, "Something went wrong");
        }
    }

    [Authorize(Policy = "Talent")]
    [HttpGet]
    public async Task<IActionResult> GetById([FromQuery] int id)
    {
        try
        {
            ApplicantDto applicant = await _applicantService.GetApplicantById(id);
            return Ok(applicant);
        }
        catch(Exception ex) 
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "Talent")]
    [HttpGet("by-email")]
    public async Task<IActionResult> GetByEmail([FromQuery] string email)
    {
        try
        {
            ApplicantDto applicant = await _applicantService.GetApplicantByEmail(email);
            return Ok(applicant);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "Talent")]
    [HttpPut]
    public async Task<IActionResult> UpdateApplicant([FromBody] ApplicantDto applicant)
    {
        try
        {
            ApplicantDto applicantDto = await _applicantService.UpdateApplicant(applicant);
            return Ok(applicantDto);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "Talent")]
    [HttpDelete]
    public async Task<IActionResult> DeleteApplicant([FromQuery] int id)
    {
        try
        {
            ApplicantDto applicant = await _applicantService.DeleteApplicant(id);
            return Ok(applicant);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}