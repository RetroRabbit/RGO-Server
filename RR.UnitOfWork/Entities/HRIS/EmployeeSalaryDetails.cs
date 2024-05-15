using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models;
using HRIS.Models.Enums;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("EmployeeSalaryDetails")]
public class EmployeeSalaryDetails : IModel<EmployeeSalaryDetailsDto>
{
    public EmployeeSalaryDetails() {}

    public EmployeeSalaryDetails(EmployeeSalaryDetailsDto employeeSalaryDetailsDto)
    {
        Id = employeeSalaryDetailsDto.Id;
        EmployeeId = employeeSalaryDetailsDto.EmployeeId!;
        Salary = employeeSalaryDetailsDto.Salary;
        MinSalary = employeeSalaryDetailsDto.MinSalary;
        MaxSalary = employeeSalaryDetailsDto.MaxSalary;
        Remuneration = employeeSalaryDetailsDto.Remuneration;
        Band = employeeSalaryDetailsDto.Band;
        Contribution = employeeSalaryDetailsDto.Contribution;
    }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }

    //public int Id { get; set; }
    public EmployeeDto? Employee { get; set; }
    [Column("salary")] public double? Salary { get; set; }
    [Column("minSalary")] public double? MinSalary { get; set; }
    [Column("maxSalary")] public double? MaxSalary { get; set; }
    [Column("remuneration")] public double? Remuneration { get; set; }
    [Column("band")] public EmployeeSalaryBand? Band { get; set; }
    [Column("contribution")] public string? Contribution { get; set; }

    //public virtual Employee? Employee { get; set; }

    [Key][Column("id")] public int Id { get; set; }

    public EmployeeSalaryDetailsDto ToDto()
    {
        return new EmployeeSalaryDetailsDto
        {
            Id = Id,
            EmployeeId = EmployeeId,
            Salary = Salary,
            MinSalary = MinSalary,
            MaxSalary = MaxSalary,
            Remuneration = Remuneration,
            Band = Band,
            Contribution = Contribution
        };
    }
}