namespace RGO.Models;

public record EmployeeEvaluationDto(
    int Id,
    int EmployeeId,
    int TemplateId,
    int OwnerId,
    string Subject,
    DateTime StartDate,
    DateTime? EndDate);
