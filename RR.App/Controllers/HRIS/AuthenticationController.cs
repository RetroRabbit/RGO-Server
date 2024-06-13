using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("auth")]
[ApiController]
public class AuthenticationController : ControllerBase
{

    public AuthenticationController()
    {
    }

    [AllowAnonymous]
    [ApiExplorerSettings(IgnoreApi = true)]
    [HttpPost()]
    public async Task<IActionResult> LoggingInUser()
    {
        try
        {
            return Ok("Server online");
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

}