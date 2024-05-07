using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HRIS.Models.Enums;

namespace HRIS.Models;

public class EmployeeSalaryDetailsDto
{
    public int Id { get; set; }
    //public EmployeeDto? Employee { get; set; }
    public int EmployeeId { get; set; }
    public float? Salary { get; set; }
    public float? MinSalary { get; set; }
    public float? MaxSalary { get; set; }
    public float? Remuneration { get; set; }
    public EmployeeSalaryBand? Band { get; set; }
    public string? Contribution { get; set; }
}