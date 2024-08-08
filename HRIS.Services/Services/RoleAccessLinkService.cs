using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using System.Data;

namespace HRIS.Services.Services;

public class RoleAccessLinkService : IRoleAccessLinkService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeRoleService _employeeRoleService;
    private readonly AuthorizeIdentity _identity;

    public RoleAccessLinkService(IUnitOfWork db, IEmployeeRoleService employeeRoleService, AuthorizeIdentity identity)
    {
        _db = db;
        _employeeRoleService = employeeRoleService;
        _identity = identity;
    }

    public Task<bool> CheckRoleAccessLink(string role, string permission)
    {
        return _db.RoleAccessLink
                  .Any(r => r.Role!.Description == role && r.RoleAccess!.Permission == permission);
    }

    public Task<bool> CheckRole(string name)
    {
        return _db.Role.Any(role => role.Description == name);
    }

    public Task<bool> CheckRoleAccess(string permission)
    {
        return _db.RoleAccess
                  .Any(r => r.Permission == permission);
    }

    public Task<bool> CheckEmployee(string email)
    {
        return _db.Employee
                  .Any(r => r.Email == email);
    }

    public async Task<RoleAccessLinkDto> Create(RoleAccessLinkDto roleAccessLinkDto)
    {
        var exists = await CheckRoleAccessLink(roleAccessLinkDto.Role!.Description!, roleAccessLinkDto.RoleAccess!.Permission);
        if (exists)
        {
            throw new CustomException("Role Access Link Already Exists");
        }

        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        var newRoleAccessLink = new RoleAccessLink(roleAccessLinkDto);

        var addedRoleAccessLink = await _db.RoleAccessLink.Add(newRoleAccessLink);

        return addedRoleAccessLink.ToDto();
    }

    public async Task<RoleAccessLinkDto> Delete(string role, string permission)
    {
        var exists = await CheckRoleAccessLink(role, permission);
        if (!exists)
        {
            throw new CustomException("Role Access Link Does Not Exist");
        }

        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        var roleAccessLink = await _db.RoleAccessLink
                                      .Get(r => r.Role!.Description == role && r.RoleAccess!.Permission == permission)
                                      .AsNoTracking()
                                      .Include(r => r.Role)
                                      .Include(r => r.RoleAccess)
                                      .Select(r => r.ToDto())
                                      .FirstOrDefaultAsync();

        var deleted = await _db.RoleAccessLink.Delete(roleAccessLink!.Id);

        return deleted.Role != null ? deleted.ToDto() : roleAccessLink;
    }

    public async Task<Dictionary<string, List<string>>> GetAll()
    {
        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        var roleAccessLinks = await _db.RoleAccessLink
                                       .Get()
                                       .AsNoTracking()
                                       .Include(r => r.Role)
                                       .Include(r => r.RoleAccess)
                                       .GroupBy(r => r.Role!.Description)
                                       .ToDictionaryAsync(
                                                          group => group.Key!,
                                                          group => group.Select(link => link.RoleAccess!.Permission)
                                                                        .ToList());

        return roleAccessLinks!;
    }

    public async Task<Dictionary<string, List<string>>> GetByRole(string role)
    {
        var exists = await CheckRole(role);
        if (!exists)
        {
            throw new CustomException("Role Does Not Exist");
        }

        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        var roleAccessLinks = await _db.RoleAccessLink
                                       .Get(r => r.Role!.Description == role)
                                       .AsNoTracking()
                                       .Include(r => r.Role)
                                       .Include(r => r.RoleAccess)
                                       .GroupBy(r => r.Role!.Description)
                                       .ToDictionaryAsync(
                                                          group => group.Key!,
                                                          group => group.Select(link => link.RoleAccess!.Permission)
                                                                        .ToList());

        return roleAccessLinks!;
    }

    public async Task<Dictionary<string, List<string>>> GetByPermission(string permission)
    {
        var exists = await CheckRoleAccess(permission);
        if (!exists)
        {
            throw new CustomException("Role Access Does Not Exist");
        }

        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        var roleAccessLinks = await _db.RoleAccessLink
                                 .Get(r => r.RoleAccess!.Permission == permission)
                                 .AsNoTracking()
                                 .Include(r => r.Role)
                                 .Include(r => r.RoleAccess)
                                 .GroupBy(r => r.Role!.Description)
                                 .ToDictionaryAsync(
                                                    group => group.Key!,
                                                    group => group.Select(link => link.RoleAccess!.Permission).ToList());

        return roleAccessLinks!;
    }

    public async Task<Dictionary<string, List<string>>> GetRoleByEmployee(string email)
    {
        var exists = await CheckEmployee(email);
        if (!exists)
        {
            throw new CustomException("Employee Does Not Exist");
        }

        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        var employeeRoles = (await _employeeRoleService.GetEmployeeRoles(email))
                            .Select(r => r.Role!.Description)
                            .ToList();

        List<Dictionary<string, List<string>>> accessRoles = new();

        foreach (var role in employeeRoles)
        {
            var roleAccessLinks = await GetByRole(role!);

            accessRoles.Add(roleAccessLinks);
        }

        var mergedRoleAccessLinks = accessRoles
                                    .SelectMany(dict => dict)
                                    .ToLookup(pair => pair.Key, pair => pair.Value)
                                    .ToDictionary(group => group.Key,
                                                  group => group.SelectMany(list => list).Distinct().ToList());

        return mergedRoleAccessLinks;
    }

    public async Task<RoleAccessLinkDto> Update(RoleAccessLinkDto roleAccessLinkDto)
    {
        var exists = await CheckRoleAccessLink(roleAccessLinkDto.Role!.Description!, roleAccessLinkDto.RoleAccess!.Permission);
        if (!exists)
        {
            throw new CustomException("Role Access Link Does Not Exist");
        }

        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        var roleAccessLink = await _db.RoleAccessLink.Update(new RoleAccessLink(roleAccessLinkDto));
        return roleAccessLink.ToDto();
    }

    public async Task<List<RoleAccessLinkDto>> GetAllRoleAccessLink()
    {
        if (_identity.IsSupport == false)
            throw new CustomException("Unauthorized Access.");

        var roleAccessLinks = await _db.RoleAccessLink
                                       .Get()
                                       .AsNoTracking()
                                       .Include(r => r.Role)
                                       .Include(r => r.RoleAccess)
                                       .Select(x => x.ToDto())
                                       .ToListAsync();

        return roleAccessLinks;
    }
}