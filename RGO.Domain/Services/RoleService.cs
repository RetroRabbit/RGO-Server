using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.Services.Services;

public class RoleService : IRoleService
{
    private readonly IRoleRepository _roleRepository;

    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<RoleDto> AddRole(RoleDto roleDto)
    {
        RoleDto newRole = await _roleRepository.Add(new Role(roleDto));

        return newRole;
    }

    public async Task<RoleDto> DeleteRole(string name)
    {
        RoleDto existingRole = await GetRole(name);

        RoleDto deletedRole = await _roleRepository
            .Delete(existingRole.Id);

        return deletedRole;
    }

    public async Task<List<RoleDto>> GetAll()
    {
        return await _roleRepository
            .GetAll();
    }

    public async Task<RoleDto> GetRole(string name)
    {
        RoleDto existingRole = await _roleRepository
            .Get(role => role.Description == name)
            .Select(role => role.ToDto())
            .FirstAsync();

        return existingRole;
    }

    public async Task<RoleDto> UpdateRole(string name)
    {
        RoleDto existingRole = await GetRole(name);

        RoleDto updatedRole = await _roleRepository
            .Update(new Role(existingRole));

        return updatedRole;
    }
}
