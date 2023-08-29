using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services;

public class EmployeeRoleService : IEmployeeRoleService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeService _employeeService;

    public EmployeeRoleService(IUnitOfWork db, IEmployeeService employeeService)
    {
        _db = db;
        _employeeService = employeeService;
    }

    public async Task<EmployeeRoleDto> SaveEmployeeRole(EmployeeRoleDto employeeRoleDto)
    {
        EmployeeRoleDto newEmployeeRole = await _db.EmployeeRole.Add(new EmployeeRole(employeeRoleDto));

        return newEmployeeRole;
    }

    public async Task<EmployeeRoleDto> DeleteEmployeeRole(string email)
    {
        EmployeeRoleDto existingEmployeeRole = await GetEmployeeRole(email);

        EmployeeRoleDto employeeRoleDto = await _db.EmployeeRole
            .Delete(existingEmployeeRole.Id);

        return employeeRoleDto;

    }

    public Task<List<EmployeeRoleDto>> GetAllEmployeeRoles()
    {
        return _db.EmployeeRole.GetAll();
    }

    public async Task<EmployeeRoleDto> GetEmployeeRole(string email)
    {
        EmployeeRoleDto existingRmployeeRole = await _db.EmployeeRole
            .Get(employeeRole => employeeRole.Employee.Email == email)
            .AsNoTracking()
            .Include(employeeRole => employeeRole.Employee)
            .Include(employeeRole => employeeRole.Role)
            .Select(employeeRole => employeeRole.ToDto())
            .FirstAsync();

        return existingRmployeeRole;
    }

    public async Task<EmployeeRoleDto> UpdateEmployeeRole(EmployeeRoleDto employeeRoleDto)
    {
        EmployeeRoleDto existingEmployeeRole = await GetEmployeeRole(employeeRoleDto.Employee.Email);

        EmployeeRoleDto updatedEmployeeRole = await _db.EmployeeRole
            .Update(new EmployeeRole(existingEmployeeRole));

        return updatedEmployeeRole;
    }

    public async Task<EmployeeRoleDto> GetEmployeeRoleByEmail(string email)
    {
        EmployeeRoleDto existingEmployeeRole = await _db.EmployeeRole
            .Get(employeeRole => employeeRole.Employee.Email == email)
            .AsNoTracking()
            .Include(employeeRole => employeeRole.Employee)
            .Include(employeeRole => employeeRole.Employee.EmployeeType)
            .Include(employeeRole => employeeRole.Role)
            .Select(employeeRole => employeeRole.ToDto())
            .FirstAsync();

        return existingEmployeeRole;
    }

    public async Task<bool> CheckEmployeeRole(string email, string role)
    {
        return await _db.EmployeeRole
            .Any(employeeRole => employeeRole.Employee.Email == email && employeeRole.Role.Description == role);
    }
}
