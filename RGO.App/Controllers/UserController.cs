using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Interfaces.Services;
using RGO.Domain.Models;
using RGO.Repository.Entities;
using System.Reflection.Metadata.Ecma335;

namespace RGO_Backend.Controllers
{
    [Route("/users/")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IProfileService _profileService;
        private readonly IUserService _userService;

        public UserController(IProfileService profileService, IUserService userService)
        {
            _profileService = profileService;
            _userService = userService;
        }

        [HttpGet("getuser")]
        public async Task<IActionResult> GetUser([FromQuery] string email)
        {
            try
            {
                var user = await _userService.GetUserByEmail(email);
                return Ok(user);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] UserDto user)
        {
             try
            {
                var newUser = await _userService.AddUser(user);
                return Ok(newUser);

            }
            catch (Exception e)
              {
                return BadRequest(e.Message);
             }
         }

        [HttpGet("get")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var user = await _userService.GetUsers();
                return Ok(user);

            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
