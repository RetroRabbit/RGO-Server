using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;

namespace HRIS.Services.Services;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _db;
    private readonly IAuthService _authService;

    public RoleService(IUnitOfWork db, IAuthService authService)
    {
        _db = db;
        _authService = authService;
    }

    public async Task<RoleDto> SaveRole(RoleDto roleDto)
    {
        var isRoleExist = await CheckRole(roleDto.Description!);
        if (isRoleExist)
        {
            var allRoles = await _authService.GetAllRolesAsync();
            var roleFound = allRoles.Any(role => role.Name == roleDto.Description);
            if (!roleFound) 
            {
                if (!string.IsNullOrEmpty(roleDto.Description)) {
                    await _authService.CreateRoleAsync(roleDto.Description, roleDto.Description);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(roleDto.AuthRoleId))
                {
                    var updatedRole = new RoleDto
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
        if (!string.IsNullOrEmpty(roleDto.Description))
        {
            await _authService.CreateRoleAsync(roleDto.Description, roleDto.Description);
        }
        return (await _db.Role.Add(new RR.UnitOfWork.Entities.HRIS.Role(roleDto))).ToDto();
    }

    public async Task<RoleDto> DeleteRole(int roleId)
    {
        var deletedRole = await _db.Role.Delete(roleId);

        return deletedRole.ToDto();
    }

    public async Task<List<RoleDto>> GetAll()
    {
        return (await _db.Role.GetAll()).Select(x => x.ToDto()).ToList();
    }

    public async Task<RoleDto> GetRole(string name)
    {
        var existingRole = await _db.Role
                                    .Get(role => role.Description == name)
                                    .Select(role => role.ToDto())
                                    .FirstOrDefaultAsync();

        if (existingRole == null)
            throw new CustomException($"Role not found({name})");

        return existingRole;
    }

    public async Task<RoleDto> UpdateRole(string name)
    {
        var existingRole = await GetRole(name);

        var updatedRole = await _db.Role
                                   .Update(new RR.UnitOfWork.Entities.HRIS.Role(existingRole));

        return updatedRole.ToDto();
    }

    public Task<bool> CheckRole(string name)
    {
        return _db.Role.Any(role => role.Description == name);
    }
}