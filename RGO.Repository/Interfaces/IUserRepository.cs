using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.Repository.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<bool> UserExists(string email);
    Task<UserDto> GetUserByEmail(string email);

    /// <summary>
    /// Add a new user 
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<UserDto> AddUser(UserDto user);

    /// <summary>
    /// Update User Profile 
    /// </summary>
    /// <param name="email"></param>
    /// <param name="updatedProfile"></param>
    /// <returns>Updated user profile</returns>
    Task<UserDto> UpdateUser(string email, ProfileDto updatedProfile);

    /// <summary>
    /// Get All Users
    /// </summary>
    /// <param></param>
    /// <returns>A list of UserDto</returns>
    Task<List<UserDto>> GetUsers();

    /// <summary>
    /// Removes a user
    /// </summary>
    /// <param name="email"></param>
    /// <returns>returns deleted user when successful</returns>
    Task<UserDto> RemoveUser(string email);

    /// Get user roles
    /// </summary>
    /// <param name="email"></param>
    /// <returns>list of integers representing UserRoles</returns>
    Task<List<int>> GetUserRoles(string email);
}

