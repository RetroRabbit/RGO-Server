namespace RGO.Models;

public record EmployeeDateDto(
    int Id,
    EmployeeDto? Employee,
    string Subject,
    string Note,
    DateTime Date);
