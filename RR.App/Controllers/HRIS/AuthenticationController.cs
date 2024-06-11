using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [Authorize(Policy = "AllRolesPolicy")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpGet()]
    public async Task<IActionResult> LoggedInUser()
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

}