using HRIS.Models;
using HRIS.Models.Enums;

namespace RR.Tests.Data.Models.HRIS;

public class EmployeeSalaryDetailsTestData
{
    public static EmployeeSalaryDetailsDto EmployeeSalaryTest1 = new EmployeeSalaryDetailsDto
    {
        Id = 1,
        EmployeeId = EmployeeTestData.EmployeeDto.Id,
        Salary = 15000,
        MinSalary = 9000,
        MaxSalary = 20000,
        Remuneration = 15000,
        Band = EmployeeSalaryBand.Level1,
        Contribution = null,
        SalaryUpdateDate = DateTime.Now
    };

    public static EmployeeSalaryDetailsDto EmployeeSalaryTest2 = new EmployeeSalaryDetailsDto
    {
        Id = 2,
        EmployeeId = EmployeeTestData.EmployeeDto.Id,
        Salary = 25000,
        MinSalary = 12000,
        MaxSalary = 40000,
        Remuneration = 25000,
        Band = EmployeeSalaryBand.Level3,
        Contribution = null,
        SalaryUpdateDate = DateTime.Now
    };
}