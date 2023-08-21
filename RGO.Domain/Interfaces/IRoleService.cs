using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IRoleService
{
    Task<RoleDto> GetRole(string name);
    Task<List<RoleDto>> GetAll();
    Task<RoleDto> AddRole(RoleDto roleDto);
    Task<RoleDto> UpdateRole(string name);
    Task<RoleDto> DeleteRole(string name);
}
