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

        [HttpGet("get")]
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

        [HttpPost("add")]
        public async Task<IActionResult> CreateUserStack([FromQuery] int userId)
        {
            try
            {
                var userStack = await _userStackService.AddUserStack(userId);
                return Ok(userStack);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("remove")]
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

        [HttpPut("update")]
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
