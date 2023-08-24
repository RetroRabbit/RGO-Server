using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IEmployeeTypeService
{
    /// <summary>
    /// Save Employee Type
    /// </summary>
    /// <param name="employeeTypeDto"></param>
    /// <returns></returns>
    Task<EmployeeTypeDto> SaveEmployeeType(EmployeeTypeDto employeeTypeDto);

    /// <summary>
    /// Delete Employee Type
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<EmployeeTypeDto> DeleteEmployeeType(string name);

    /// <summary>
    /// Get Employee Type
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<EmployeeTypeDto> GetEmployeeType(string name);

    /// <summary>
    /// Get All Employee Type
    /// </summary>
    /// <returns></returns>
    Task<List<EmployeeTypeDto>> GetAllEmployeeType();

    /// <summary>
    /// Update Employee Type
    /// </summary>
    /// <param name="employeeTypeDto"></param>
    /// <returns></returns>
    Task<EmployeeTypeDto> UpdateEmployeeType(EmployeeTypeDto employeeTypeDto);
}
