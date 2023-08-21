using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RGO.Models;
using RGO.Models.Enums;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.Services.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private readonly IEmployeeService _employeeService;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IEmployeeRoleRepository _employeeRoleRepository;
    private readonly IRoleAccessRepository _roleAccessRepository;

    public AuthService(
        IConfiguration configuration,
        IEmployeeRepository employeeRepository,
        IEmployeeRoleRepository employeeRoleRepository,
        IRoleAccessRepository roleAccessRepository,
        IEmployeeService employeeService)
    {
        _employeeRepository = employeeRepository;
        _configuration = configuration;
        _employeeRoleRepository = employeeRoleRepository;
        _roleAccessRepository = roleAccessRepository;
        _employeeService = employeeService;
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
        var roles = await GetUserRoles(employee.PersonalEmail);
        var rolesString = roles
            .Select(role => $"{role.Action}#{role.View}|{role.Edit}|{role.Delete}")
            .Select(role => role.Replace("True", "1").Replace("False", "0"))
            .Select(role => new Claim(ClaimTypes.Role, role))
            .ToList();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
            new Claim(ClaimTypes.Email, employee.PersonalEmail),
            new Claim(ClaimTypes.Name, employee.Name + " " + employee.Surname)
        };

        foreach (var role in rolesString) claims.Add(role);

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
        var employeeRoles = await _employeeRoleRepository
            .Get(employeeRole => employeeRole.Employee.PersonalEmail == email)
            .Include(employeeRole => employeeRole.Role)
            .Include(employeeRole => employeeRole.Employee)
            .Include(employeeRole => employeeRole.Employee.EmployeeType)
            .Select(employeeRole => employeeRole.ToDto().Role.Description)
            .ToListAsync();

        if (employeeRoles.Count <= 0 && employeeRoles == null) throw new Exception("User not assigned role(s)");

        var role = await _roleAccessRepository
            .Get(roleAccess => employeeRoles.Contains(roleAccess.Role.Description))
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
