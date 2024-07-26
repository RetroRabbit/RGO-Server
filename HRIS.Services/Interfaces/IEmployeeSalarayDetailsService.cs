using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IEmployeeSalarayDetailsService
{
    /// <summary>
    /// Create Employee Salary
    /// </summary>
    /// <param name="employeeSalaryDto">The employeeSalaryDto of the employee salary details to save.</param>
    /// <returns>The saved EmployeeSalaryDto object.</returns>
    Task<EmployeeSalaryDetailsDto> CreateEmployeeSalary(EmployeeSalaryDetailsDto employeeSalaryDto);
    /// <summary>
    /// Get Employee Salary
    /// </summary>
    /// <param name="employeeId">The employeeId of the employee salary details to get.</param>
    /// <returns>The employeeSalaryDetailsDto object.</returns>
    Task<EmployeeSalaryDetailsDto> GetEmployeeSalaryById(int employeeId);
    /// <summary>
    /// Get All Employee Salaries
    /// </summary>
    /// <returns></returns>
    Task<List<EmployeeSalaryDetailsDto>> GetAllEmployeeSalaries();
    /// <summary>
    /// Update Employee Salary
    /// </summary>
    /// <param name="employeeSalaryDto">The epmployeeSalaryDto to update.</param>
    /// <returns>The updated employeeSalarayDto object.</returns>
    Task<EmployeeSalaryDetailsDto> UpdateEmployeeSalary(EmployeeSalaryDetailsDto employeeSalaryDto);
    /// <summary>
    /// Delete Employee Salary
    /// </summary>
    /// <param name="employeeId">The employeeId of the employee salary details to delete.</param>
    /// <returns>The deleted employeeSalaryDetails object.</returns>
    Task<EmployeeSalaryDetailsDto> DeleteEmployeeSalary(int employeeId);
    /// <summary>
    ///     Employee Type Exists
    /// </summary>
    /// <param name="id">Check if employee type exist by id.</param>
    /// <returns>True/False based on whether employee type exists.</returns>
    Task<bool> EmployeeSalaryDetailsExists(int id);
}