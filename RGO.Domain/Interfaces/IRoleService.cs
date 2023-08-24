using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IRoleService
{
    /// <summary>
    /// Get Role
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<RoleDto> GetRole(string name);

    /// <summary>
    /// Get All Role
    /// </summary>
    /// <returns></returns>
    Task<List<RoleDto>> GetAll();

    /// <summary>
    /// Save Role
    /// </summary>
    /// <param name="roleDto"></param>
    /// <returns></returns>
    Task<RoleDto> SaveRole(RoleDto roleDto);

    /// <summary>
    /// Update Role
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<RoleDto> UpdateRole(string name);

    /// <summary>
    /// Delete Role
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<RoleDto> DeleteRole(string name);
}
