using HRIS.Models.Enums.QualificationEnums;
using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class EmployeeQualificationDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }

    [Required]
    public HighestQualification HighestQualification { get; set; }
    [Required]
    public string? School { get; set; }
    [Required]
    public string? Degree { get; set; }
    [Required]
    public string? FieldOfStudy { get; set; }
    [Required]
    public NQFLevel NQFLevel { get; set; }
    [Required]
    public DateOnly? Year { get; set; }
}