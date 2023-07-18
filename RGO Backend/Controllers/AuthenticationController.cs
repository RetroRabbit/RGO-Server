using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Interfaces.Services;
using RGO.Domain.Models;

namespace RGO_Backend.Controllers
{
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
        public async Task<IActionResult> LoginUser([FromBody] UserDto user)
        {
            bool userExists = await _authService.CheckUserExist(user.email);

            if (!userExists)
            {
                return NotFound("Failed to Login");
            }

            return Ok("Successfully Logged in");

        }
    }
}
