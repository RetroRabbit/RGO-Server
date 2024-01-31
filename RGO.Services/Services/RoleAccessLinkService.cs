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
            .Include(r => r.Role)
            .Include(r => r.RoleAccess)
            .Select(r => r.ToDto())
            .FirstOrDefaultAsync();

        if (roleAccessLink == null) throw new Exception($"Role Access Link not found(Role:{role},Permission:{permission})");

        RoleAccessLinkDto deleted = await _db.RoleAccessLink.Delete(roleAccessLink.Id);

        return deleted.Role != null ? deleted : roleAccessLink;
    }

    public async Task<Dictionary<string, List<string>>> GetAll()
    {
        var roleAccessLinks = await _db.RoleAccessLink
            .Get()
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

    public Task<Dictionary<string, List<string>>> GetByPermission(string permission)
    {
        var roleAccessLinks = _db.RoleAccessLink
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

    public async Task<Dictionary<string, List<string>>> GetRoleByEmployee(string email)
    {
         var employeeRoles = (await _employeeRoleService.GetEmployeeRoles(email))
            .Select(r => r.Role!.Description)
            .ToList();

        List<Dictionary<string, List<string>>> accessRoles = new();

        foreach (var role in employeeRoles)
        {
            var roleAccessLinks = await GetByRole(role);

            accessRoles.Add(roleAccessLinks);
        }

        Dictionary<string, List<string>> mergedRoleAccessLinks = accessRoles
            .SelectMany(dict => dict)
            .ToLookup(pair => pair.Key, pair => pair.Value)
            .ToDictionary(group => group.Key, group => group.SelectMany(list => list).Distinct().ToList());

       return mergedRoleAccessLinks;
    }

    public Task<RoleAccessLinkDto> Update(RoleAccessLinkDto roleAccessLinkDto)
    {
        var roleAccessLink = _db.RoleAccessLink.Update(new RoleAccessLink(roleAccessLinkDto));
        return roleAccessLink;
    }

    public async Task<List<RoleAccessLinkDto>> GetAllRoleAccessLink()
    {
        List<RoleAccessLinkDto> roleAccessLinks = await _db.RoleAccessLink
            .Get()
            .AsNoTracking()
            .Include(r => r.Role)
            .Include(r => r.RoleAccess)
            .Select(x => x.ToDto())
            .ToListAsync();

        return roleAccessLinks;
    }

}
