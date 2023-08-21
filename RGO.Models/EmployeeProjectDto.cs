namespace RGO.Models;

public record EmployeeProjectDto(
    int Id,
    int EmployeeId,
    string Name,
    string Description,
    string Client,
    DateTime StartDate,
    DateTime? EndDate);
