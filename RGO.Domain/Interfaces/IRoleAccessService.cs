using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IRoleAccessService
{
    /// <summary>
    /// Save Role Access
    /// </summary>
    /// <param name="roleAccessDto"></param>
    /// <returns></returns>
    Task<RoleAccessDto> SaveRoleAccess(RoleAccessDto roleAccessDto);

    /// <summary>
    /// Delete Role Access
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    Task<RoleAccessDto> DeleteRoleAccess(string action);

    /// <summary>
    /// Get Role Access
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    Task<RoleAccessDto> GetRoleAccess(string action);

    /// <summary>
    /// Get Role Access By Role
    /// </summary>
    /// <param name="description"></param>
    /// <returns></returns>
    Task<List<RoleAccessDto>> GetRoleAccessByRole(string description);

    /// <summary>
    /// Get All Role Access
    /// </summary>
    /// <returns></returns>
    Task<List<RoleAccessDto>> GetAllRoleAccess();

    /// <summary>
    /// Update Role Access
    /// </summary>
    /// <param name="roleAccessDto"></param>
    /// <returns></returns>
    Task<RoleAccessDto> UpdateRoleAccess(RoleAccessDto roleAccessDto);
}