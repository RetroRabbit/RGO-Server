using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IRoleAccessService
{
    Task<RoleAccessDto> SaveRoleAccess(RoleAccessDto roleAccessDto);
    Task<RoleAccessDto> DeleteRoleAccess(string action);
    Task<RoleAccessDto> GetRoleAccess(string action);
    Task<List<RoleAccessDto>> GetRoleAccessByRole(string description);
    Task<List<RoleAccessDto>> GetAllRoleAccess();
    Task<RoleAccessDto> UpdateRoleAccess(RoleAccessDto roleAccessDto);
}
