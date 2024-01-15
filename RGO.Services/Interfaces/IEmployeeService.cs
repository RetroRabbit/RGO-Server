using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IEmployeeService
{
    /// <summary>
    /// Check if user exist
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<bool> CheckUserExist(string email);

    /// <summary>
    /// Get all employees
    /// </summary>
    /// <returns></returns>
    Task<List<EmployeeDto>> GetAll();

    /// <summary>
    /// Get employee by email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<EmployeeDto> GetEmployee(string email);

    /// <summary>
    /// Get employee by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>An employee dto including type and addresses</returns>
    Task<EmployeeDto> GetEmployeeById(int id);

    /// <summary>
    /// Save employee
    /// </summary>
    /// <param name="employeeDto"></param>
    /// <returns></returns>
    Task<EmployeeDto> SaveEmployee(EmployeeDto employeeDto);

    /// <summary>
    /// Update employee
    /// </summary>
    /// <param name="employeeDto"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<EmployeeDto> UpdateEmployee(EmployeeDto employeeDto, string email);

    /// <summary>
    /// Delete employee
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<EmployeeDto> DeleteEmployee(string email);

    Task<List<EmployeeDto>> GetEmployeesByType(string type);

    /// <summary>
    /// Get employee by the id
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns>EmployeeDto</returns>
    Task<EmployeeDto> GetById(int employeeId);


    /// <summary>
    /// Get simple employee profile for non admin users
    /// </summary>
    /// <returns>SimpeEmployeeProfileDto</returns>
    Task<SimpleEmployeeProfileDto> GetSimpleProfile(string employeeEmail);
}
