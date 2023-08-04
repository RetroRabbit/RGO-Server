using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Interfaces.Services;

namespace RGO.App.Controllers
{
    [Route("/userstacks")]
    [ApiController]
    public class UserStackController : ControllerBase
    {
        private readonly IUserStackService _userStackService;

        public UserStackController(IUserStackService userStackService)
        {
            _userStackService = userStackService;
        }

        [HttpGet("GetUserStack")]
        public async Task<IActionResult> GetUserStack([FromQuery] int userId)
        {
            try
            {
                var userStack = await _userStackService.GetUserStack(userId);
                return Ok(userStack);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateUserStack")]
        public async Task<IActionResult> CreateUserStack([FromQuery] int userId)
        {
            try
            {
                var userStack = await _userStackService.CreateUserStack(userId);
                return Ok(userStack);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("RemoveUserStack")]
        public async Task<IActionResult> RemoveUserStack([FromQuery] int userId)
        {
            try
            {
                var userStack = await _userStackService.RemoveUserStack(userId);
                return Ok(userStack);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("UpdateUserStack")]
        public async Task<IActionResult> UpdateUserStack([FromQuery] int userId, [FromBody] string description)
        {
            try
            {
                var userStack = await _userStackService.UpdateUserStack(userId, description);
                return Ok(userStack);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
