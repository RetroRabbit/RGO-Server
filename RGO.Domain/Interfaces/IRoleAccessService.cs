using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IRoleAccessService
{
    Task<RoleAccessDto> AddRoleAccess(RoleAccessDto roleAccessDto);
    Task<RoleAccessDto> DeleteRoleAccess(string action);
    Task<RoleAccessDto> GetRoleAccess(string action);
    Task<List<RoleAccessDto>> GetAllRoleAccess();
    Task<RoleAccessDto> UpdateRoleAccess(RoleAccessDto roleAccessDto);
}
