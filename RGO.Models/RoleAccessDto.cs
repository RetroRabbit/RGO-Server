namespace RGO.Models;

public record RoleAccessDto(
    int Id,
    RoleDto Role,
    string Action,
    bool View,
    bool Edit,
    bool Delete);
