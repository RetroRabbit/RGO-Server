using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IRoleAccessLinkService
{
    /// <summary>
    ///     Get All Role Access Links
    /// </summary>
    /// <returns>Dictionary with role as the key and permissions</returns>
    Task<Dictionary<string, List<string>>> GetAll();

    /// <summary>
    ///     Get Role Access Link By Role
    /// </summary>
    /// <param name="role"></param>
    /// <returns>Dictionary with role as the key and permissions</returns>
    Task<Dictionary<string, List<string>>> GetByRole(string role);

    /// <summary>
    ///     Get Role Access Link By Permission
    /// </summary>
    /// <param name="permission"></param>
    /// <returns></returns>
    Task<Dictionary<string, List<string>>> GetByPermission(string permission);

    /// <summary>
    ///     Save Role Access Link
    /// </summary>
    /// <param name="roleAccessLinkDto"></param>
    /// <returns></returns>
    Task<RoleAccessLinkDto> Create(RoleAccessLinkDto roleAccessLinkDto);

    /// <summary>
    ///     Update Role Access Link
    /// </summary>
    /// <param name="roleAccessLinkDto"></param>
    /// <returns></returns>
    Task<RoleAccessLinkDto> Update(RoleAccessLinkDto roleAccessLinkDto);

    /// <summary>
    ///     Delete Role Access Link
    /// </summary>
    /// <param name="role"></param>
    /// <param name="permission"></param>
    /// <returns></returns>
    Task<RoleAccessLinkDto> Delete(string role, string permission);

    /// <summary>
    ///     Get Role By Employee
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<Dictionary<string, List<string>>> GetRoleByEmployee(string email);

    /// <summary>
    ///     Get All RoleAccess Links
    /// </summary>
    /// <returns>RoleAccessLinkDto</returns>
    Task<List<RoleAccessLinkDto>> GetAllRoleAccessLink();

    /// <summary>
    ///     Check if RoleAccessLink exists
    /// </summary>
    /// <param name="role"></param>
    /// <param name="permission"></param>
    /// <returns></returns>
    Task<bool> CheckRoleAccessLink(string role, string permission);

    /// <summary>
    ///     Check if Role Exists
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<bool> CheckRole(string name);

    /// <summary>
    ///     Check if Role Access Exists
    /// </summary>
    /// <param name="permission"></param>
    /// <returns></returns>
    Task<bool> CheckRoleAccess(string permission);

    /// <summary>
    ///     Check if Employee exists
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<bool> CheckEmployee(string email);
}