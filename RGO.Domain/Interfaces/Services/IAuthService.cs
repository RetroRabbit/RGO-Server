using RGO.Domain.Enums;

namespace RGO.Domain.Interfaces.Services;

public interface IAuthService
{
    /// <summary>
    /// Check if user exist
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<bool> CheckUserExist(string email);

    /// <summary>
    /// Get user roles by email
    /// </summary>
    /// <param name="email"></param>
    /// <returns>List of role, will reflect as list of int</returns>
    Task<List<UserRole>> GetUserRoles(string email);

    /// <summary>
    /// Generate JWT token
    /// </summary>
    /// <param name="email"></param>
    /// <returns>JWT token as string</returns>
    Task<string> GenerateToken(string email);
}
