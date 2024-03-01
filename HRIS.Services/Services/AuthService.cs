using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HRIS.Services.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IEmployeeService _employeeService;
    private readonly IRoleAccessLinkService _roleAccessLinkService;

    public AuthService(
        IConfiguration configuration,
        IEmployeeService employeeService,
        IRoleAccessLinkService roleAccessLinkService)
    {
        _configuration = configuration;
        _employeeService = employeeService;
        _roleAccessLinkService = roleAccessLinkService;
    }

    public async Task<bool> CheckUserExist(string email)
    {
        return await _employeeService.CheckUserExist(email);
    }

    public async Task<string> Login(string email)
    {
        var employee = await _employeeService.GetEmployee(email);

        return await GenerateToken(employee!);
    }

    public async Task<string> RegisterEmployee(EmployeeDto employeeDto)
    {
        var newEmployee = await _employeeService.SaveEmployee(employeeDto);

        return await GenerateToken(newEmployee);
    }

    public async Task<string> GenerateToken(EmployeeDto employee)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("Auth__Key"));
        var roles = await GetUserRoles(employee.Email);
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, employee.Id.ToString()),
            new(ClaimTypes.Email, employee.Email!),
            new(ClaimTypes.Name, employee.Name + " " + employee.Surname)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Key));

            foreach (var permission in role.Value) claims.Add(new Claim("Permission", permission));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(int.Parse(Environment.GetEnvironmentVariable("Auth__Expires")!)),
            Issuer = Environment.GetEnvironmentVariable("Auth__Issuer"),
            Audience = Environment.GetEnvironmentVariable("Auth__Audience"),
            SigningCredentials = new SigningCredentials(
                                                        new SymmetricSecurityKey(key),
                                                        SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<Dictionary<string, List<string>>> GetUserRoles(string email)
    {
        var roles = await _roleAccessLinkService.GetRoleByEmployee(email);

        return roles;
    }
}