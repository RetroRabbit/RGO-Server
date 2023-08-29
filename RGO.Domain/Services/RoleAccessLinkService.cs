using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services;

public class RoleAccessLinkService : IRoleAccessLinkService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeRoleService _employeeRoleService;

    public RoleAccessLinkService(IUnitOfWork db, IEmployeeRoleService employeeRoleService)
    {
        _db = db;
        _employeeRoleService = employeeRoleService;
    }

    public async Task<RoleAccessLinkDto> Save(RoleAccessLinkDto roleAccessLinkDto)
    {
        var newRoleAccessLink = new RoleAccessLink(roleAccessLinkDto);

        var addedRoleAccessLink = await _db.RoleAccessLink.Add(newRoleAccessLink);

        return addedRoleAccessLink;
    }

    public async Task<RoleAccessLinkDto> Delete(string role, string permission)
    {
        var roleAccessLink = await _db.RoleAccessLink
            .Get(r => r.Role.Description == role && r.RoleAccess.Permission == permission)
            .AsNoTracking()
            .Select(r => r.ToDto())
            .FirstOrDefaultAsync();

        if (roleAccessLink == null) throw new Exception($"Role Access Link not found(Role:{role},Permission:{permission})");

        var deletedRoleAccessLink = await _db.RoleAccessLink.Delete(roleAccessLink.Id);

        return deletedRoleAccessLink;
    }

    public async Task<Dictionary<string, List<string>>> GetAll()
    {
        var roleAccessLinks = await _db.RoleAccessLink
            .Get(r => true)
            .AsNoTracking()
            .Include(r => r.Role)
            .Include(r => r.RoleAccess)
            .GroupBy(r => r.Role.Description)
            .ToDictionaryAsync(
                group => group.Key,
                group => group.Select(link => link.RoleAccess.Permission).ToList());

        return roleAccessLinks;
    }

    public async Task<Dictionary<string, List<string>>> GetByPermission(string permission)
    {
        var roleAccessLinks = await _db.RoleAccessLink
            .Get(r => r.RoleAccess.Permission == permission)
            .AsNoTracking()
            .Include(r => r.Role)
            .Include(r => r.RoleAccess)
            .GroupBy(r => r.Role.Description)
            .ToDictionaryAsync(
                group => group.Key,
                group => group.Select(link => link.RoleAccess.Permission).ToList());

        return roleAccessLinks;
    }

    public async Task<Dictionary<string, List<string>>> GetByRole(string role)
    {
        var roleAccessLinks = await _db.RoleAccessLink
            .Get(r => r.Role.Description == role)
            .AsNoTracking()
            .Include(r => r.Role)
            .Include(r => r.RoleAccess)
            .GroupBy(r => r.Role.Description)
            .ToDictionaryAsync(
                group => group.Key,
                group => group.Select(link => link.RoleAccess.Permission).ToList());

        return roleAccessLinks;
    }

    public async Task<Dictionary<string, List<string>>> GetRoleByEmployee(string email)
    {
         var employeeRole = await _employeeRoleService.GetEmployeeRole(email);

        var roleAccessLinks = await _db.RoleAccessLink
            .Get(r => r.Role.Id == employeeRole.Role.Id)
            .AsNoTracking()
            .Include(r => r.Role)
            .Include(r => r.RoleAccess)
            .GroupBy(r => r.Role.Description)
            .ToDictionaryAsync(
                group => group.Key,
                group => group.Select(link => link.RoleAccess.Permission).ToList());

        return roleAccessLinks;
    }

    public Task<RoleAccessLinkDto> Update(RoleAccessLinkDto roleAccessLinkDto)
    {
        var roleAccessLink = _db.RoleAccessLink.Update(new RoleAccessLink(roleAccessLinkDto));
        return roleAccessLink;
    }
}
