using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
namespace HRIS.Services.Interfaces;

public interface IAuthService
{
    Task<IPagedList<Role>> GetAllRolesAsync();

    Task<IPagedList<User>> GetAllUsersAsync();

    Task<IList<User>> GetUsersByEmailAsync(string email);

    Task<IPagedList<AssignedUser>> GetUsersByRoleAsync(string roleId);

    Task<IPagedList<Role>> GetUserRolesAsync(string userId);

    Task<bool> AddRoleToUserAsync(string userId, string roleId);

    Task<bool> RemoveRoleFromUserAsync(string userId, string roleId);

    Task<Role> CreateRoleAsync(string roleName, string description);

    Task<bool> DeleteRoleAsync(string roleId);

    Task<bool> UpdateRoleAsync(string roleId, string roleName, string description);

    Task<IPagedList<Permission>> GetPermissionsByRoleAsync(string roleId);

    Task<bool> RemovePermissionsFromRoleAsync(string roleId, string permissionName);

    Task<bool> AddPermissionsToRoleAsync(string roleId, string permissionName);

    Task<bool> CreateUser(UserCreateRequest request);

    Task<User> GetUserById(string userId);

    Task<bool> DeleteUser(string userId);

    Task<bool> UpdateUser(string userId, UserUpdateRequest request);

}