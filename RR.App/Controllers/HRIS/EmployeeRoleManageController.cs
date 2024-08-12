using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RR.App.Controllers.HRIS;

[Route("employee-role-manager")]
[ApiController]
public class EmployeeRoleManageController : ControllerBase
{
    private readonly IEmployeeRoleService? _employeeRoleService;
    private readonly IEmployeeService? _employeeService;
    private readonly IRoleService? _roleService;
    private readonly IAuthService _authService;

    public EmployeeRoleManageController(IEmployeeRoleService? employeeRoleService, IEmployeeService? employeeService,
                                        IRoleService? roleService, IAuthService authService)
    {
        _employeeRoleService = employeeRoleService;
        _employeeService = employeeService;
        _roleService = roleService;
        _authService = authService;
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPost]
    public async Task<IActionResult> AddRole([FromQuery] string email, [FromQuery] string role)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(role))
            throw new CustomException("Invalid input");

        var employee = await _employeeService.GetEmployeeByEmail(email);
        if (employee == null)
            throw new CustomException("Employee not found.");

        var authUser = await _authService.GetUsersByEmailAsync(email);
        if (authUser == null || !authUser.Any())
            throw new CustomException("User not found in authentication service.");

        var authEmployeeId = authUser.First().UserId;

        var currRole = await _roleService.CheckRole(role)
            ? await _roleService.GetRole(role)
            : await _roleService.CreateRole(new RoleDto { Id = 0, Description = role });

        await _authService.AddRoleToUserAsync(authEmployeeId, currRole.AuthRoleId);

        var employeeRole = new EmployeeRoleDto { Id = 0, Employee = employee, Role = currRole };

        var employeeRoleSaved = await _employeeRoleService.SaveEmployeeRole(employeeRole);

        return CreatedAtAction(nameof(AddRole), employeeRoleSaved);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpPut]
    public async Task<IActionResult> UpdateRole([FromQuery] string email, [FromQuery] string role)
    {
        var employee = await _employeeService.GetEmployeeByEmail(email);
        if (employee == null)
            throw new CustomException("Employee not found.");

        var authUser = await _authService.GetUsersByEmailAsync(email);
        if (authUser == null || !authUser.Any())
            throw new CustomException("User not found in authentication service.");

        var authEmployeeId = authUser.First().UserId;

        var changingToRole = await _roleService.CheckRole(role)
            ? await _roleService.GetRole(role)
            : await _roleService.CreateRole(new RoleDto { Id = 0, Description = role });

        var userRoleIsFoundInAuth0 = await _authService.AddRoleToUserAsync(authEmployeeId, changingToRole.AuthRoleId);

        if (!userRoleIsFoundInAuth0)
            throw new CustomException("Auth role not found in the database.");

        var authRoles = await _authService.GetUserRolesAsync(authEmployeeId);
        if (authRoles != null && authRoles.Any())
        {
            var authPreviousRole = authRoles.First(authRole => authRole.Description != role);
            await _authService.RemoveRoleFromUserAsync(authEmployeeId, authPreviousRole.Id);
        }

        var currEmployeeRole = await _employeeRoleService.GetEmployeeRole(email);

        var updatedEmployeeRole = new EmployeeRoleDto
        {
            Id = currEmployeeRole.Id,
            Employee = employee,
            Role = changingToRole
        };

        var savedEmployeeRole = await _employeeRoleService.UpdateEmployeeRole(updatedEmployeeRole);
        return CreatedAtAction(nameof(UpdateRole), savedEmployeeRole);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [ProducesResponseType(typeof(EmployeeRoleDto), 200)]
    [ProducesErrorResponseType(typeof(string))]
    [HttpDelete]
    public async Task<IActionResult> RemoveRole([FromQuery] string email, [FromQuery] string role)
    {
        var employee = await _employeeService.GetEmployeeByEmail(email);
        if (employee == null)
            throw new CustomException("Employee not found.");


        var roleToRemove = await _roleService.GetRole(role);
        if (roleToRemove == null)
            throw new CustomException("Role not found.");

        var authUser = await _authService.GetUsersByEmailAsync(email);
        if (authUser == null || !authUser.Any())
            throw new CustomException("User not found in authentication service.");

        var authEmployeeId = authUser.First().UserId;
        var authRoles = await _authService.GetUserRolesAsync(authEmployeeId);
        if (authRoles == null)
            throw new CustomException("User roles not found in authentication service.");

        var authRoleToRemove = authRoles.FirstOrDefault(r => r.Id == roleToRemove.AuthRoleId);
        if (authRoleToRemove != null)
            await _authService.RemoveRoleFromUserAsync(authEmployeeId, authRoleToRemove.Id);

        var employeeRole = await _employeeRoleService.GetEmployeeRole(email);
        if (employeeRole == null)
            throw new CustomException("Employee role not found.");

        var employeeRoleRemoved = await _employeeRoleService.DeleteEmployeeRole(email, role);

        return Ok(employeeRoleRemoved);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet]
    public async Task<IActionResult> GetEmployeeRole([FromQuery] string email)
    {
        if (_employeeRoleService == null)
            throw new CustomException("Invalid input");

        var employeeRoles = await _employeeRoleService.GetEmployeeRole(email);
        string[] role = { employeeRoles.Role!.Description! };

        return Ok(role);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllRoles()
    {
        if (_roleService == null)
            throw new CustomException("Invalid input");

        var roles = await _roleService.GetAll();

        var rolesDescriptions = roles
                                .Select(role => role.Description)
                                .ToList();

        return Ok(rolesDescriptions);
    }

    [Authorize(Policy = "AdminOrTalentOrJourneyOrSuperAdminPolicy")]
    [HttpGet("get-role")]
    public async Task<IActionResult> GetAllEmployeeOnRoles([FromQuery] int roleId)
    {
        if (_employeeRoleService == null)
            throw new CustomException("Invalid input");

        var roles = await _employeeRoleService.GetAllEmployeeOnRoles(roleId);
        return Ok(roles);
    }
}
