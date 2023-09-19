namespace RGO.Models;

public record EmployeeEvaluationTemplateItemDto(
    int Id,
    EmployeeEvaluationTemplateDto? Template,
    string Section,
    string Question);
