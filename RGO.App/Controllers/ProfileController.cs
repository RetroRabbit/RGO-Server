using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Interfaces.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RGO.App.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService profileService;

        public ProfileController(IProfileService profileService)
        {
            this.profileService = profileService;
        }

        // GET api/<ProfileController>/5
        [HttpGet("getuser")]
        public async Task<IActionResult> GetUser([FromQuery]string email)
        {
            try
            {
            var user = await profileService.GetUserByEmail(email);
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
