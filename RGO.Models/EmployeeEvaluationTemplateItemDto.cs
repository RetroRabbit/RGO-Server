namespace RGO.Models;

public record EmployeeEvaluationTemplateItemDto(
    int Id,
    int TemplateId,
    string Section,
    string Question);
