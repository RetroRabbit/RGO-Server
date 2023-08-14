using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Interfaces.Services;

namespace RGO.App.Controllers;

[Authorize(Policy = "isAdmin")]
[Route("/profile/")]
[ApiController]
public class ProfileController : ControllerBase
{
    private readonly IProfileService _profileService;

    public ProfileController(IProfileService profileService)
    {
        _profileService = profileService;
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetUserProfileByEmail([FromQuery]string email)
    {
        try
        {
        var userProfile = await _profileService.GetUserProfileByEmail(email);
            return Ok(userProfile);

        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}