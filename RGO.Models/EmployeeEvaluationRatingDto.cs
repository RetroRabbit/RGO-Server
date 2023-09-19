namespace RGO.Models;

public record EmployeeEvaluationRatingDto(
    int Id,
    EmployeeEvaluationDto? Evaluation,
    EmployeeDto? Employee,
    float Score,
    string Comment);
