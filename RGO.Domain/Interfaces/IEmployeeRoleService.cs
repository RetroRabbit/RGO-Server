using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IEmployeeRoleService
{
    /// <summary>
    /// Save Employee Role
    /// </summary>
    /// <param name="employeeRoleDto"></param>
    /// <returns></returns>
    Task<EmployeeRoleDto> SaveEmployeeRole(EmployeeRoleDto employeeRoleDto);

    /// <summary>
    /// Delete Employee Role
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<EmployeeRoleDto> DeleteEmployeeRole(string name, string role);

    /// <summary>
    /// Get Employee Role
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<EmployeeRoleDto> GetEmployeeRole(string email, string role);

    /// <summary>
    /// Get All Employee Roles
    /// </summary>
    /// <returns></returns>
    Task<List<EmployeeRoleDto>> GetAllEmployeeRoles();

    /// <summary>
    /// Update Employee Role
    /// </summary>
    /// <param name="employeeRoleDto"></param>
    /// <returns></returns>
    Task<EmployeeRoleDto> UpdateEmployeeRole(EmployeeRoleDto employeeRoleDto);

    Task<List<EmployeeRoleDto>> GetEmployeeRoles(string email);

    Task<bool> CheckEmployeeRole(string email, string role);
}
