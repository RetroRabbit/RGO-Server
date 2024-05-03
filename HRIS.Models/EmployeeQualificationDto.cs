namespace HRIS.Models;

public class EmployeeQualificationDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public required string? Qualification { get; set; }
    public required string? School { get; set; }
    public required string? Degree { get; set; }
    public required string? FieldOfStudy { get; set; }
    public required string? NQF { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime? EndDate { get; set; }
}
