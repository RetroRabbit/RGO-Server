namespace RGO.Models;

public record EmployeeEvaluationRatingDto(
    int Id,
    EmployeeEvaluationDto? Evaluation,
    EmployeeDto? Employee,
    string Description,
    float Score,
    string Comment);
