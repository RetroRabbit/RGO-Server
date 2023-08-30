namespace RGO.Models;

public record RoleAccessLinkDto(
    int Id,
    RoleDto Role,
    RoleAccessDto RoleAccess);