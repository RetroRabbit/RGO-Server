using HRIS.Models.Employee.Commons;

namespace HRIS.Services.Interfaces;

public interface IEmployeeQualificationService
{
    /// <summary>
    ///     Check if user exist
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Boolean to check if the employee qualification exists</returns>
    Task<bool> CheckIfExists(int id);
    /// <summary>
    /// Saves a new employee qualification.
    /// </summary>
    /// <param name="employeeQualificationDto">Qualification data to be saved.</param>
    /// <param name="employeeId">id of the employee to whom the qualification belongs.</param>
    /// <returns>The saved employee qualification data transfer object.</returns>
    /// <exception cref="HRIS.Services.Services.CustomException"></exception>
    Task<EmployeeQualificationDto> CreateEmployeeQualification(EmployeeQualificationDto employeeQualificationDto, int employeeId);

    /// <summary>
    /// Retrieves all employee qualifications.
    /// </summary>
    /// <returns>A list of employee qualification data transfer objects.</returns>
    Task<List<EmployeeQualificationDto>> GetAllEmployeeQualifications();

    /// <summary>
    /// Retrieves all qualifications associated with a specific employee.
    /// </summary>
    /// <param name="employeeId">The id of the employee for whom to retrieve all qualifications.</param>
    /// <returns>The requested employee qualification data transfer object.</returns>
    /// <exception cref="HRIS.Services.Services.CustomException"></exception>
    Task<EmployeeQualificationDto> GetEmployeeQualificationsByEmployeeId(int employeeId);

    /// <summary>
    /// Updates an existing employee qualification.
    /// </summary>
    /// <param name="employeeQualificationDto">Updated qualification data.</param>
    /// <returns>The updated employee qualification data transfer object.</returns>
    /// <exception cref="HRIS.Services.Services.CustomException"></exception>
    Task<EmployeeQualificationDto> UpdateEmployeeQualification(EmployeeQualificationDto employeeQualificationDto);

    /// <summary>
    /// Deletes an employee qualification by its id.
    /// </summary>
    /// <param name="id">The ID of the qualification to delete.</param>
    /// <returns>The deleted employee qualification data transfer object</returns>
    /// <exception cref="HRIS.Services.Services.CustomException"></exception>
    Task<EmployeeQualificationDto> DeleteEmployeeQualification(int id);
}
