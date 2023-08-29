using RGO.Models;
namespace RGO.Services.Interfaces;

public interface IRoleAccessLinkService
{
    Task<Dictionary<string, List<string>>> GetAll();
    Task<Dictionary<string, List<string>>> GetByRole(string role);
    Task<Dictionary<string, List<string>>> GetByPermission(string permission);
    Task<RoleAccessLinkDto> Save(RoleAccessLinkDto roleAccessLinkDto);
    Task<RoleAccessLinkDto> Update(RoleAccessLinkDto roleAccessLinkDto);
    Task<RoleAccessLinkDto> Delete(string role, string permission);
    Task<Dictionary<string, List<string>>> GetRoleByEmployee(string email);
}
