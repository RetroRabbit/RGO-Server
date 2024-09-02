using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace RR.App.Controllers.HRIS;

[Route("auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IEmployeeService _employeeService;
    private readonly IRoleAccessLinkService _roleAccessLinkService;
    private readonly ITerminationService _terminationService;
    private readonly IErrorLoggingService _errorLoggingService;
 
    public AuthenticationController(IAuthService authService, IEmployeeService employeeService, IRoleAccessLinkService roleAccessLinkService, ITerminationService terminationService, IErrorLoggingService errorLoggingService)
    {
        _authService = authService;
        _employeeService = employeeService;
        _roleAccessLinkService = roleAccessLinkService;
        _terminationService = terminationService;
        _errorLoggingService = errorLoggingService;
    }

    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet()]
    public async Task<IActionResult> LoggingInUser()
    {
        try
        {
            return Ok("Api connection works");
        }
        catch (Exception ex)
        {
            _errorLoggingService.LogException(ex);
            return NotFound(ex.Message);
        }
    }

    [Authorize]
    [HttpPost()]
    public async Task<IActionResult> CheckUserExistence()
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;

        await _authService.CheckUserExistence(claimsIdentity!);

        return Ok("User found.");
    }

}
