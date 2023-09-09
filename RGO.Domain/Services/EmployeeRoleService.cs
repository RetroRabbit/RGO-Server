using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System.Xml.Linq;

namespace RGO.Services.Services;

public class EmployeeRoleService : IEmployeeRoleService
{
    private readonly IUnitOfWork _db;

    public EmployeeRoleService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<EmployeeRoleDto> SaveEmployeeRole(EmployeeRoleDto employeeRoleDto)
    {
        bool isEmployeeRoleExist = await CheckEmployeeRole(employeeRoleDto.Employee!.Email, employeeRoleDto.Role!.Description);

        if (isEmployeeRoleExist)
            throw new Exception("Employee Role already exist");
        EmployeeRoleDto newEmployeeRole = await _db.EmployeeRole.Add(new EmployeeRole(employeeRoleDto));

        if (newEmployeeRole.Employee == null || newEmployeeRole.Role == null)
            newEmployeeRole = await _db.EmployeeRole.GetById(newEmployeeRole.Id);

        return newEmployeeRole;
    }

    public async Task<EmployeeRoleDto> DeleteEmployeeRole(string email, string role)
    {
        bool isEmployeeRoleExist = await CheckEmployeeRole(email, role);

        if (!isEmployeeRoleExist) throw new Exception("Employee Role not found");

        var employeeRoles = await GetEmployeeRoles(email);

        EmployeeRoleDto? toDelete = employeeRoles
            .Where(employeeRole => employeeRole.Role.Description == role)
            .Select(employeeRole => employeeRole)
            .FirstOrDefault();

        if (toDelete is null)
            throw new Exception("Employee Role not found");

        EmployeeRoleDto deletedEmployeeRole = await _db.EmployeeRole
                .Delete(toDelete.Id);

        return deletedEmployeeRole;
    }

    public Task<List<EmployeeRoleDto>> GetAllEmployeeRoles()
    {
        return _db.EmployeeRole
            .GetAll();
    }

    public async Task<List<EmployeeRoleDto>> GetEmployeeRoles(string email)
    {
        List<EmployeeRoleDto> existingRmployeeRole = await _db.EmployeeRole
            .Get(employeeRole => employeeRole.Employee.Email == email)
            .AsNoTracking()
            .Include(employeeRole => employeeRole.Role)
            .Include(employeeRole => employeeRole.Employee)
            .Include(employeeRole => employeeRole.Employee.EmployeeType)
            .Select(employeeRole => employeeRole.ToDto())
            .ToListAsync();

        return existingRmployeeRole;
    }

    public async Task<EmployeeRoleDto> UpdateEmployeeRole(EmployeeRoleDto employeeRoleDto)
    {
        bool exists = await CheckEmployeeRole(employeeRoleDto.Employee.Email, employeeRoleDto.Role.Description);

        if (!exists)
        {
            throw new Exception("Employee Role not found");
        }

        EmployeeRoleDto updatedEmployeeRole = await _db.EmployeeRole
            .Update(new EmployeeRole(employeeRoleDto));

        return updatedEmployeeRole;
    }

    public async Task<EmployeeRoleDto> GetEmployeeRole(string email, string role)
    {
        EmployeeRoleDto existingEmployeeRole = await _db.EmployeeRole
            .Get(employeeRole =>
                employeeRole.Role.Description == role &&
                employeeRole.Employee.Email == email)
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
