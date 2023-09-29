namespace RGO.Models;

public record EmployeeEvaluationAudienceDto(
    int Id,
    EmployeeEvaluationDto? Evaluation,
    EmployeeDto? Employee);