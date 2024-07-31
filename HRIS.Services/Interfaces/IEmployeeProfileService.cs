using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IEmployeeProfileService
{
    /// <summary>
    ///     Get employee by id
    /// </summary>
    /// <param name="employeeEmail"></param>
    /// <returns>An employee dto including type and addresses</returns>
    Task<EmployeeProfileDto> GetEmployeeProfileById(int? id);
}
