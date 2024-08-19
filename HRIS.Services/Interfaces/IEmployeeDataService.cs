using HRIS.Models.Employee.Commons;

namespace HRIS.Services.Interfaces;

public interface IEmployeeDataService
{
    /// <summary>
    ///     Save Employee Data
    /// </summary>
    /// <param name="employeeDataDto"></param>
    /// <returns>Employee data</returns>
    Task<EmployeeDataDto> CreateEmployeeData(EmployeeDataDto employeeDataDto);

    /// <summary>
    ///     Get Employee Data
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="value"></param>
    /// <returns>Employee data</returns>
    Task<EmployeeDataDto> GetEmployeeData(int employeeId);

    /// <summary>
    ///     Update Employee Data
    /// </summary>
    /// <param name="employeeDataDto"></param>
    /// <returns>Employee data</returns>
    Task<EmployeeDataDto> UpdateEmployeeData(EmployeeDataDto employeeDataDto);

    /// <summary>
    ///     Delete Employee Data
    /// </summary>
    /// <param name="employeeDataId"></param>
    /// <returns>Employee data</returns>
    Task<EmployeeDataDto> DeleteEmployeeData(int employeeDataId);
}