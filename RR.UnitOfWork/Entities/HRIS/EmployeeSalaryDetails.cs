using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HRIS.Models.Employee.Commons;
using HRIS.Models.Enums;
using RR.UnitOfWork.Interfaces;

namespace RR.UnitOfWork.Entities.HRIS;

[Table("EmployeeSalaryDetails")]
public class EmployeeSalaryDetails : IModel
{
    public EmployeeSalaryDetails() {}

    public EmployeeSalaryDetails(BankingSalaryDetailsDto employeeSalaryDetailsDto)
    {
        Id = employeeSalaryDetailsDto.Id;
        EmployeeId = employeeSalaryDetailsDto.EmployeeId!;
        Salary = employeeSalaryDetailsDto.Salary;
        MinSalary = employeeSalaryDetailsDto.MinSalary;
        MaxSalary = employeeSalaryDetailsDto.MaxSalary;
        Remuneration = employeeSalaryDetailsDto.Remuneration;
        Band = employeeSalaryDetailsDto.Band;
        Contribution = employeeSalaryDetailsDto.Contribution;
        SalaryUpdateDate = employeeSalaryDetailsDto.SalaryUpdateDate;
    }

    [Column("employeeId")]
    [ForeignKey("Employee")]
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    [Column("salary")] public double? Salary { get; set; }
    [Column("minSalary")] public double? MinSalary { get; set; }
    [Column("maxSalary")] public double? MaxSalary { get; set; }
    [Column("remuneration")] public double? Remuneration { get; set; }
    [Column("band")] public EmployeeSalaryBand? Band { get; set; }
    [Column("contribution")] public string? Contribution { get; set; }
    [Column("salaryUpdateDate")] public DateTime? SalaryUpdateDate { get; set; }

    [Key][Column("id")] public int Id { get; set; }

    public BankingSalaryDetailsDto ToDto()
    {
        return new BankingSalaryDetailsDto
        {
            Id = Id,
            EmployeeId = EmployeeId,
            Salary = Salary,
            MinSalary = MinSalary,
            MaxSalary = MaxSalary,
            Remuneration = Remuneration,
            Band = Band,
            Contribution = Contribution,
            SalaryUpdateDate = SalaryUpdateDate
        };
    }
}