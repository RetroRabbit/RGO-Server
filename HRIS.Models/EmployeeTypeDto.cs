using System.ComponentModel.DataAnnotations;

namespace HRIS.Models;

public class EmployeeTypeDto
{
    [Required (ErrorMessage = "Employee Type 'Id' field is missing.")]
    public int Id { get; set; }
    [Required (ErrorMessage = "Employee Type 'Name' field is missing.")]
    public string Name { get; set; }
}
