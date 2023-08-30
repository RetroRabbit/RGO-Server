using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;
using System.Security.Claims;

namespace RGO.App.Controllers;

[Route("/rolemanage/")]
[ApiController]
public class RoleManageController : ControllerBase
{
    private readonly IRoleAccessLinkService _roleAccessLinkService;
    private readonly IEmployeeService _employeeService;
    private readonly IEmployeeRoleService _employeeRoleService;
    private readonly IRoleService _roleService;
    private readonly IRoleAccessService _roleAccessService;

    public RoleManageController(
        IEmployeeRoleService employeeRoleService,
        IEmployeeService employeeService,
        IRoleAccessLinkService roleAccessLinkService,
        IRoleService roleService,
        IRoleAccessService roleAccessService)
    {
        _employeeRoleService = employeeRoleService;
        _employeeService = employeeService;
        _roleAccessLinkService = roleAccessLinkService;
        _roleService = roleService;
        _roleAccessService = roleAccessService;
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpPost("add")]
    public async Task<IActionResult> AddRoleAccessLink([FromQuery] string? email,
        [FromBody] RoleAccessLinkDto newRoleAccessLink)
    {
        try
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            string emailToUse = email ??
                claimsIdentity!.FindFirst(ClaimTypes.Email)!.Value;

            var employee = await _employeeService.GetEmployee(emailToUse);

            var role = await _roleService.CheckRole(newRoleAccessLink.Role.Description) ?
                await _roleService.GetRole(newRoleAccessLink.Role.Description) :
                await _roleService.SaveRole(newRoleAccessLink.Role);

            var employeRole = await _employeeRoleService.CheckEmployeeRole(emailToUse, role.Description) ?
                await _employeeRoleService.GetEmployeeRole(emailToUse, role.Description) :
                await _employeeRoleService.SaveEmployeeRole(new EmployeeRoleDto(0, employee, role));

            var roleAccess = await _roleAccessService.CheckRoleAccess(newRoleAccessLink.RoleAccess.Permission) ?
                await _roleAccessService.GetRoleAccess(newRoleAccessLink.RoleAccess.Permission) :
                await _roleAccessService.SaveRoleAccess(newRoleAccessLink.RoleAccess);

            var roleAccessLink = await _roleAccessLinkService.Save(new RoleAccessLinkDto(0,role, roleAccess));

            return CreatedAtAction(nameof(AddRoleAccessLink), new { role = roleAccessLink.Role.Description, permission = roleAccessLink.RoleAccess.Permission }, roleAccessLink);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [HttpGet("get")]
    public async Task<IActionResult> GetRoleAccessLink([FromQuery] string? email)
    {
        try
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;

            string emailToUse = email ??
                claimsIdentity!.FindFirst(ClaimTypes.Email)!.Value;

            var employee = await _employeeService.GetEmployee(emailToUse);

            var roleAccessLink = await _roleAccessLinkService.GetRoleByEmployee(emailToUse);

            return Ok(roleAccessLink);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteRoleAccessLink([FromQuery] string role, [FromQuery] string access)
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
