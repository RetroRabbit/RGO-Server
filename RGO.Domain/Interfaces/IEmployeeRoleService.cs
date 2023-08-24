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
    Task<EmployeeRoleDto> DeleteEmployeeRole(string name);

    /// <summary>
    /// Get Employee Role
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<EmployeeRoleDto> GetEmployeeRole(string name);

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
}
