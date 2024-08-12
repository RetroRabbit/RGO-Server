using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("role-manager")]
[ApiController]
public class RoleManageController : ControllerBase
{
    private readonly IRoleAccessLinkService _roleAccessLinkService;
    private readonly IRoleAccessService _roleAccessService;
    private readonly IRoleService _roleService;

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
    [HttpPost]
    public async Task<IActionResult> AddPermission([FromQuery] string role, [FromQuery] string permission,
                                                   [FromQuery] string grouping)
    {
        var foundRole = await _roleService.CheckRole(role)
                ? await _roleService.GetRole(role)
                : await _roleService.CreateRole(new RoleDto { Id = 0, Description = role });

        var roleAccess = await _roleAccessService.CheckRoleAccess(permission)
            ? await _roleAccessService.GetRoleAccess(permission)
            : await _roleAccessService.CreateRoleAccess(new RoleAccessDto { Id = 0, Permission = permission, Grouping = grouping });

        var roleAccessLink = await _roleAccessLinkService.Create(new RoleAccessLinkDto { Id = 0, Role = foundRole, RoleAccess = roleAccess });

        return CreatedAtAction(nameof(AddPermission), roleAccessLink);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [ProducesResponseType(typeof(RoleAccessLinkDto), 200)]
    [ProducesErrorResponseType(typeof(string))]
    [HttpDelete]
    public async Task<IActionResult> RemovePermission([FromQuery] string role, [FromQuery] string permission,
                                                      string grouping)
    {
            var roleAccess = await _roleAccessService.CheckRoleAccess(permission)
                ? await _roleAccessService.GetRoleAccess(permission)
                : await _roleAccessService.CreateRoleAccess(new RoleAccessDto { Id = 0, Permission = permission, Grouping = grouping });

            var roleAccessLink = await _roleAccessLinkService.Delete(role, permission);

            return CreatedAtAction(nameof(RemovePermission), roleAccessLink);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [ProducesResponseType(typeof(List<string>), 200)]
    [ProducesErrorResponseType(typeof(string))]
    [HttpGet("permissions")]
    public async Task<IActionResult> GetRolePermissions([FromQuery] string role)
    {
        var roleAccessLink = await _roleAccessLinkService.GetByRole(role);

        return Ok(roleAccessLink);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetAllRoleAccessLink()
    {
        var roleAccessLink = await _roleAccessLinkService.GetAll();

        return Ok(roleAccessLink);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet("role-access-links")]
    public async Task<IActionResult> GetAllRoleAccessLinks()
    {
        var roleAccessLink = await _roleAccessLinkService.GetAllRoleAccessLink();

        return Ok(roleAccessLink);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet("role-accesses")]
    public async Task<IActionResult> GetAllRoleAccesses()
    {
        var roleAccesses = await _roleAccessService.GetAllRoleAccess();

        return Ok(roleAccesses);
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet("roles")]
    public async Task<IActionResult> GetAllRoles()
    {
        var roles = await _roleService.GetAll();

        return Ok(roles);
    }
}