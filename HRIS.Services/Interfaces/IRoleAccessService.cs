using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IRoleAccessService
{
    /// <summary>
    ///     Save Role Access
    /// </summary>
    /// <param name="roleAccessDto"></param>
    /// <returns></returns>
    Task<RoleAccessDto> SaveRoleAccess(RoleAccessDto roleAccessDto);

    /// <summary>
    ///     Delete Role Access
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    Task<RoleAccessDto> DeleteRoleAccess(string permission);

    /// <summary>
    ///     Get Role Access
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    Task<RoleAccessDto> GetRoleAccess(string permission);

    /// <summary>
    ///     Get All Role Access
    /// </summary>
    /// <returns></returns>
    Task<List<RoleAccessDto>> GetAllRoleAccess();

    /// <summary>
    ///     Update Role Access
    /// </summary>
    /// <param name="roleAccessDto"></param>
    /// <returns></returns>
    Task<RoleAccessDto> UpdateRoleAccess(RoleAccessDto roleAccessDto);

    /// <summary>
    ///     Check Role Access
    /// </summary>
    /// <param name="permission"></param>
    /// <returns></returns>
    Task<bool> CheckRoleAccess(string permission);
}