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
        try
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var authEmail = claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(authEmail))
            {
                var exception = new Exception("Email claim not found");
                _errorLoggingService.LogException(exception);
                return NotFound("User not found.");
            }

            var userExists = await _employeeService.CheckUserExist(authEmail);
            if (!userExists)
            {
                await _authService.DeleteUser(authEmail);
                var exception = new Exception("User not found");
                _errorLoggingService.LogException(exception);
                return NotFound("User not found.");
            }

            var authId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(authId))
            {
                var exception = new Exception("Auth Id claim not found");
                _errorLoggingService.LogException(exception);
                return BadRequest("User not found.");
            }

            var role = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(role))
            {
                var employee = await _employeeService.GetEmployee(authEmail);
                if (employee == null)
                {
                    var exception = new Exception("User account not found in database.");
                    _errorLoggingService.LogException(exception);
                    return NotFound("User not found.");
                }

                if (employee.AuthUserId != authId)
                {
                    employee.AuthUserId = authId;
                    await _employeeService.UpdateEmployee(employee, authEmail);
                }

                var isUserTerminated = await _terminationService.CheckTerminationExist(employee.Id);
                if (true == isUserTerminated)
                {
                    var exception = new Exception("User account not found in database.");
                    _errorLoggingService.LogException(exception);
                    return NotFound("User not found.");
                }

                var allRoles = await _authService.GetAllRolesAsync();
                var databaseEmployeeRole = await _roleAccessLinkService.GetRoleByEmployee(authEmail);
                var roleFound = allRoles.Any(r => r.Name == databaseEmployeeRole.First().Key);

                if (!roleFound)
                {
                    var exception = new Exception($"Auth0 does not have this {databaseEmployeeRole.First().Key} Role.");
                    _errorLoggingService.LogException(exception);
                    return NotFound("User not found");
                }

                await _authService.AddRoleToUserAsync(authId, allRoles.First(r => r.Name == databaseEmployeeRole.First().Key).Id);

                return Ok("User found.");
            }

            return Ok("User found.");
        }
        catch (Exception ex)
        {
            _errorLoggingService.LogException(ex);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

}