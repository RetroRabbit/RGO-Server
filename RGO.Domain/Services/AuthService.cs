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
    private readonly IUnitOfWork _db;

    public AuthService(
        IConfiguration configuration,
        IEmployeeService employeeService,
        IUnitOfWork db)
    {
        _configuration = configuration;
        _employeeService = employeeService;
        _db = db;
    }

    public async Task<bool> CheckUserExist(string email)
    {
        return await _employeeService.CheckUserExist(email);
    }

    public async Task<string> Login(string email)
    {
        var employee = await _employeeService.GetEmployee(email);

        return employee == null ? throw new Exception("User not found") : await GenerateToken(employee);
    }

    public async Task<string> RegisterEmployee(EmployeeDto employeeDto)
    {
        EmployeeDto newEmployee = await _employeeService.AddEmployee(employeeDto);

        return await GenerateToken(newEmployee);
    }

    private async Task<string> GenerateToken(EmployeeDto employee)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Auth:Key"]!);
        List<AuthRoleResult> roles = await GetUserRoles(employee.PersonalEmail);
        var collectionOfRoleClaims = roles
            .Select(role =>
            {
                string upperRole = role.Role.ToUpper();

                List<Claim> roleClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Role, upperRole),
                    new Claim($"{upperRole}-Action", role.Action)
                };

                if (role.View) roleClaims.Add(new Claim($"{upperRole}-View", "true"));
                if (role.Edit) roleClaims.Add(new Claim($"{upperRole}-Edit", "true"));
                if (role.Delete) roleClaims.Add(new Claim($"{upperRole}-Delete", "true"));

                return roleClaims;
            })
            .ToList();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
            new Claim(ClaimTypes.Email, employee.PersonalEmail),
            new Claim(ClaimTypes.Name, employee.Name + " " + employee.Surname)
        };

        foreach (var role in collectionOfRoleClaims) claims.AddRange(role);

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

    public async Task<List<AuthRoleResult>> GetUserRoles(string email)
    {
        var employeeRoles = await _db.EmployeeRole
            .Get(employeeRole => employeeRole.Employee.PersonalEmail == email)
            .AsNoTracking()
            .Include(employeeRole => employeeRole.Role)
            .Include(employeeRole => employeeRole.Employee)
            .Include(employeeRole => employeeRole.Employee.EmployeeType)
            .Select(employeeRole => employeeRole.ToDto().Role.Description)
            .ToListAsync();

        if (employeeRoles.Count <= 0 && employeeRoles == null) throw new Exception("User not assigned role(s)");

        var role = await _db.RoleAccess
            .Get(roleAccess => employeeRoles.Contains(roleAccess.Role.Description))
            .AsNoTracking()
            .Include(roleAccess => roleAccess.Role)
            .Select(roleAccess => roleAccess.ToDto())
            .ToListAsync();

        return role
            .Select(r => new AuthRoleResult(
                r.Role.Description,
                r.Action,
                r.View,
                r.Edit,
                r.Delete))
            .ToList();
    }
}
