using HRIS.Models.Enums.QualificationEnums;
using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class EmployeeQualificationDto
{
    [Required(ErrorMessage = "Employee Qualification 'Id' field is missing.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Employee Qualification 'EmployeeId' field is missing.")]
    public int EmployeeId { get; set; }

    [Required(ErrorMessage = "Employee Qualification 'HighestQualification' field is missing.")]
    public HighestQualification HighestQualification { get; set; }

    [Required(ErrorMessage = "Employee Qualification 'School' field is missing.")]
    public string School { get; set; } = string.Empty;

    [Required(ErrorMessage = "Employee Qualification 'FieldOfStudy' field is missing.")]
    public string FieldOfStudy { get; set; } = string.Empty;

    [Required(ErrorMessage = "Employee Qualification 'NQFLevel' field is missing.")]
    public NQFLevel NQFLevel { get; set; }

    [Required(ErrorMessage = "Employee Qualification 'Year' field is missing.")]
    public DateOnly Year { get; set; } = DateOnly.MinValue;

    [Required(ErrorMessage = "Employee Qualification 'ProofOfQualification' field is missing.")]
    public byte[] ProofOfQualification { get; set; } = Array.Empty<byte>();
    public string DocumentName { get; set; }
}