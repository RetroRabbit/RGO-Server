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

        Task<UserDto> GetUserByEmail(string email);
    }
}