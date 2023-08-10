using RGO.Domain.Enums;

namespace RGO.Domain.Interfaces.Services;

public interface IAuthService
{
    Task<bool> CheckUserExist(string email);

    Task<List<UserRole>> GetUserRoles(string email);
    Task<string> GenerateToken(string email);
}
