namespace RGO.Models;

public record EmployeeEvaluationInput(
    int? Id,
    string OwnerEmail,
    string EmployeeEmail,
    string Template,
    string Subject);