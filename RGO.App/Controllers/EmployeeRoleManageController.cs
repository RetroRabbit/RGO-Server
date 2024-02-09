using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork.Entities;

namespace RGO.App.Controllers;

[Route("employee-role-manager")]
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
    [HttpPost()]
    public async Task<IActionResult> AddRole([FromQuery] string email, [FromQuery] string role)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role))
        {
            return BadRequest("Invalid input");
        }

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
    [HttpPost()]
    public async Task<IActionResult> UpdateRole([FromQuery] string email, [FromQuery] string role)
    {
        try
        {
            var employee = await _employeeService.GetEmployee(email);

            var currRole = await _roleService.CheckRole(role) ?
                await _roleService.GetRole(role) :
                await _roleService.SaveRole(new RoleDto(0, role));

            var currEmployeeRole =  await _employeeRoleService.GetEmployeeRole(email);
            var employeeRole = new EmployeeRoleDto(currEmployeeRole.Id, employee, currRole);

            var employeeRoleSaved = await _employeeRoleService.UpdateEmployeeRole(employeeRole);

            return CreatedAtAction(nameof(UpdateRole), employeeRoleSaved);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [ProducesResponseType(typeof(EmployeeRoleDto), 200)]
    [ProducesErrorResponseType(typeof(string))]
    [HttpDelete()]
    public async Task<IActionResult> RemoveRole([FromQuery] string email, [FromQuery] string role)
    {
        try
        {
            var employeeRoleRemoved = await _employeeRoleService.DeleteEmployeeRole(email, role);

            return Ok(employeeRoleRemoved);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet()]
    public async Task<IActionResult> GetEmployeeRole([FromQuery] string email)
    {
        try
        {
            var employeeRoles = await _employeeRoleService.GetEmployeeRole(email);
            string[] role = { employeeRoles.Role.Description };

            return Ok(role);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet()]
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

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpGet("get-role")]
    public async Task<IActionResult> GetAllEmployeeOnRoles([FromQuery] int roleId)
    {
        try
        {
            var roles = await _employeeRoleService.GetAllEmployeeOnRoles(roleId);
            return Ok(roles);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}
