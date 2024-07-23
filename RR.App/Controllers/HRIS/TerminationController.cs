using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("termination")]
[ApiController]
public class TerminationController : ControllerBase
{
    private readonly ITerminationService _terminationService;
    private readonly IEmployeeService _employeeService;

    public TerminationController(ITerminationService terminationService)
    {
        _terminationService = terminationService;
    }

    [Authorize(Policy = "AdminOrTalentOrSuperAdminPolicy")]
    [HttpPost]
    public async Task<IActionResult> AddTermination([FromBody] TerminationDto newTerminationDto)
    {
        try
        {
            var termination = await _terminationService.CreateTermination(newTerminationDto);
            return CreatedAtAction(nameof(AddTermination), new { Id = termination.Id}, termination);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("exists"))
                return BadRequest(ex.Message);
            return Problem("Could not save data.", statusCode: 500);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrSuperAdminPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetTerminationByEmployeeId([FromQuery] int employeeId)
    {
        try
        {
            var workExperienceData = await _terminationService.GetTerminationByEmployeeId(employeeId);

            return Ok(workExperienceData);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
