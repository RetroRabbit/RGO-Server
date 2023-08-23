using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.Services.Services;

public class RoleAccessService : IRoleAccessService
{
    private readonly IUnitOfWork _db;

    public RoleAccessService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<RoleAccessDto> SaveRoleAccess(RoleAccessDto roleAccessDto)
    {
        RoleAccessDto newRoleAccess = await _db.RoleAccess.Add(new RoleAccess(roleAccessDto));

        return newRoleAccess;
    }

    public async Task<RoleAccessDto> DeleteRoleAccess(string action)
    {
        RoleAccessDto existingRoleAccess = await GetRoleAccess(action);

        RoleAccessDto deletedRoleAccess = await _db.RoleAccess
            .Delete(existingRoleAccess.Id);

        return deletedRoleAccess;
    }

    public async Task<List<RoleAccessDto>> GetAllRoleAccess()
    {
        return await _db.RoleAccess
            .GetAll();
    }

    public async Task<RoleAccessDto> GetRoleAccess(string action)
    {
        RoleAccessDto existingRoleAccess = await _db.RoleAccess
            .Get(roleAccess => roleAccess.Action == action)
            .Select(roleAccess => roleAccess.ToDto())
            .FirstAsync();

        return existingRoleAccess;
    }

    public async Task<List<RoleAccessDto>> GetRoleAccessByRole(string description)
    {
        return await _db.RoleAccess
            .Get(roleAccess => roleAccess.Role.Description == description)
            .AsNoTracking()
            .Include(roleAccess => roleAccess.Role)
            .Select(roleAccess => roleAccess.ToDto())
            .ToListAsync();
    }

    public async Task<RoleAccessDto> UpdateRoleAccess(RoleAccessDto roleAccessDto)
    {
        RoleAccessDto existingRoleAccess = await GetRoleAccess(roleAccessDto.Action);

        RoleAccessDto updatedRoleAccess = await _db.RoleAccess
            .Update(new RoleAccess(existingRoleAccess));

        return updatedRoleAccess;
    }
}
