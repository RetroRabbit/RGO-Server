using RGO.Models;

namespace RGO.Services.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Adds a user
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        Task<UserDto> AddUser(UserDto userDto);

        /// <summary>
        /// Get a user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<UserDto> GetUserByEmail(string email);
        
        /// <summary>
        /// Get All users
        /// </summary>
        /// <param></param>
        /// <returns>A list of UserDto</returns>
        Task<List<UserDto>> GetUsers();

        /// <summary>
        /// Retrieves Grad Groups
        /// </summary>
        /// <returns>A list of Grad Groups</returns>
        Task<List<GradGroupDto>> GetGradGroups();

        /// <summary>
        /// Removes a user
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task<UserDto> RemoveUser(string email);
    }
}