namespace RGO.Models;

public record ChartRoleLinkDto(
    int Id,
    ChartDto? Chart,
    RoleDto? Role
    );