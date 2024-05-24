using HRIS.Models.Enums;

namespace HRIS.Models;

public class EmployeeSalaryDetailsDto
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public double? Salary { get; set; }
    public double? MinSalary { get; set; }
    public double? MaxSalary { get; set; }
    public double? Remuneration { get; set; }
    public EmployeeSalaryBand? Band { get; set; }
    public string? Contribution { get; set; }
    public DateTime? SalaryUpdateDate { get; set; }
}