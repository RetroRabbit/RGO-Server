using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Services
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
        /// Update user profile
        /// </summary>
        /// <param></param>
        /// <returns>user profile update of ProfileDto</returns>
        Task<ProfileDto> UpdateUser(string email, ProfileDto profile);
        
        /// <summary>
        /// Get All users
        /// </summary>
        /// <param></param>
        /// <returns>A list of UserDto</returns>
        Task<List<UserDto>> GetUsers();

        /// <summary>
        /// Retrieves User Groups
        /// </summary>
        /// <returns>A list of Grad Groups</returns>
        Task<List<GradGroupDto>> GetGradGroups();
    }
}