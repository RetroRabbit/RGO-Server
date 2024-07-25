using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IEmployeeTypeService
{
    /// <summary>
    ///     Create Employee Type
    /// </summary>
    /// <param name="employeeTypeDto">The employeeTypeDto of the employee to save.</param>
    /// <returns>The saved EmployeeTypeDto object</returns>
    Task<EmployeeTypeDto> CreateEmployeeType(EmployeeTypeDto employeeTypeDto);

    /// <summary>
    ///     Deletes an existing employee type by id.
    /// </summary>
    /// <param name="id">The id of the employee type to delete.</param>
    /// <returns>The deleted EmployeeTypeDto object.</returns>
    Task<EmployeeTypeDto> DeleteEmployeeType(int id);

    /// <summary>
    ///     Get Employee Type by Name
    /// </summary>
    /// <param Id="name">The name of the employee type to get</param>
    /// <returns>The ExistingEmployeeType.</returns>
    Task<EmployeeTypeDto> GetEmployeeType(string name);

    /// <summary>
    ///     Get All Employee Type
    /// </summary>
    /// <returns></returns>
    Task<List<EmployeeTypeDto>> GetAllEmployeeType();

    /// <summary>
    ///     Update Employee Type
    /// </summary>
    /// <param name="employeeTypeDto">The employeeTypeDto of the employee to update.</param>
    /// <returns>The updated EmployeeTypeDto object.</returns>
    Task<EmployeeTypeDto> UpdateEmployeeType(EmployeeTypeDto employeeTypeDto);

    /// <summary>
    ///     Employee Type Exists
    /// </summary>
    /// <param name="id">Check if employee type exist by id.</param>
    /// <returns>True/False based on whether employee type exists.</returns>
    Task<bool> EmployeeTypeExists(int id);
}