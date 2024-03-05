using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IEmployeeCertificationService
{
    /// <summary>
    ///     Save Employee Certification
    /// </summary>
    /// <param name="employeeCertificationDto"></param>
    /// <returns></returns>
    Task<EmployeeCertificationDto> SaveEmployeeCertification(EmployeeCertificationDto employeeCertificationDto);

    /// <summary>
    ///     Get Employee Certification
    /// </summary>
    /// <param name="employeeId"></param>
    /// <param name="certificationId"></param>
    /// <returns></returns>
    Task<EmployeeCertificationDto> GetEmployeeCertification(int employeeId, int certificationId);

    /// <summary>
    ///     Get All Employee Certifications
    /// </summary>
    /// <param name="employeeId"></param>
    /// <returns></returns>
    Task<List<EmployeeCertificationDto>> GetAllEmployeeCertifications(int employeeId);

    /// <summary>
    ///     Update Employee Certification
    /// </summary>
    /// <param name="employeeCertificationDto"></param>
    /// <returns></returns>
    Task<EmployeeCertificationDto> UpdateEmployeeCertification(EmployeeCertificationDto employeeCertificationDto);

    /// <summary>
    ///     Delete Employee Certification
    /// </summary>
    /// <param name="employeeCertificationDto"></param>
    /// <returns></returns>
    Task<EmployeeCertificationDto> DeleteEmployeeCertification(EmployeeCertificationDto employeeCertificationDto);
}