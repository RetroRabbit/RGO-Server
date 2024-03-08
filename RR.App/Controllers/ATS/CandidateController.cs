using ATS.Models;
using ATS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.ATS;

[Route("candidates")]
[ApiController]
public class CandidateController : ControllerBase
{
    private readonly ICandidateService _candidateService;

    public CandidateController(ICandidateService candidateService)
    {
        _candidateService = candidateService;
    }

    [Authorize(Policy = "AdminOrTalentOrSuperAdminPolicy")]
    [HttpPost]
    public async Task<IActionResult> AddCandidate([FromBody] CandidateDto candidate)
    {
        try
        {
            var newCandidate = await _candidateService.SaveCandidate(candidate);
            return Ok(newCandidate);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("Exists"))
                return Conflict("User Exists");

            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrSuperAdminPolicy")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            List<CandidateDto> candidateList = await _candidateService.GetAllCandidates();
            return Ok(candidateList);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrSuperAdminPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetById([FromQuery] int id)
    {
        try
        {
            CandidateDto candidate = await _candidateService.GetCandidateById(id);
            return Ok(candidate);
        }
        catch(Exception ex) 
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrSuperAdminPolicy")]
    [HttpGet("by-email")]
    public async Task<IActionResult> GetByEmail([FromQuery] string email)
    {
        try
        {
            CandidateDto candidate = await _candidateService.GetCandidateByEmail(email);
            return Ok(candidate);
        }
       catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrSuperAdminPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateCandidate([FromBody] CandidateDto candidate)
    {
        try
        {
            CandidateDto candidateDto = await _candidateService.UpdateCandidate(candidate);
            return Ok(candidateDto);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrSuperAdminPolicy")]
    [HttpDelete]
    public async Task<IActionResult> DeleteCandidate([FromQuery] int id)
    {
        try
        {
            CandidateDto candidate = await _candidateService.DeleteCandidate(id);
            return Ok(candidate);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}