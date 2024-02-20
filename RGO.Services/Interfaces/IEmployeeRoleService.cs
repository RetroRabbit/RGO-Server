using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IEmployeeRoleService
{
    /// <summary>
    ///     Save Employee Role
    /// </summary>
    /// <param name="employeeRoleDto"></param>
    /// <returns></returns>
    Task<EmployeeRoleDto> SaveEmployeeRole(EmployeeRoleDto employeeRoleDto);

    /// <summary>
    ///     Delete Employee Role
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<EmployeeRoleDto> DeleteEmployeeRole(string name, string role);

    /// <summary>
    ///     Get Employee Role
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<EmployeeRoleDto> GetEmployeeRole(string email);

    /// <summary>
    ///     Get All Employee Roles
    /// </summary>
    /// <returns></returns>
    Task<List<EmployeeRoleDto>> GetAllEmployeeRoles();

    /// <summary>
    ///     Update Employee Role
    /// </summary>
    /// <param name="employeeRoleDto"></param>
    /// <returns></returns>
    Task<EmployeeRoleDto> UpdateEmployeeRole(EmployeeRoleDto employeeRoleDto);

    /// <summary>
    ///     Get Employee Roles
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<List<EmployeeRoleDto>> GetEmployeeRoles(string email);

    /// <summary>
    ///     Check Employee Role
    /// </summary>
    /// <param name="email"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    Task<bool> CheckEmployeeRole(string email, string role);

    /// <summary>
    ///     Get All Employee on Roles
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns>List of EmployeeRoleDtos</returns>
    Task<List<EmployeeRoleDto>> GetAllEmployeeOnRoles(int roleId);
}