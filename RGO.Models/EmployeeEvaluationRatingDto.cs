namespace RGO.Models;

public record EmployeeEvaluationRatingDto(
    int Id,
    int EmployeeEvaluationId,
    int EmployeeId,
    float Score,
    string Comment);
