using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RGO.Models;
using RGO.Services.Interfaces;

namespace RGO.App.Controllers;

[Route("/employeerolemanage/")]
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

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [ProducesResponseType(typeof(Dictionary<string, List<string>>), 200)]
    [ProducesErrorResponseType(typeof(string))]
    [HttpGet("get")]
    public async Task<IActionResult> GetEmployeeRole([FromQuery] string email)
    {
        try
        {
            var employeeRole = await _employeeRoleService.GetEmployeeRoles(email);

            Dictionary<string, List<string>> listOfRoles = employeeRole
                            .GroupBy(e => e.Employee!.Email)
                            .ToDictionary(
                                e => e.Key,
                                e => e.Select(employeeRole => employeeRole.Role!.Description).Distinct().ToList());

            return base.Ok(listOfRoles);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [ProducesResponseType(201)]
    [ProducesErrorResponseType(typeof(string))]
    [HttpPost("assign/role")]
    public async Task<IActionResult> AssignRole(
        [FromQuery] string email,
        [FromQuery] string role)
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
            var employeeRole = new EmployeeRoleDto(
                0,
                await _employeeService.GetEmployee(email),
                roleDto);

            var newEmployeeRole = await _employeeRoleService.SaveEmployeeRole(employeeRole);

            return CreatedAtAction(nameof(AssignRole), new { email = newEmployeeRole.Employee!.Email, role = newEmployeeRole.Role!.Description }, newEmployeeRole);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrEmployeePolicy")]
    [ProducesResponseType(typeof(EmployeeRoleDto), 200)]
    [ProducesErrorResponseType(typeof(string))]
    [HttpDelete("unassign/role")]
    public async Task<IActionResult> UnassignRole(
        [FromQuery] string email,
        [FromQuery] string role)
    {
        try
        {
            var deletedEmployeeRole = await _employeeRoleService.DeleteEmployeeRole(email, role);

            return CreatedAtAction(nameof(UnassignRole), deletedEmployeeRole);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
