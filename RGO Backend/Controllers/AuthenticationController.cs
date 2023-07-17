using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Interfaces.Services;
using RGO.Domain.Models;

namespace RGO_Backend.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {   

        IAuthService _authService;

        public AuthenticationController(IAuthService authService) 
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUser(UserDto user)
        {
            //check if user exist
            bool userExists = _authService.CheckUserExist(user);

            if (!userExists)
            {
                return NotFound("User not found");
            }

            return Ok(); //_authService.generateToken return oauth token for user

        }
    }
}
