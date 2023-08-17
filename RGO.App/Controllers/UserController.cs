using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.App.Controllers;

[Route("/user/")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
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

    [Authorize(Policy = "isAdmin")]
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
            List<GradGroupDto> userGroups = await _userService.GetGradGroups();
            return Ok(userGroups);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }

    [Authorize]
    [HttpDelete("delete")]
    public async Task<IActionResult> RemoveUser([FromQuery] string email)
    {
        try
        {
            var removedUser = await _userService.RemoveUser(email);
            return Ok(removedUser);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

