using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;

namespace RGO.Services.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IEmployeeService _employeeService;
    private readonly IRoleAccessLinkService _roleAccessLinkService;
    private readonly IUnitOfWork _db;

    public AuthService(
        IConfiguration configuration,
        IEmployeeService employeeService,
        IUnitOfWork db,
        IRoleAccessLinkService roleAccessLinkService)
    {
        _configuration = configuration;
        _employeeService = employeeService;
        _db = db;
        _roleAccessLinkService = roleAccessLinkService;
    }

    public async Task<bool> CheckUserExist(string email)
    {
        return await _employeeService.CheckUserExist(email);
    }

    public async Task<string> Login(string email)
    {
        var employee = await _employeeService.GetEmployee(email);

        return await GenerateToken(employee);
    }

    public async Task<string> RegisterEmployee(EmployeeDto employeeDto)
    {
        EmployeeDto newEmployee = await _employeeService.SaveEmployee(employeeDto);

        return await GenerateToken(newEmployee);
    }

    private async Task<string> GenerateToken(EmployeeDto employee)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Auth:Key"]!);
        var roles = await GetUserRoles(employee.Email);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
            new Claim(ClaimTypes.Email, employee.Email),
            new Claim(ClaimTypes.Name, employee.Name + " " + employee.Surname)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Key));

            foreach (var permission in role.Value)
            {
                claims.Add(new Claim("Permission", permission));
            }
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Auth:Expires"]!)),
            Issuer = _configuration["Auth:Issuer"],
            Audience = _configuration["Auth:Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<Dictionary<string, List<string>>> GetUserRoles(string email)
    {
        List<string> employeeRoles = await _db.EmployeeRole
            .Get(employeeRole => employeeRole.Employee.Email == email)
            .AsNoTracking()
            .Include(employeeRole => employeeRole.Role)
            .Include(employeeRole => employeeRole.Employee)
            .Include(employeeRole => employeeRole.Employee.EmployeeType)
            .Select(employeeRole => employeeRole.ToDto().Role.Description)
            .ToListAsync();

        if (employeeRoles.Count <= 0 && employeeRoles == null) throw new Exception("User not assigned role(s)");

        List<Dictionary<string, List<string>>> accessRoles = new();
        
        foreach (var role in employeeRoles)
        {
            var access = await _roleAccessLinkService.GetByRole(role);

            accessRoles.Add(access);
        }

        Dictionary<string, List<string>> mergedAccessRoles = accessRoles
            .SelectMany(dict => dict)
            .GroupBy(pair => pair.Key)
            .ToDictionary(
                group => group.Key,
                group => group.SelectMany(pair => pair.Value).ToList());

        return mergedAccessRoles;
    }
}
