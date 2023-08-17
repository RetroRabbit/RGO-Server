using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Models.Enums;
using RGO.Services.Interfaces;

namespace RGO.App.Controllers;

[Route("/auth/")]
[ApiController]
public class AuthenticationController : ControllerBase
{

    private readonly IAuthService _authService;

    public AuthenticationController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromQuery] string email)
    {
        try
        {
            bool userExists = await _authService.CheckUserExist(email);

            if (!userExists) throw new Exception("User not found");


            string token = await _authService.GenerateToken(email);

            return Ok(token);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("roles")]
    public async Task<IActionResult> GetUserRoles([FromQuery] string email)
    {
        try
        {
            List<UserRole> roles = await _authService.GetUserRoles(email);

            return Ok(roles);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}