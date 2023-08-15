using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RGO.Domain.Enums;
using RGO.Domain.Interfaces.Repository;
using RGO.Domain.Interfaces.Services;
using RGO.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RGO.Domain.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<bool> CheckUserExist(string email)
    {
        return await _userRepository.UserExists(email);
    }

    public async Task<string> GenerateToken(string email)
    {
        UserDto user = await _userRepository.GetUserByEmail(email);
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.ASCII.GetBytes(_configuration["Auth:Key"]);
        List<UserRole> roles = await GetUserRoles(email);
        string rolesString = string.Empty;

        try
        {
            rolesString = string.Join(",", roles.Select(role => role.ToString()));
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Claim[] claims = new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
            new Claim(ClaimTypes.Role, rolesString)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Auth:Expires"])),
            Issuer = _configuration["Auth:Issuer"],
            Audience = _configuration["Auth:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
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
