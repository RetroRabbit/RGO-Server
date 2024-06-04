﻿using System.Security.Claims;
using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RR.UnitOfWork.Migrations;

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

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPost]
    public async Task<IActionResult> AddTermination([FromBody] TerminationDto newTerminationDto)
    {
        try
        {
            var termination = await _terminationService.SaveTermination(newTerminationDto);
            return CreatedAtAction(nameof(AddTermination), new { Id = termination.Id}, termination);
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("exists"))
                return BadRequest(ex.Message);
            return Problem("Could not save data.", statusCode: 500);
        }
    }
}
