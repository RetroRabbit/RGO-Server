using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IEmployeeQualificationService
{
    /// <summary>
    /// Save Employee Qualification
    /// </summary>
    /// <param name="employeeQualificationDto"></param>
    /// <returns></returns>
    Task<EmployeeQualificationDto> SaveEmployeeQualification(EmployeeQualificationDto employeeQualificationDto);

    /// <summary>
    /// Get Employee Qualification
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<EmployeeQualificationDto> GetEmployeeQualification(string name);

    /// <summary>
    /// Get All Employee Qualifications
    /// </summary>
    /// <returns></returns>
    Task<EmployeeQualificationDto> GetAllEmployeeQualifications();

    /// <summary>
    /// Update Employee Qualification
    /// </summary>
    /// <param name="employeeQualificationDto"></param>
    /// <returns></returns>
    Task<EmployeeQualificationDto> UpdateEmployeeQualification(EmployeeQualificationDto employeeQualificationDto);

    /// <summary>
    /// Delete Employee Qualification
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task<EmployeeQualificationDto> DeleteEmployeeQualification(string name);
}

