using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class RoleAccessService : IRoleAccessService
{
    private readonly IUnitOfWork _db;
    private readonly IErrorLoggingService _errorLoggingService;

    public RoleAccessService(IUnitOfWork db, IErrorLoggingService errorLoggingService)
    {
        _db = db;
        _errorLoggingService = errorLoggingService;
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

        if (roleAccess == null)
        {
            var exception = new Exception($"RoleAccess not found({permission})");
            throw _errorLoggingService.LogException(exception);
        }

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