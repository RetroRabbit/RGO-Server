namespace RGO.Models;

public record AuditLogDto(
    int Id,
    EmployeeDto? EditFor,
    EmployeeDto? EditBy,
    DateTime EditDate,
    string Description);