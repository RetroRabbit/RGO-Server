using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.Services.Services;

public class RoleAccessService : IRoleAccessService
{
    private readonly IRoleAccessRepository _roleAccessRepository;

    public RoleAccessService(IRoleAccessRepository roleAccessRepository)
    {
        _roleAccessRepository = roleAccessRepository;
    }

    public async Task<RoleAccessDto> AddRoleAccess(RoleAccessDto roleAccessDto)
    {
        RoleAccessDto newRoleAccess = await _roleAccessRepository.Add(new RoleAccess(roleAccessDto));

        return newRoleAccess;
    }

    public async Task<RoleAccessDto> DeleteRoleAccess(string action)
    {
        RoleAccessDto existingRoleAccess = await GetRoleAccess(action);

        RoleAccessDto deletedRoleAccess = await _roleAccessRepository
            .Delete(existingRoleAccess.Id);

        return deletedRoleAccess;
    }

    public async Task<List<RoleAccessDto>> GetAllRoleAccess()
    {
        return await _roleAccessRepository
            .GetAll();
    }

    public async Task<RoleAccessDto> GetRoleAccess(string action)
    {
        RoleAccessDto existingRoleAccess = await _roleAccessRepository
            .Get(roleAccess => roleAccess.Action == action)
            .Select(roleAccess => roleAccess.ToDto())
            .FirstAsync();

        return existingRoleAccess;
    }

    public async Task<RoleAccessDto> UpdateRoleAccess(RoleAccessDto roleAccessDto)
    {
        RoleAccessDto existingRoleAccess = await GetRoleAccess(roleAccessDto.Action);

        RoleAccessDto updatedRoleAccess = await _roleAccessRepository
            .Update(new RoleAccess(existingRoleAccess));

        return updatedRoleAccess;
    }
}
