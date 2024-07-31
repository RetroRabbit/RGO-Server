using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HRIS.Services.Services;

namespace RR.App.Controllers.HRIS;

[Route("employee-profile-maybe")]
[ApiController]
public class EmployeeProfileController : ControllerBase
{
    private readonly IEmployeeProfileService _employeeProfileService;

    public EmployeeProfileController(AuthorizeIdentity identity, IEmployeeProfileService employeeProfileService)
    {
        _employeeProfileService = employeeProfileService;
    }

    [HttpGet]
    public async Task<IActionResult> GetEmployeeProfileById([FromQuery] int? id)
    {
        var employee = await _employeeProfileService.GetEmployeeProfileById(id);
        return Ok(employee);
    }
}

