using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Interfaces.Services;

namespace RGO.App.Controllers
{
    [Route("/gradstacks/")]
    [ApiController]
    public class GradStackController : ControllerBase
    {
        private readonly IGradStackService _gradStackService;
        private readonly IProfileService _profileService;
        private readonly IUserService _userService;

        public GradStackController(IGradStackService gradStackService, IUserService userService)
        {
            _gradStackService = gradStackService;
            _userService = userService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetUserStack([FromQuery] string email)
        {
            try
            {
                var user = await _userService.GetUserByEmail(email);
                var gradStack = await _gradStackService.GetGradStack(user.Id);
                return Ok(gradStack);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> CreateUserStack([FromQuery] string email)
        {
            try
            {
                var user = await _userService.GetUserByEmail(email);
                var gradStack = await _gradStackService.AddGradStack(user.Id);
                return Ok(gradStack);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveGradStack([FromQuery] string email)
        {
            try
            {
                var user = await _userService.GetUserByEmail(email);
                var gradStack = await _gradStackService.RemoveGradStack(user.Id);
                return Ok(gradStack);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateGradStack([FromQuery] string email, [FromBody] Dictionary<string, string> description)
        {
            try
            {
                var user = await _userService.GetUserByEmail(email);
                var gradStack = await _gradStackService.UpdateGradStack(user.Id, description["description"]);
                return Ok(gradStack);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
