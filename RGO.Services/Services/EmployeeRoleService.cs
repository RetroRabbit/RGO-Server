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
        if (employeeRoleDto.Employee is null || employeeRoleDto.Role is null)
            throw new Exception("Employee or Role not found");

        bool isEmployeeRoleExist = await _db.EmployeeRole
            .Any(employeeRole => employeeRole.Employee.Email == employeeRoleDto.Employee.Email);

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
        bool exists = await _db.EmployeeRole
            .Any(employeeRole => employeeRole.Employee.Email == employeeRoleDto.Employee.Email);

        if (!exists)
        {
            throw new Exception("Employee Role not found");
        }

        EmployeeRoleDto updatedEmployeeRole = await _db.EmployeeRole
            .Update(new EmployeeRole(employeeRoleDto));

        return updatedEmployeeRole;
    }

    public async Task<EmployeeRoleDto> GetEmployeeRole(string email)
    {
        EmployeeRoleDto existingEmployeeRole = await _db.EmployeeRole
            .Get(employeeRole =>
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

    public async Task<List<EmployeeRoleDto>> GetAllEmployeeOnRoles(int roleId)
    {
        List<EmployeeRoleDto> existingRmployeeRole = await _db.EmployeeRole
            .Get(employeeRole => employeeRole.Role.Id == roleId)
            .AsNoTracking()
            .Include(employeeRole => employeeRole.Role)
            .Include(employeeRole => employeeRole.Employee)
            .Include(employeeRole => employeeRole.Employee.EmployeeType)
            .Select(employeeRole => employeeRole.ToDto())
            .ToListAsync();

        return existingRmployeeRole;
    }

}
