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
    /// Delete Employee Evaluation
    /// </summary>
    /// <param name="employeeEvaluationDto"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationDto> DeleteEmployeeEvaluation(EmployeeEvaluationDto employeeEvaluationDto);

    /// <summary>
    /// Get Employee Evaluation
    /// </summary>
    /// <param name="employeeEvaluationDto"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationDto> GetEmployeeEvaluation(EmployeeEvaluationDto employeeEvaluationDto);

    /// <summary>
    /// Get All Employee Evaluations
    /// </summary>
    /// <returns></returns>
    Task<List<EmployeeEvaluationDto>> GetAllEmployeeEvaluations();

    /// <summary>
    /// Update Employee Evaluation
    /// </summary>
    /// <param name="employeeEvaluationDto"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationDto> UpdateEmployeeEvaluation(EmployeeEvaluationDto employeeEvaluationDto);
}
