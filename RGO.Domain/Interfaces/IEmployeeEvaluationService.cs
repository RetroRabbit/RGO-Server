using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IEmployeeEvaluationService
{
    /// <summary>
    /// Save Employee Evaluation
    /// </summary>
    /// <param name="employeeEvaluationDto"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationDto> SaveEmployeeEvaluation(EmployeeEvaluationDto employeeEvaluationDto);

    /// <summary>
    /// Delete Employee Evaluation By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationDto> DeleteEmployeeEvaluationById(int id);

    /// <summary>
    /// Get Employee Evaluation By Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationDto> GetEmployeeEvaluationById(int id);

    /// <summary>
    /// Get All Employee Evaluations By Owner email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<List<EmployeeEvaluationDto>> GetAllEmployeeEvaluationByOwner(string email);

    /// <summary>
    /// Get All Employee Evaluations By Employee email
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<List<EmployeeEvaluationDto>> GetAllEmployeeEvaluationByEmployee(string email);

    /// <summary>
    /// Get All Employee Evaluations By Template name
    /// </summary>
    /// <param name="template"></param>
    /// <returns></returns>
    Task<List<EmployeeEvaluationDto>> GetAllEmployeeEvaluationByTemplate(string template);

    /// <summary>
    /// Get All Employee Evaluations
    /// </summary>
    /// <returns></returns>
    Task<List<EmployeeEvaluationDto>> GetAllEmployeeEvaluations(string email);

    /// <summary>
    /// Update Employee Evaluation
    /// </summary>
    /// <param name="employeeEvaluationDto"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationDto> UpdateEmployeeEvaluation(EmployeeEvaluationDto employeeEvaluationDto);

    /// <summary>
    /// Check If Employee Evaluation Exists
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> CheckIfExists(int id);
}
