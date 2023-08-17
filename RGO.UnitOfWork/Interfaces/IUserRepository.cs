using RGO.Models;
using RGO.UnitOfWork.Entities;

namespace RGO.UnitOfWork.Interfaces;

public interface IUserRepository : IRepository<User, UserDto>
{
    Task<UserDto> GetUserByEmail(string email);

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

    /// Get user roles
    /// </summary>
    /// <param name="email"></param>
    /// <returns>list of integers representing UserRoles</returns>
    Task<List<int>> GetUserRoles(string email);

    Task<UserDto> GetByEmail(string email);
}

