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
        bool isRoleExist = await CheckRole(roleDto.Description);
        if (isRoleExist) return await GetRole(roleDto.Description);

        return await _db.Role.Add(new Role(roleDto));
    }

    public async Task<RoleDto> DeleteRole(int roleId)
    {
        RoleDto deletedRole = await _db.Role.Delete(roleId);

        return deletedRole;
    }

    public async Task<List<RoleDto>> GetAll()
    {
        return await _db.Role.GetAll();
    }

    public async Task<RoleDto> GetRole(string name)
    {
        RoleDto? existingRole = await _db.Role
            .Get(role => role.Description == name)
            .Select(role => role.ToDto())
            .FirstOrDefaultAsync();

        if (existingRole == null) throw new Exception($"Role not found({name})");

        return existingRole;
    }

    public async Task<RoleDto> UpdateRole(string name)
    {
        RoleDto existingRole = await GetRole(name);

        RoleDto updatedRole = await _db.Role
            .Update(new Role(existingRole));

        return updatedRole;
    }

    public Task<bool> CheckRole(string name)
    {
        return _db.Role.Any(role => role.Description == name);
    }
}
