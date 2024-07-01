using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.Tests.Data.Models.HRIS;

public class EmployeeSalaryDetailsTestData
{
    public static EmployeeSalaryDetails EmployeeSalaryDetailsOne = new()
    {
        Id = 1,
        EmployeeId = EmployeeTestData.EmployeeOne.Id,
        Salary = 15000,
        MinSalary = 9000,
        MaxSalary = 20000,
        Remuneration = 15000,
        Band = EmployeeSalaryBand.Level1,
        Contribution = null,
        SalaryUpdateDate = DateTime.Now
    };

    public static EmployeeSalaryDetails EmployeeSalaryDetailsTwo = new()
    {
        Id = 2,
        EmployeeId = EmployeeTestData.EmployeeOne.Id,
        Salary = 25000,
        MinSalary = 12000,
        MaxSalary = 40000,
        Remuneration = 25000,
        Band = EmployeeSalaryBand.Level3,
        Contribution = null,
        SalaryUpdateDate = DateTime.Now
    };
}