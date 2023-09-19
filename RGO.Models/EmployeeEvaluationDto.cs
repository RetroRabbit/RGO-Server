namespace RGO.Models;

public record EmployeeEvaluationDto(
    int Id,
    EmployeeDto? Employee,
    EmployeeEvaluationTemplateDto? Template,
    EmployeeDto? Owner,
    string Subject,
    DateOnly StartDate,
    DateOnly? EndDate);
