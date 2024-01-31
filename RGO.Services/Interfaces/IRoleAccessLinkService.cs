using RGO.Models;
namespace RGO.Services.Interfaces;

public interface IRoleAccessLinkService
{
    /// <summary>
    /// Get All Role Access Links
    /// </summary>
    /// <returns>Dictionary with role as the key and permissions</returns>
    Task<Dictionary<string, List<string>>> GetAll();

    /// <summary>
    /// Get Role Access Link By Role
    /// </summary>
    /// <param name="role"></param>
    /// <returns>Dictionary with role as the key and permissions</returns>
    Task<Dictionary<string, List<string>>> GetByRole(string role);

    /// <summary>
    /// Get Role Access Link By Permission
    /// </summary>
    /// <param name="permission"></param>
    /// <returns></returns>
    Task<Dictionary<string, List<string>>> GetByPermission(string permission);

    /// <summary>
    /// Save Role Access Link
    /// </summary>
    /// <param name="roleAccessLinkDto"></param>
    /// <returns></returns>
    Task<RoleAccessLinkDto> Save(RoleAccessLinkDto roleAccessLinkDto);

    /// <summary>
    /// Update Role Access Link
    /// </summary>
    /// <param name="roleAccessLinkDto"></param>
    /// <returns></returns>
    Task<RoleAccessLinkDto> Update(RoleAccessLinkDto roleAccessLinkDto);

    /// <summary>
    /// Delete Role Access Link
    /// </summary>
    /// <param name="role"></param>
    /// <param name="permission"></param>
    /// <returns></returns>
    Task<RoleAccessLinkDto> Delete(string role, string permission);

    /// <summary>
    /// Get Role By Employee
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<Dictionary<string, List<string>>> GetRoleByEmployee(string email);

    /// <summary>
    /// Get All RoleAccess Links
    /// </summary>
    /// <returns>RoleAccessLinkDto</returns>
    Task<List<RoleAccessLinkDto>> GetAllRoleAccessLink();
}
