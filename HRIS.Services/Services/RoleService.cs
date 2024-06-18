using Auth0.ManagementApi.Models;
using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _db;
    private readonly IErrorLoggingService _errorLoggingService;
    private readonly IAuthService _authService;

    public RoleService(IUnitOfWork db, IErrorLoggingService errorLoggingService, IAuthService authService)
    {
        _db = db;
        _errorLoggingService = errorLoggingService;
        _authService = authService;
    }

    public async Task<RoleDto> SaveRole(RoleDto roleDto)
    {
        var isRoleExist = await CheckRole(roleDto.Description!);
        if (isRoleExist)
        {
            var allRoles = await _authService.GetAllRolesAsync();
            bool roleFound = allRoles.Any(role => role.Name == roleDto.Description);
            if (!roleFound) 
            {
                if (!String.IsNullOrEmpty(roleDto.Description)) {
                    await _authService.CreateRoleAsync(roleDto.Description, roleDto.Description);
                }
            }
            else
            {
                if (String.IsNullOrEmpty(roleDto.AuthRoleId))
                {
                    RoleDto updatedRole = new RoleDto()
                    {
                        Id = roleDto.Id,
                        AuthRoleId = allRoles.First(role => role.Name == roleDto.Description).Id,
                        Description = roleDto.Description
                    };
                    await _db.Role.Update(new RR.UnitOfWork.Entities.HRIS.Role(updatedRole));
                }
            }
            return await GetRole(roleDto.Description!);
        }
        if (!String.IsNullOrEmpty(roleDto.Description))
        {
            await _authService.CreateRoleAsync(roleDto.Description, roleDto.Description);
        }
        return await _db.Role.Add(new RR.UnitOfWork.Entities.HRIS.Role(roleDto));
    }

    public async Task<RoleDto> DeleteRole(int roleId)
    {
        var deletedRole = await _db.Role.Delete(roleId);

        return deletedRole;
    }

    public async Task<List<RoleDto>> GetAll()
    {
        return await _db.Role.GetAll();
    }

    public async Task<RoleDto> GetRole(string name)
    {
        var existingRole = await _db.Role
                                    .Get(role => role.Description == name)
                                    .Select(role => role.ToDto())
                                    .FirstOrDefaultAsync();

        if (existingRole == null)
        {
            var exception = new Exception($"Role not found({name})");
            throw _errorLoggingService.LogException(exception);
        }

        return existingRole;
    }

    public async Task<RoleDto> UpdateRole(string name)
    {
        var existingRole = await GetRole(name);

        var updatedRole = await _db.Role
                                   .Update(new RR.UnitOfWork.Entities.HRIS.Role(existingRole));

        return updatedRole;
    }

    public Task<bool> CheckRole(string name)
    {
        return _db.Role.Any(role => role.Description == name);
    }
}