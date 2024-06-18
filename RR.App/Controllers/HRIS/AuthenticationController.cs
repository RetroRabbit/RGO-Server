using System.Security.Claims;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.App.Controllers.HRIS;

[Route("auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthenticationController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromQuery] string email)
    {
        try
        {
            var userExists = await _authService.CheckUserExist(email);

            if (!userExists) throw new Exception("User not found");

            var token = await _authService.Login(email);

            return Ok(token);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> RegisterEmployee([FromBody] EmployeeDto newEmployee)
    {
        try
        {
            var userExists = await _authService.CheckUserExist(newEmployee.Email!);

            if (userExists) throw new Exception("Employee already exists");

            var token = await _authService.RegisterEmployee(newEmployee);

            return CreatedAtAction(nameof(RegisterEmployee), newEmployee);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AllRolesPolicy")]
    [HttpGet("roles")]
    public async Task<IActionResult> GetUserRoles([FromQuery] string? email)
    {
        try
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var accessTokenEmail = claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value;
            var role = claimsIdentity?.FindFirst(ClaimTypes.Role)?.Value;

            if (!string.IsNullOrEmpty(accessTokenEmail))
            {
                var emailToUse = string.IsNullOrEmpty(email)
                ? claimsIdentity?.FindFirst(ClaimTypes.Email)?.Value
                : email;

                if ("SuperAdmin" == role || "Admin" == role || "Talent" == role || "Journey" == role)
                {
                    var roles = await _authService.GetUserRoles(emailToUse ?? "");

                    return Ok(roles);
                }


                if (emailToUse == accessTokenEmail)
                {
                    var roles = await _authService.GetUserRoles(emailToUse ?? "");

                    return Ok(roles);
                }
            }

            return NotFound("Tampering found!");
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}