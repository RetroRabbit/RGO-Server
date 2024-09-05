using HRIS.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRIS.Models;

public class EmployeeSalaryDetailsDto
{
    [Required(ErrorMessage = "Employee Role 'Id' field is missing.")]
    public int Id { get; set; }
    [Required(ErrorMessage = "Employee Role 'EmployeeId' field is missing.")]
    public int EmployeeId { get; set; }
    public double? Salary { get; set; }
    public double? MinSalary { get; set; }
    public double? MaxSalary { get; set; }
    public double? Remuneration { get; set; }
    public EmployeeSalaryBand? Band { get; set; }
    public string? Contribution { get; set; }
    public DateTime? SalaryUpdateDate { get; set; }
    public string? TaxNumber { get; set; }

}