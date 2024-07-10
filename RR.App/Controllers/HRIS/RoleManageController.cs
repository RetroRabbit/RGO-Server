using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("role-manager")]
[ApiController]
public class RoleManageController : ControllerBase
{
    private readonly AuthorizeIdentity _identity;
    private readonly IRoleAccessLinkService? _roleAccessLinkService;
    private readonly IRoleAccessService? _roleAccessService;
    private readonly IRoleService? _roleService;

    public RoleManageController(
        IRoleAccessLinkService? roleAccessLinkService,
        IRoleService? roleService,
        IRoleAccessService? roleAccessService)
    {
        _roleAccessLinkService = roleAccessLinkService;
        _roleService = roleService;
        _roleAccessService = roleAccessService;
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpPost]
    public async Task<IActionResult> AddPermission([FromQuery] string role, [FromQuery] string permission,
                                                   [FromQuery] string grouping)
    {
        if (_roleService == null || _roleAccessService == null || _roleAccessLinkService == null)
        {
            return BadRequest("Invalid input");
        }

        var foundRole = await _roleService.CheckRole(role)
                ? await _roleService.GetRole(role)
                : await _roleService.SaveRole(new RoleDto { Id = 0, Description = role });

        var roleAccess = await _roleAccessService.CheckRoleAccess(permission)
            ? await _roleAccessService.GetRoleAccess(permission)
            : await _roleAccessService.SaveRoleAccess(new RoleAccessDto { Id = 0, Permission = permission, Grouping = grouping });

        var roleAccessLink = await _roleAccessLinkService.Save(new RoleAccessLinkDto { Id = 0, Role = foundRole, RoleAccess = roleAccess });

        return CreatedAtAction(nameof(AddPermission), roleAccessLink);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [ProducesResponseType(typeof(RoleAccessLinkDto), 200)]
    [ProducesErrorResponseType(typeof(string))]
    [HttpDelete]
    public async Task<IActionResult> RemovePermission([FromQuery] string role, [FromQuery] string permission,
                                                      string grouping)
    {
        if (_roleService == null || _roleAccessService == null || _roleAccessLinkService == null)
        {
            return BadRequest("Invalid input");
        }

        var foundRole = await _roleService.CheckRole(role)
                ? await _roleService.GetRole(role)
                : await _roleService.SaveRole(new RoleDto{ Id = 0, Description = role});

            var roleAccess = await _roleAccessService.CheckRoleAccess(permission)
                ? await _roleAccessService.GetRoleAccess(permission)
                : await _roleAccessService.SaveRoleAccess(new RoleAccessDto { Id = 0, Permission = permission, Grouping = grouping });

            var roleAccessLink = await _roleAccessLinkService.Delete(role, permission);

            return CreatedAtAction(nameof(RemovePermission), roleAccessLink);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [ProducesResponseType(typeof(List<string>), 200)]
    [ProducesErrorResponseType(typeof(string))]
    [HttpGet("permissions")]
    public async Task<IActionResult> GetRolePermissions([FromQuery] string role)
    {
        if (_roleAccessLinkService == null)
        {
            return BadRequest("Invalid input");
        }

        var roleAccessLink = await _roleAccessLinkService.GetByRole(role);

        return Ok(roleAccessLink);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetAllRoleAccessLink()
    {
        if (_roleAccessLinkService == null)
        {
            return BadRequest("Invalid input");
        }

        var roleAccessLink = await _roleAccessLinkService.GetAll();

        return Ok(roleAccessLink);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet("role-access-links")]
    public async Task<IActionResult> GetAllRoleAccessLinks()
    {
        if (_roleAccessLinkService == null)
        {
            return BadRequest("Invalid input");
        }

        var roleAccessLink = await _roleAccessLinkService.GetAllRoleAccessLink();

        return Ok(roleAccessLink);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet("role-accesses")]
    public async Task<IActionResult> GetAllRoleAccesses()
    {
        if (_roleAccessService == null)
        {
            return BadRequest("Invalid input");
        }

        var roleAccesses = await _roleAccessService.GetAllRoleAccess();

        return Ok(roleAccesses);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet("roles")]
    public async Task<IActionResult> GetAllRoles()
    {
        if (_roleService == null)
        {
            return BadRequest("Invalid input");
        }

        var roles = await _roleService.GetAll();

        return Ok(roles);
    }
}