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
    /// Get Employee Qualifications
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<EmployeeQualificationDto> GetEmployeeQualification(int id);

    /// <summary>
    ///     Get All Employee Qualifications
    /// </summary>
    /// <returns></returns>
    Task<List<EmployeeQualificationDto>> GetAllEmployeeQualifications();

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
    Task<EmployeeQualificationDto> DeleteEmployeeQualification(int id);
}

