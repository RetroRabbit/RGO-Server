using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Interfaces.Services;

namespace RGO.App.Controllers;

[Route("[controller]")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet("getuserprofile")]
    public async Task<IActionResult> GetUserProfileByEmail([FromQuery]string email)
    {
        try
        {
        var userProfile = await _profileService.GetUserProfileByEmail(email);
            return Ok(userProfile);

        }
        catch (Exception e)
        {
            await Console.Out.WriteLineAsync(e.Message);
            return BadRequest(e.Message);
        }
    }
}