using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("employee-profile-maybe")]
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
    public async Task<IActionResult> GetEmployeeProfileById([FromQuery] int? id)
    {
        var employee = await _employeeProfileService.GetEmployeeProfileById(id);
        return Ok(employee);
    }
}

