using Microsoft.IdentityModel.Tokens;
using RGO.Domain.Enums;
using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Interfaces.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

    public async Task<string> GenerateToken(string email)
    {
        var user = await _userRepository.GetUserByEmail(email);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("super secret key for the rabbit ");
        var roles = await GetUserRoles(email);
        string rolesString = string.Empty;
        try
        {
            rolesString = string.Join(",", roles.Select(role => role.ToString()));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.Role, rolesString)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = "API",
            Audience = "Client",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<List<UserRole>> GetUserRoles(string email)
    {
        List<int> roles = await _userRepository.GetUserRoles(email);
        return roles
            .Select(role => (UserRole)role)
            .ToList();
    }
}
