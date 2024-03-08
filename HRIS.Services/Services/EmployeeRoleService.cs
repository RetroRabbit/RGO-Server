using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeRoleService : IEmployeeRoleService
{
    private readonly IUnitOfWork _db;
    private readonly IErrorLoggingService _errorLoggingService;

    public EmployeeRoleService(IUnitOfWork db, IErrorLoggingService errorLoggingService)
    {
        _db = db;
        _errorLoggingService = errorLoggingService;
    }

    public async Task<EmployeeRoleDto> SaveEmployeeRole(EmployeeRoleDto employeeRoleDto)
    {
        if (employeeRoleDto.Employee is null || employeeRoleDto.Role is null)
        {
            var exception = new Exception("Employee or Role not found");
            throw _errorLoggingService.LogException(exception);
        }

        var isEmployeeRoleExist = await _db.EmployeeRole
                                           .Any(employeeRole =>
                                                    employeeRole.Employee!.Email == employeeRoleDto.Employee.Email);

        if (isEmployeeRoleExist)
        {
            var exception = new Exception("Employee Role already exist");
            throw _errorLoggingService.LogException(exception);
        }

        var newEmployeeRole = await _db.EmployeeRole.Add(new EmployeeRole(employeeRoleDto));

        if (newEmployeeRole.Employee == null || newEmployeeRole.Role == null)
            newEmployeeRole = await _db.EmployeeRole.GetById(newEmployeeRole.Id);

        return newEmployeeRole!;
    }

    public async Task<EmployeeRoleDto> DeleteEmployeeRole(string email, string role)
    {
        var isEmployeeRoleExist = await CheckEmployeeRole(email, role);

        if (!isEmployeeRoleExist)
        {
            var exception = new Exception("Employee Role not found");
            throw _errorLoggingService.LogException(exception);
        }

        var employeeRoles = await GetEmployeeRoles(email);

        var toDelete = employeeRoles
                       .Where(employeeRole => employeeRole.Role!.Description == role)
                       .Select(employeeRole => employeeRole)
                       .FirstOrDefault();

        var deletedEmployeeRole = await _db.EmployeeRole
                                           .Delete(toDelete!.Id);

        return deletedEmployeeRole;
    }

    public Task<List<EmployeeRoleDto>> GetAllEmployeeRoles()
    {
        return _db.EmployeeRole
                  .GetAll();
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
        {
            var exception = new Exception("Employee Role not found");
            throw _errorLoggingService.LogException(exception);
        }

        var updatedEmployeeRole = await _db.EmployeeRole
                                           .Update(new EmployeeRole(employeeRoleDto));

        return updatedEmployeeRole;
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