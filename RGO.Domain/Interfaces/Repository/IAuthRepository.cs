using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Repository;

public interface IAuthRepository
{
    Task<bool> FindUserByEmail(string email);
    Task<UserDto> GetUserByEmail(string email);
}
