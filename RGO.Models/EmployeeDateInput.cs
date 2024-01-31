namespace RGO.Models;

public record EmployeeDateInput(
    string Email,
    string Subject,
    string Note,
    DateOnly Date);
