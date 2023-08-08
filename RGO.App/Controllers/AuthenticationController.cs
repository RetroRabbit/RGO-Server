using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Enums;
using RGO.Domain.Interfaces.Services;

namespace RGO.App.Controllers;

[Route("/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{   

    private readonly IAuthService _authService;

    public AuthenticationController(IAuthService authService) 
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] Dictionary<string, string> email)
    {
        try
        {
            bool userExists = await _authService.CheckUserExist(email["email"]);

            if (!userExists) throw new Exception("User not found");

            List<UserRole> roles = await _authService.GetUserRoles(email["email"]);

            return Ok(roles);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}