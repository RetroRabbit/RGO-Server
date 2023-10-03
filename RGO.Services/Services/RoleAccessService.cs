using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services;

public class RoleAccessService : IRoleAccessService
{
    private readonly IUnitOfWork _db;

    public RoleAccessService(IUnitOfWork db)
    {
        _db = db;
    }

    public Task<bool> CheckRoleAccess(string permission)
    {
        return _db.RoleAccess
            .Any(r => r.Permission == permission);
    }

    public async Task<RoleAccessDto> DeleteRoleAccess(string permission)
    {
        var roleAccess = await GetRoleAccess(permission);

        var deletedRoleAccess = await _db.RoleAccess.Delete(roleAccess.Id);

        return deletedRoleAccess;
    }

    public Task<List<RoleAccessDto>> GetAllRoleAccess()
    {
        return _db.RoleAccess.GetAll();
    }

    public async Task<RoleAccessDto> GetRoleAccess(string permission)
    {
        var roleAccess = await _db.RoleAccess
            .Get(r => r.Permission == permission)
            .Select(r => r.ToDto())
            .FirstOrDefaultAsync();

        if (roleAccess == null) throw new Exception($"RoleAccess not found({permission})");

        return roleAccess;
    }

    public async Task<RoleAccessDto> SaveRoleAccess(RoleAccessDto roleAccessDto)
    {
        var addedRoleAccess = await _db.RoleAccess.Add(new RoleAccess(roleAccessDto));

        return addedRoleAccess;
    }

    public async Task<RoleAccessDto> UpdateRoleAccess(RoleAccessDto roleAccessDto)
    {
        var updatedRoleAccess = await _db.RoleAccess.Update(new RoleAccess(roleAccessDto));

        return updatedRoleAccess;
    }
}
