using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IEmployeeSalarayDetailsService
{
    /// <summary>
    /// Save Employee Salary
    /// </summary>
    /// <param name="employeeSalaryDto"></param>
    /// <returns></returns>
    Task<EmployeeSalaryDetailsDto> SaveEmployeeSalary(EmployeeSalaryDetailsDto employeeSalaryDto);
    /// <summary>
    /// Get Employee Salary
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns></returns>
    Task<EmployeeSalaryDetailsDto> GetEmployeeSalary(int employeeId);
    /// <summary>
    /// Get All Employee Salaries
    /// </summary>
    /// <returns></returns>
    Task<EmployeeSalaryDetailsDto> GetAllEmployeeSalaries();
    /// <summary>
    /// Update Employee Salary
    /// </summary>
    /// <param name="employeeSalaryDto"></param>
    /// <returns></returns>
    Task<EmployeeSalaryDetailsDto> UpdateEmployeeSalary(EmployeeSalaryDetailsDto employeeSalaryDto);
    /// <summary>
    /// Delete Employee Salary
    /// </summary>
    /// <param name="employeeSalaryDto"></param>
    /// <returns></returns>
    Task<EmployeeSalaryDetailsDto> DeleteEmployeeSalary(EmployeeSalaryDetailsDto employeeSalaryDto);
}