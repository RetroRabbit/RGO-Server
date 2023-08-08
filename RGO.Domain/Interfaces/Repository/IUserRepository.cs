using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Repository;

public interface IUserRepository
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
    /// Get All Users
    /// </summary>
    /// <param></param>
    /// <returns>A list of UserDto</returns>
    Task<List<UserDto>> GetUsers();
}
