﻿using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.App.Controllers.HRIS;

[Route("employee-role-manager")]
[ApiController]
public class EmployeeRoleManageController : ControllerBase
{
    private readonly IEmployeeRoleService? _employeeRoleService;
    private readonly IEmployeeService? _employeeService;
    private readonly IRoleService? _roleService;

    public EmployeeRoleManageController(IEmployeeRoleService? employeeRoleService, IEmployeeService? employeeService,
                                        IRoleService? roleService)
    {
        _employeeRoleService = employeeRoleService;
        _employeeService = employeeService;
        _roleService = roleService;
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPost]
    public async Task<IActionResult> AddRole([FromQuery] string email, [FromQuery] string role)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role)) return BadRequest("Invalid input");

        if(_employeeRoleService == null || _employeeService == null || _roleService == null)
        {
            return BadRequest("Invalid input");
        }

        try
        {
            var employee = await _employeeService.GetEmployee(email);

            var currRole = await _roleService.CheckRole(role)
                ? await _roleService.GetRole(role)
                : await _roleService.SaveRole(new RoleDto { Id = 0, Description = role });

            var employeeRole = new EmployeeRoleDto{ Id = 0, Employee = employee, Role = currRole };

            var employeeRoleSaved = await _employeeRoleService.SaveEmployeeRole(employeeRole);

            return CreatedAtAction(nameof(AddRole), employeeRoleSaved);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrSuperAdminPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateRole([FromQuery] string email, [FromQuery] string role)
    {
        if (_employeeRoleService == null || _employeeService == null || _roleService == null)
        {
            return BadRequest("Invalid input");
        }

        try
        {
            var employee = await _employeeService.GetEmployee(email);

            var currRole = await _roleService.CheckRole(role)
                ? await _roleService.GetRole(role)
                : await _roleService.SaveRole(new RoleDto{ Id = 0, Description = role });

            var currEmployeeRole = await _employeeRoleService.GetEmployeeRole(email);
            var employeeRole = new EmployeeRoleDto { Id = currEmployeeRole.Id, Employee = employee, Role = currRole };

            var employeeRoleSaved = await _employeeRoleService.UpdateEmployeeRole(employeeRole);

            return CreatedAtAction(nameof(UpdateRole), employeeRoleSaved);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [ProducesResponseType(typeof(EmployeeRoleDto), 200)]
    [ProducesErrorResponseType(typeof(string))]
    [HttpDelete]
    public async Task<IActionResult> RemoveRole([FromQuery] string email, [FromQuery] string role)
    {
        if (_employeeRoleService == null || _employeeService == null || _roleService == null)
        {
            return BadRequest("Invalid input");
        }

        try
        {
            var employee = await _employeeService.GetEmployee(email);

            var roleToRemove = await _roleService.GetRole(role);

            var employeeRole = await _employeeRoleService.GetEmployeeRole(email);

            var employeeRoleRemoved = await _employeeRoleService.DeleteEmployeeRole(email, role);

            return Ok(employeeRoleRemoved);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetEmployeeRole([FromQuery] string email)
    {
        if (_employeeRoleService == null)
        {
            return BadRequest("Invalid input");
        }

        try
        {
            var employeeRoles = await _employeeRoleService.GetEmployeeRole(email);
            string[] role = { employeeRoles.Role!.Description! };

            return Ok(role);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllRoles()
    {
        if (_roleService == null)
        {
            return BadRequest("Invalid input");
        }

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

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("get-role")]
    public async Task<IActionResult> GetAllEmployeeOnRoles([FromQuery] int roleId)
    {
        if (_employeeRoleService == null)
        {
            return BadRequest("Invalid input");
        }

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
