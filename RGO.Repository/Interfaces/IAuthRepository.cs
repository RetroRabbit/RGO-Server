using RGO.Models;

namespace RGO.Repository.Interfaces;

public interface IAuthRepository
{
    Task<bool> FindUserByEmail(string email);
    Task<UserDto> GetUserByEmail(string email);
}
