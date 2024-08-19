using HRIS.Models.Employee.Commons;

namespace HRIS.Services.Interfaces;

public interface IEmployeeSalaryDetailsService
{
    /// <summary>
    /// Create Employee Salary
    /// </summary>
    /// <param name="employeeSalaryDto">The employeeSalaryDto of the employee salary details to save.</param>
    /// <returns>The saved EmployeeSalaryDto object.</returns>
    Task<BankingSalaryDetailsDto> CreateEmployeeSalary(BankingSalaryDetailsDto employeeSalaryDto);
    /// <summary>
    /// Get Employee Salary
    /// </summary>
    /// <param name="employeeId">The employeeId of the employee salary details to get.</param>
    /// <returns>The employeeSalaryDetailsDto object.</returns>
    Task<BankingSalaryDetailsDto> GetEmployeeSalaryById(int employeeId);
    /// <summary>
    /// Get All Employee Salaries
    /// </summary>
    /// <returns></returns>
    Task<List<BankingSalaryDetailsDto>> GetAllEmployeeSalaries();
    /// <summary>
    /// Update Employee Salary
    /// </summary>
    /// <param name="employeeSalaryDto">The epmployeeSalaryDto to update.</param>
    /// <returns>The updated employeeSalarayDto object.</returns>
    Task<BankingSalaryDetailsDto> UpdateEmployeeSalary(BankingSalaryDetailsDto employeeSalaryDto);
    /// <summary>
    /// Delete Employee Salary
    /// </summary>
    /// <param name="employeeId">The employeeId of the employee salary details to delete.</param>
    /// <returns>The deleted employeeSalaryDetails object.</returns>
    Task<BankingSalaryDetailsDto> DeleteEmployeeSalary(int employeeId);
    /// <summary>
    ///     Employee Type Exists
    /// </summary>
    /// <param name="id">Check if employee type exist by id.</param>
    /// <returns>True/False based on whether employee type exists.</returns>
    Task<bool> EmployeeSalaryDetailsExists(int id);
}