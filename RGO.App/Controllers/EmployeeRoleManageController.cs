using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.App.Controllers;

[Route("/employeerolemanager/")]
[ApiController]
public class EmployeeRoleManageController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IEmployeeRoleService _employeeRoleService;
    private readonly IRoleService _roleService;

    public EmployeeRoleManageController(IEmployeeRoleService employeeRoleService, IEmployeeService employeeService, IRoleService roleService)
    {
        _employeeRoleService = employeeRoleService;
        _employeeService = employeeService;
        _roleService = roleService;
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpPost("add")]
    public async Task<IActionResult> AddRole([FromQuery] string email, [FromQuery] string role)
    {
        try
        {
            var employee = await _employeeService.GetEmployee(email);

            var currRole = await _roleService.CheckRole(role) ?
                await _roleService.GetRole(role) :
                await _roleService.SaveRole(new RoleDto(0, role));

            var employeeRole = new EmployeeRoleDto(0, employee, currRole);

            var employeeRoleSaved = await _employeeRoleService.SaveEmployeeRole(employeeRole);

            return CreatedAtAction(nameof(AddRole), employeeRoleSaved);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [ProducesResponseType(typeof(EmployeeRoleDto), 200)]
    [ProducesErrorResponseType(typeof(string))]
    [HttpDelete("remove")]
    public async Task<IActionResult> RemoveRole([FromQuery] string email, [FromQuery] string role)
    {
        try
        {
            var employee = await _employeeService.GetEmployee(email);

            var roleToRemove = await _roleService.GetRole(role);

            var employeeRole = await _employeeRoleService.GetEmployeeRole(email, role);

            var employeeRoleRemoved = await _employeeRoleService.DeleteEmployeeRole(email, role);

            return Ok(employeeRoleRemoved);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet("get")]
    public async Task<IActionResult> GetEmployeeRole([FromQuery] string email)
    {
        try
        {
            var employeeRoles = await _employeeRoleService.GetEmployeeRoles(email);

            var roles = employeeRoles
                .Select(employeeRole => employeeRole.Role!.Description)
                .ToList();

            return Ok(roles);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllRoles()
    {
        try
        {
            var roles = await _roleService.GetAll();

            var rolesDescriptions = roles
                .Select(role => role.Description)
                .ToList();

            return Ok(rolesDescriptions);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
