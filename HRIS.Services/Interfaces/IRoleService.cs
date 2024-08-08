using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IRoleService
{
    /// <summary>
    ///     Get Role
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<RoleDto> GetRole(string name);

    /// <summary>
    ///     Get All Role
    /// </summary>
    /// <returns></returns>
    Task<List<RoleDto>> GetAll();

    /// <summary>
    ///     Create Role
    /// </summary>
    /// <param name="roleDto"></param>
    /// <returns></returns>
    Task<RoleDto> CreateRole(RoleDto roleDto);

    /// <summary>
    ///     Update Role
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<RoleDto> UpdateRole(string name);

    /// <summary>
    ///     Delete Role
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<RoleDto> DeleteRole(int roleId);

    /// <summary>
    ///     Check Role exists
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<bool> CheckRole(string name);
}