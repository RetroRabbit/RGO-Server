using HRIS.Models.Enums;

namespace HRIS.Models.EmployeeProfileModels;

public class EmployeeProfileSalaryDto
{
    public string? TaxNumber { get; set; }
    public float? LeaveInterval { get; set; }
    public float? SalaryDays { get; set; }
    public float? PayRate { get; set; }
    public int? Salary { get; set; }
}
