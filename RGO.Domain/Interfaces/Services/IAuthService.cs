using RGO.Domain.Models;

namespace RGO.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<bool> CheckUserExist(string email);
    Task<UserDto> GetUserByEmail(string email);
    string GenerateToken();
}
