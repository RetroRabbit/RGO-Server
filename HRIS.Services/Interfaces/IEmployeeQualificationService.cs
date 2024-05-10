using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Interfaces;

public interface IEmployeeQualificationService
{
    /// <summary>
    /// Saves a new employee qualification.
    /// </summary>
    /// <param name="employeeQualificationDto">Qualification data to be saved.</param>
    /// <param name="employeeId">ID of the employee to whom the qualification belongs.</param>
    /// <returns>The saved employee qualification data transfer object.</returns>
    Task<EmployeeQualificationDto> SaveEmployeeQualification(EmployeeQualificationDto employeeQualificationDto, int employeeId);

    /// <summary>
    /// Retrieves all employee qualifications.
    /// </summary>
    /// <returns>A list of employee qualification data transfer objects.</returns>
    Task<List<EmployeeQualificationDto>> GetAllEmployeeQualifications();

    /// <summary>
    /// Retrieves a specific employee qualification by its ID.
    /// </summary>
    /// <param name="id">The ID of the qualification to retrieve.</param>
    /// <returns>The requested employee qualification data transfer object.</returns>
    Task<EmployeeQualificationDto> GetEmployeeQualificationById(int id);

    /// <summary>
    /// Retrieves all qualifications associated with a specific employee.
    /// </summary>
    /// <param name="employeeId">The ID of the employee for whom to retrieve all qualifications.</param>
    /// <returns>The requested employee qualification data transfer object.</returns>
    Task<List<EmployeeQualificationDto>> GetAllEmployeeQualificationsByEmployeeId(int employeeId);

    /// <summary>
    /// Updates an existing employee qualification.
    /// </summary>
    /// <param name="employeeQualificationDto">Updated qualification data.</param>
    /// <returns>The updated employee qualification data transfer object.</returns>
    Task<EmployeeQualificationDto> UpdateEmployeeQualification(EmployeeQualificationDto employeeQualificationDto);

    /// <summary>
    /// Deletes an employee qualification by its ID.
    /// </summary>
    /// <param name="id">The ID of the qualification to delete.</param>
    /// <returns></returns>
    Task<EmployeeQualificationDto> DeleteEmployeeQualification(int id);
}
