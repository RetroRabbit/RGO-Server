using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Interfaces.Services;

namespace RGO.App.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }

        [HttpGet("getuser")]
        public async Task<IActionResult> GetUser([FromQuery]string email)
        {
            try
            {
            var user = await _profileService.GetUserByEmail(email);
                return Ok(user);

            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
