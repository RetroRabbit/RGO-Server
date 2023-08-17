using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RGO.Models;
using RGO.Models.Enums;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;

namespace RGO.Services.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _db;

    public AuthService(IConfiguration configuration, IUnitOfWork db)
    {
        _configuration = configuration;
        _db = db;
    }

    public async Task<bool> CheckUserExist(string email)
    {
        return await _db.User.Any(x => x.Email == email);
    }

    public async Task<string> GenerateToken(string email)
    {

        var user = await _db.User.GetByEmail(email);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Auth:Key"]);
        var roles = await GetUserRoles(email);
        var rolesString = string.Empty;

        try
        {
            rolesString = string.Join(",", roles.Select(role => role.ToString()));
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        var claims = new Claim[]
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

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<List<UserRole>> GetUserRoles(string email)
    {
        var roles = await _db.User.GetUserRoles(email);
        return roles
            .Select(role => (UserRole)role)
            .ToList();
    }
}
