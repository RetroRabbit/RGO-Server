using RGO.Domain.Enums;
using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Interfaces.Services;

namespace RGO.Domain.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public AuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> CheckUserExist(string email)
    {
        return await _userRepository.UserExists(email);
    }

    public async Task<List<UserRole>> GetUserRoles(string email)
    {
        List<int> roles = await _userRepository.GetUserRoles(email);
        return roles
            .Select(role => (UserRole)role)
            .ToList();
    }
}
