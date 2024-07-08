using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

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
            throw new CustomException("Employee or Role not found");

        var isEmployeeRoleExist = await _db.EmployeeRole
                                           .Any(employeeRole =>
                                                    employeeRole.Employee!.Email == employeeRoleDto.Employee.Email);

        if (isEmployeeRoleExist)
            throw new CustomException("Employee Role already exist");

        var newEmployeeRole = await _db.EmployeeRole.Add(new EmployeeRole(employeeRoleDto));

        if (newEmployeeRole.Employee == null || newEmployeeRole.Role == null)
            newEmployeeRole = await _db.EmployeeRole.GetById(newEmployeeRole.Id);

        return newEmployeeRole!.ToDto();
    }

    public async Task<EmployeeRoleDto> DeleteEmployeeRole(string email, string role)
    {
        var isEmployeeRoleExist = await CheckEmployeeRole(email, role);

        if (!isEmployeeRoleExist)
            throw new CustomException("Employee Role not found");

        var employeeRoles = await GetEmployeeRoles(email);

        var toDelete = employeeRoles
                       .Where(employeeRole => employeeRole.Role!.Description == role)
                       .Select(employeeRole => employeeRole)
                       .FirstOrDefault();

        var deletedEmployeeRole = await _db.EmployeeRole
                                           .Delete(toDelete!.Id);

        return deletedEmployeeRole.ToDto();
    }

    public async Task<List<EmployeeRoleDto>> GetAllEmployeeRoles()
    {
        return (await _db.EmployeeRole.GetAll()).Select(x => x.ToDto()).ToList();
    }

    public async Task<List<EmployeeRoleDto>> GetEmployeeRoles(string email)
    {
        var existingRmployeeRole = await _db.EmployeeRole
                                            .Get(employeeRole => employeeRole.Employee!.Email == email)
                                            .AsNoTracking()
                                            .Include(employeeRole => employeeRole.Role)
                                            .Include(employeeRole => employeeRole.Employee)
                                            .Include(employeeRole => employeeRole.Employee!.EmployeeType)
                                            .Select(employeeRole => employeeRole.ToDto())
                                            .ToListAsync();

        return existingRmployeeRole;
    }

    public async Task<EmployeeRoleDto> UpdateEmployeeRole(EmployeeRoleDto employeeRoleDto)
    {
        var exists = await _db.EmployeeRole
                              .Any(employeeRole => employeeRole.Employee!.Email == employeeRoleDto.Employee!.Email);

        if (!exists)
            throw new CustomException("Employee Role not found");

        var updatedEmployeeRole = await _db.EmployeeRole
                                           .Update(new EmployeeRole(employeeRoleDto));

        return updatedEmployeeRole.ToDto();
    }

    public async Task<EmployeeRoleDto> GetEmployeeRole(string email)
    {
        var existingEmployeeRole = await _db.EmployeeRole
                                            .Get(employeeRole =>
                                                     employeeRole.Employee!.Email == email)
                                            .AsNoTracking()
                                            .Include(employeeRole => employeeRole.Employee)
                                            .Include(employeeRole => employeeRole.Employee!.EmployeeType)
                                            .Include(employeeRole => employeeRole.Role)
                                            .Select(employeeRole => employeeRole.ToDto())
                                            .FirstAsync();

        return existingEmployeeRole;
    }

    public async Task<bool> CheckEmployeeRole(string email, string role)
    {
        return await _db.EmployeeRole
                        .Any(employeeRole =>
                                 employeeRole.Employee!.Email == email && employeeRole.Role!.Description == role);
    }

    public async Task<List<EmployeeRoleDto>> GetAllEmployeeOnRoles(int roleId)
    {
        var existingRmployeeRole = await _db.EmployeeRole
                                            .Get(employeeRole => employeeRole.Role!.Id == roleId)
                                            .AsNoTracking()
                                            .Include(employeeRole => employeeRole.Role)
                                            .Include(employeeRole => employeeRole.Employee)
                                            .Include(employeeRole => employeeRole.Employee!.EmployeeType)
                                            .Select(employeeRole => employeeRole.ToDto())
                                            .ToListAsync();

        return existingRmployeeRole;
    }
}