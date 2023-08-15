using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Interfaces.Services;
using RGO.Domain.Models;

namespace RGO_Backend.Controllers;

[Route("/user/")]
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

    [Authorize]
    [HttpGet("get")]
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

    [Authorize(Policy = "isAdmin")]
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

    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateUser([FromQuery] string email, [FromBody] ProfileDto profile)
    {
        try
        {
            ProfileDto updatedProfile = await _userService.UpdateUser(email, profile);
            return Ok(updatedProfile);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpGet("users/get")]
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

    [Authorize]
    [HttpGet("groups")]
    public async Task<IActionResult> GetUserGroups()
    {
        try
        {
            List<UserGroupDto> userGroups = await _userService.GetUserGroups();
            return Ok(userGroups);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }
}

