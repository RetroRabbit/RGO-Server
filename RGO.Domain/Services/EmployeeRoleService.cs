using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

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
}
