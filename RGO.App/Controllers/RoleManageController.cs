using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.App.Controllers;

[Route("/rolemanage/")]
[ApiController]
public class RoleManageController : ControllerBase
{
    private readonly IRoleAccessLinkService _roleAccessLinkService;
    private readonly IRoleService _roleService;
    private readonly IRoleAccessService _roleAccessService;

    public RoleManageController(
        IRoleAccessLinkService roleAccessLinkService,
        IRoleService roleService,
        IRoleAccessService roleAccessService)
    {
        _roleAccessLinkService = roleAccessLinkService;
        _roleService = roleService;
        _roleAccessService = roleAccessService;
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpPost("assign/permission")]
    public async Task<IActionResult> AssignPermission(
        [FromQuery] string role,
        [FromQuery] string permission)
    {
        try
        {
            RoleDto roleDto;
            try
            {
                roleDto = await _roleService.GetRole(role);
            }
            catch (Exception)
            {
                roleDto = await _roleService.SaveRole(new RoleDto(0, role));
            }

            RoleAccessDto roleAccessDto;

            try
            {
                roleAccessDto = await _roleAccessService.GetRoleAccess(permission);
            }
            catch (Exception)
            {
                roleAccessDto = await _roleAccessService.SaveRoleAccess(new RoleAccessDto(0, permission));
            }

            var roleAccessLink = await _roleAccessLinkService.Save(new RoleAccessLinkDto(0,roleDto, roleAccessDto));

            return CreatedAtAction(nameof(AssignPermission), new { role = roleAccessLink.Role!.Description, permission = roleAccessLink.RoleAccess!.Permission }, roleAccessLink);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [ProducesResponseType(typeof(List<string>), 200)]
    [ProducesErrorResponseType(typeof(string))]
    [HttpGet("get")]
    public async Task<IActionResult> GetRoleAccessLink([FromQuery] string role)
    {
        try
        {
            var roleAccessLink = await _roleAccessLinkService.GetByRole(role);

            return Ok(roleAccessLink.First().Value);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [ProducesResponseType(typeof(RoleAccessLinkDto), 200)]
    [ProducesErrorResponseType(typeof(string))]
    [HttpDelete("unassign/permission")]
    public async Task<IActionResult> DeleteRoleAccessLink(
        [FromQuery] string role,
        [FromQuery] string access)
    {
        try
        {
            var roleAccessLink = await _roleAccessLinkService.Delete(role, access);

            return CreatedAtAction(nameof(DeleteRoleAccessLink), roleAccessLink);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
