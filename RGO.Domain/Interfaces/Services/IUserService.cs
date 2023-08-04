using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto> AddUser(UserDto userDto);
    }
}