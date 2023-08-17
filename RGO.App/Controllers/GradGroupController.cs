using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RGO.Domain.Interfaces.Services;
using RGO.Domain.Models;

namespace RGO.App.Controllers;

[Route("/gradgroup/")]
[ApiController]
public class GradGroupController : ControllerBase
{
    private readonly IGradGroupService _gradGroupService;

    public GradGroupController(IGradGroupService gradGroupService)
    {
        _gradGroupService = gradGroupService;
    }

    [Authorize]
    [HttpGet("getall")]
    public async Task<IActionResult> GetGradGroups()
    {
        var gradGroups = await _gradGroupService.GetGradGroups();
        return Ok(gradGroups);
    }

    [Authorize]
    [HttpPost("add")]
    public async Task<IActionResult> AddGradGroups([FromBody] GradGroupDto newGroupDto)
    {
        try
        {
            var addedGroup = await _gradGroupService.AddGradGroups(newGroupDto);
            return CreatedAtAction(nameof(GetGradGroups), new { id = addedGroup.Id }, addedGroup);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpDelete("delete")]
    public async Task<IActionResult> RemoveGradGroups([FromQuery]int gradGroupId)
    {
        try
        {
            var deleteGradGroup = await _gradGroupService.RemoveGradGroups(gradGroupId);
            return Ok(deleteGradGroup);
        }
        catch (Exception ex)
        {

            return BadRequest(ex.Message);
        }
    }

    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> UpdateGradGroups([FromBody]GradGroupDto updatedGroup)
    {
        try
        {
            var group = await _gradGroupService.UpdateGradGroups(updatedGroup);
            return Ok(group);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
