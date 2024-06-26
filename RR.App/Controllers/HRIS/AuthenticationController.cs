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

    public AuthenticationController(IAuthService authService, IEmployeeService employeeService)
    {
        _authService = authService;
        _employeeService = employeeService;
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
                return BadRequest("Email claim not found.");
            }

            var userExists = await _employeeService.CheckUserExist(authEmail);
            if (!userExists)
            {
                return NotFound("User not found.");
            }

            var authId = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(authId))
            {
                return BadRequest("Auth Id claim not found.");
            }

            var role = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(role))
            {
                var employee = await _employeeService.GetEmployee(authEmail);
                if (employee == null)
                {
                    return NotFound("Employee not found.");
                }

                employee.AuthUserId = authId;
                await _employeeService.UpdateEmployee(employee, authEmail);

                var allRoles = await _authService.GetAllRolesAsync();
                var roleFound = allRoles.Any(r => r.Name == "Employee");

                if (!roleFound)
                {
                    return NotFound("Auth0 does not have this Employee Role.");
                }

                await _authService.AddRoleToUserAsync(authId, allRoles.First(r => r.Name == "Employee").Id);

                return Ok("User found.");
            }

            return Ok("User found.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

}