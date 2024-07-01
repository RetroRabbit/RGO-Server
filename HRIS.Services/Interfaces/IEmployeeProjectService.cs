using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IEmployeeProjectService
{
    /// <summary>
    ///     Save Employee Project
    /// </summary>
    /// <param name="employeeProjectDto"></param>
    /// <returns></returns>
    Task<EmployeeProjectDto> SaveEmployeeProject(EmployeeProjectDto employeeProjectDto);

    /// <summary>
    ///     Delete Employee Project
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<EmployeeProjectDto> DeleteEmployeeProject(string name);

    /// <summary>
    ///     Get Employee Project
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<EmployeeProjectDto> GetEmployeeProject(string name);
    
    /// <summary>
    ///     Update Employee Project
    /// </summary>
    /// <param name="employeeProjectDto"></param>
    /// <returns></returns>
    Task<EmployeeProjectDto> UpdateEmployeeProject(EmployeeProjectDto employeeProjectDto);
}