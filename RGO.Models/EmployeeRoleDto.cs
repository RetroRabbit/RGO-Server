namespace RGO.Models;

public record EmployeeRoleDto(
    int Id,
    EmployeeDto? Employee,
    RoleDto? Role);