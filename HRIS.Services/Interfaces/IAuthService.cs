using Auth0.ManagementApi.Models;
using Auth0.ManagementApi.Paging;
namespace HRIS.Services.Interfaces;

public interface IAuthService
{
    /// <summary>
    ///     Get all roles
    /// </summary>
    /// <returns>Paged list of roles</returns>
    Task<IPagedList<Role>> GetAllRolesAsync();

    /// <summary>
    ///     Get all users
    /// </summary>
    /// <returns>Paged list of users</returns>
    Task<IPagedList<User>> GetAllUsersAsync();

    /// <summary>
    ///     Get users by email
    /// </summary>
    /// <param name="email">User email</param>
    /// <returns>List of users with the given email</returns>
    Task<IList<User>> GetUsersByEmailAsync(string email);

    /// <summary>
    ///     Get users by role
    /// </summary>
    /// <param name="roleId">Role ID</param>
    /// <returns>Paged list of users assigned to the given role</returns>
    Task<IPagedList<AssignedUser>> GetUsersByRoleAsync(string roleId);

    /// <summary>
    ///     Get roles of a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>Paged list of roles assigned to the given user</returns>
    Task<IPagedList<Role>> GetUserRolesAsync(string userId);

    /// <summary>
    ///     Add a role to a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="roleId">Role ID</param>
    /// <returns>true if role was added successfully, else false</returns>
    Task<bool> AddRoleToUserAsync(string userId, string roleId);

    /// <summary>
    ///     Remove a role from a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="roleId">Role ID</param>
    /// <returns>true if role was removed successfully, else false</returns>
    Task<bool> RemoveRoleFromUserAsync(string userId, string roleId);

    /// <summary>
    ///     Create a new role
    /// </summary>
    /// <param name="roleName">Role name</param>
    /// <param name="description">Role description</param>
    /// <returns>The created role</returns>
    Task<Role> CreateRoleAsync(string roleName, string description);

    /// <summary>
    ///     Delete a role
    /// </summary>
    /// <param name="roleId">Role ID</param>
    /// <returns>true if role was deleted successfully, else false</returns>
    Task<bool> DeleteRoleAsync(string roleId);

    /// <summary>
    ///     Update a role
    /// </summary>
    /// <param name="roleId">Role ID</param>
    /// <param name="roleName">Role name</param>
    /// <param name="description">Role description</param>
    /// <returns>true if role was updated successfully, else false</returns>
    Task<bool> UpdateRoleAsync(string roleId, string roleName, string description);

    /// <summary>
    ///     Get permissions of a role
    /// </summary>
    /// <param name="roleId">Role ID</param>
    /// <returns>Paged list of permissions assigned to the given role</returns>
    Task<IPagedList<Permission>> GetPermissionsByRoleAsync(string roleId);

    /// <summary>
    ///     Remove permissions from a role
    /// </summary>
    /// <param name="roleId">Role ID</param>
    /// <param name="permissionName">Permission name</param>
    /// <returns>true if permissions were removed successfully, else false</returns>
    Task<bool> RemovePermissionsFromRoleAsync(string roleId, string permissionName);

    /// <summary>
    ///     Add permissions to a role
    /// </summary>
    /// <param name="roleId">Role ID</param>
    /// <param name="permissionName">Permission name</param>
    /// <returns>true if permissions were added successfully, else false</returns>
    Task<bool> AddPermissionsToRoleAsync(string roleId, string permissionName);

    /// <summary>
    ///     Create a new user
    /// </summary>
    /// <param name="request">User creation request</param>
    /// <returns>true if user was created successfully, else false</returns>
    Task<bool> CreateUser(UserCreateRequest request);

    /// <summary>
    ///     Get a user by ID
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>User with the given ID</returns>
    Task<User> GetUserById(string userId);

    /// <summary>
    ///     Delete a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <returns>true if user was deleted successfully, else false</returns>
    Task<bool> DeleteUser(string userId);

    /// <summary>
    ///     Update a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="request">User update request</param>
    /// <returns>true if user was updated successfully, else false</returns>
    Task<bool> UpdateUser(string userId, UserUpdateRequest request);

}