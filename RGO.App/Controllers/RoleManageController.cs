using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork.Entities;
using System.Security;

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
    [HttpPost("add")]
    public async Task<IActionResult> AddPermission([FromQuery] string role, [FromQuery] string permission)
    {
        try
        {
            var foundRole = await _roleService.CheckRole(role) ?
                await _roleService.GetRole(role) :
                await _roleService.SaveRole(new RoleDto(0, role));

            var roleAccess = await _roleAccessService.CheckRoleAccess(permission) ?
                await _roleAccessService.GetRoleAccess(permission) :
                await _roleAccessService.SaveRoleAccess(new RoleAccessDto(0, permission));

            var roleAccessLink = await _roleAccessLinkService.Save(new RoleAccessLinkDto(0, foundRole, roleAccess));

            return CreatedAtAction(nameof(AddPermission), roleAccessLink);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [ProducesResponseType(typeof(RoleAccessLinkDto), 200)]
    [ProducesErrorResponseType(typeof(string))]
    [HttpDelete("remove")]
    public async Task<IActionResult> RemovePermission([FromQuery] string role, [FromQuery] string permission)
    {
        try
        {
            var foundRole = await _roleService.CheckRole(role) ?
                await _roleService.GetRole(role) :
                await _roleService.SaveRole(new RoleDto(0, role));

            var roleAccess = await _roleAccessService.CheckRoleAccess(permission) ?
                await _roleAccessService.GetRoleAccess(permission) :
                await _roleAccessService.SaveRoleAccess(new RoleAccessDto(0, permission));

            var roleAccessLink = await _roleAccessLinkService.Delete(role, permission);

            return CreatedAtAction(nameof(RemovePermission), roleAccessLink);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [ProducesResponseType(typeof(List<string>), 200)]
    [ProducesErrorResponseType(typeof(string))]
    [HttpGet("get")]
    public async Task<IActionResult> GetRolePermissions([FromQuery] string role)
    {
        try
        {
            var roleAccessLink = await _roleAccessLinkService.GetByRole(role);

            return Ok(roleAccessLink);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet("getall")]
    public async Task<IActionResult> GetAllRoleAccessLink()
    {
        try
        {
            var roleAccessLink = await _roleAccessLinkService.GetAll();

            return Ok(roleAccessLink);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
