using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _db;

    public RoleService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<RoleDto> SaveRole(RoleDto roleDto)
    {
        RoleDto newRole = await _db.Role.Add(new Role(roleDto));

        return newRole;
    }

    public async Task<RoleDto> DeleteRole(string name)
    {
        RoleDto existingRole = await GetRole(name);

        RoleDto deletedRole = await _db.Role
            .Delete(existingRole.Id);

        return deletedRole;
    }

    public async Task<List<RoleDto>> GetAll()
    {
        return await _db.Role.GetAll();
    }

    public async Task<RoleDto> GetRole(string name)
    {
        RoleDto existingRole = await _db.Role
            .Get(role => role.Description == name)
            .Select(role => role.ToDto())
            .FirstAsync();

        return existingRole;
    }

    public async Task<RoleDto> UpdateRole(string name)
    {
        RoleDto existingRole = await GetRole(name);

        RoleDto updatedRole = await _db.Role
            .Update(new Role(existingRole));

        return updatedRole;
    }
}
