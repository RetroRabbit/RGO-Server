namespace RGO.Models;

public record EvaluationRatingInput(
    int Id,
    string EmployeeEmail,
    EmployeeEvaluationInput Evaluation,
    string Description,
    float Score,
    string Comment);