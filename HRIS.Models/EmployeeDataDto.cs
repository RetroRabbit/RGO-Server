using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class EmployeeDataDto
{
    [Required(ErrorMessage = "Employee Data 'Id' field is missing.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Employee Data 'EmployeeId' field is missing.")]
    public int EmployeeId { get; set; }
    [Required(ErrorMessage = "Employee Data 'FieldCodeId' field is missing.")]
    public int FieldCodeId { get; set; }
    [Required(ErrorMessage = "Employee Data 'Value' field is missing.")]
    public string? Value { get; set; }
}
