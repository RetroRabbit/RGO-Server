using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RR.App.Controllers.HRIS;

[Route("employee-profile")]
[ApiController]
public class EmployeeProfileController : ControllerBase
{
    private readonly AuthorizeIdentity _identity;
    private readonly IEmployeeProfileService _employeeProfileService;

    public EmployeeProfileController(AuthorizeIdentity identity, IEmployeeProfileService employeeProfileService)
    {
        _identity = identity;
        _employeeProfileService = employeeProfileService;
    }

   

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetEmployeeProfileById([FromQuery] string? email)
    {
        var employee = await _employeeProfileService.GetEmployeeProfileByEmail(email);
        return Ok(employee);
    }
}

