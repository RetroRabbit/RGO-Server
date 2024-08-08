using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class RoleAccessService : IRoleAccessService
{
    private readonly IUnitOfWork _db;
    private readonly AuthorizeIdentity _identity;

    public RoleAccessService(IUnitOfWork db, AuthorizeIdentity identity)
    {
        _db = db;
        _identity = identity;
    }

    public Task<bool> CheckRoleAccess(string permission)
    {
        return _db.RoleAccess
                  .Any(r => r.Permission == permission);
    }

    public async Task<RoleAccessDto> DeleteRoleAccess(string permission)
    {
        var roleAccessExist = await CheckRoleAccess(permission);
        if (!roleAccessExist)
        {
            throw new CustomException("Role Access Does Not Exist");
        }

        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        var roleAccess = await GetRoleAccess(permission);

        var deletedRoleAccess = await _db.RoleAccess.Delete(roleAccess.Id);

        return deletedRoleAccess.ToDto();
    }

    public async Task<List<RoleAccessDto>> GetAllRoleAccess()
    {
        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        return (await _db.RoleAccess.GetAll()).Select(x => x.ToDto()).ToList();
    }

    public async Task<RoleAccessDto> GetRoleAccess(string permission)
    {
        var roleAccessExist = await CheckRoleAccess(permission);
        if (!roleAccessExist)
        {
            throw new CustomException("Role Access Does Not Exist");
        }

        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        var roleAccess = await _db.RoleAccess
                                  .Get(r => r.Permission == permission)
                                  .Select(r => r.ToDto())
                                  .FirstOrDefaultAsync();

        return roleAccess;
    }

    public async Task<RoleAccessDto> CreateRoleAccess(RoleAccessDto roleAccessDto)
    {
        var roleAccessExist = await CheckRoleAccess(roleAccessDto.Permission);
        if (roleAccessExist)
        {
            throw new CustomException("Role Access Already Exists");
        }

        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        var addedRoleAccess = await _db.RoleAccess.Add(new RoleAccess(roleAccessDto));

        return addedRoleAccess.ToDto();
    }

    public async Task<RoleAccessDto> UpdateRoleAccess(RoleAccessDto roleAccessDto)
    {
        var roleAccessExist = await CheckRoleAccess(roleAccessDto.Permission);
        if (!roleAccessExist)
        {
            throw new CustomException("Role Access Does Not Exist");
        }

        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        var updatedRoleAccess = await _db.RoleAccess.Update(new RoleAccess(roleAccessDto));

        return updatedRoleAccess.ToDto();
    }
}