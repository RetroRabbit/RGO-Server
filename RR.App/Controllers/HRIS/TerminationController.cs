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
        var termination = await _terminationService.CreateTermination(newTerminationDto);
        return CreatedAtAction(nameof(AddTermination), new { termination.Id }, termination);
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetTerminationByEmployeeId([FromQuery] int employeeId)
    {
        var workExperienceData = await _terminationService.GetTerminationByEmployeeId(employeeId);
        return Ok(workExperienceData);
    }
}
