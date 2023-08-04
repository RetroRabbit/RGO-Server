using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Repository
{
    public interface IUserRepository
    {
        Task<bool> UserExists(string email);
        Task<UserDto> GetUserByEmail(string email);

        Task<UserDto> AddUser(UserDto user);
    }
}
