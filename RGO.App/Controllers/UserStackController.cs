using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Interfaces.Services;

namespace RGO.App.Controllers;

[Authorize(Policy = "isGrad")]
[Route("/userstacks/")]
[ApiController]
public class UserStackController : ControllerBase
{
    private readonly IUserStackService _userStackService;
    private readonly IProfileService _profileService;
    private readonly IUserService _userService;

    public UserStackController(IUserStackService userStackService, IUserService userService)
    {
        _userStackService = userStackService;
        _userService = userService;
    }

    [Authorize(Policy = "isGrad")]
    [Authorize(Policy = "isAdmin")]
    [HttpGet("get")]
    public async Task<IActionResult> GetUserStack([FromQuery] string email)
    {
        try
        {
            var user = await _userService.GetUserByEmail(email);
            var userStack = await _userStackService.GetUserStack(user.Id);
            return Ok(userStack);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "isGrad")]
    [Authorize(Policy = "isAdmin")]
    [HttpPost("add")]
    public async Task<IActionResult> CreateUserStack([FromQuery] string email)
    {
        try
        {
            var user = await _userService.GetUserByEmail(email);
            var userStack = await _userStackService.AddUserStack(user.Id);
            return Ok(userStack);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "isGrad")]
    [Authorize(Policy = "isAdmin")]
    [HttpDelete("remove")]
    public async Task<IActionResult> RemoveUserStack([FromQuery] string email)
    {
        try
        {
            var user = await _userService.GetUserByEmail(email);
            var userStack = await _userStackService.RemoveUserStack(user.Id);
            return Ok(userStack);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "isGrad")]
    [Authorize(Policy = "isAdmin")]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateUserStack([FromQuery] string email, [FromBody] string description)
    {
        try
        {
            var user = await _userService.GetUserByEmail(email);
            var userStack = await _userStackService.UpdateUserStack(user.Id, description);
            return Ok(userStack);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
