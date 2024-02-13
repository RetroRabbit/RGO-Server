using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IEmployeeEvaluationService
{
    /// <summary>
    /// Save Employee Evaluation
    /// </summary>
    /// <param name="evaluationInput"></param>
    /// <returns>Added Employee Evaluation</returns>
    Task<EmployeeEvaluationDto> Save(EmployeeEvaluationInput evaluationInput);

    /// <summary>
    /// Delete Employee Evaluation
    /// </summary>
    /// <param name="evaluationInput"></param>
    /// <returns>Deleted Employee Evaluation</returns>
    Task<EmployeeEvaluationDto> Delete(EmployeeEvaluationInput evaluationInput);

    /// <summary>
    /// Get an Employee Evaluation
    /// </summary>
    /// <param name="employeeEmail"></param>
    /// <param name="ownerEmail"></param>
    /// <param name="template"></param>
    /// <param name="subject"></param>
    /// <returns>Employee Evaluation</returns>
    Task<EmployeeEvaluationDto> Get(
        string employeeEmail,
        string ownerEmail,
        string template,
        string subject);

    /// <summary>
    /// Get All Employee Evaluations By Owner email
    /// </summary>
    /// <param name="email"></param>
    /// <returns>List of EmployeeEvaluation</returns>
    Task<List<EmployeeEvaluationDto>> GetAllByOwner(string email);

    /// <summary>
    /// Get All Employee Evaluations By Employee email
    /// </summary>
    /// <param name="email"></param>
    /// <returns>List of EmployeeEvaluation</returns>
    Task<List<EmployeeEvaluationDto>> GetAllByEmployee(string email);

    /// <summary>
    /// Get All Employee Evaluations By Template name
    /// </summary>
    /// <param name="template"></param>
    /// <returns>List of EmployeeEvaluation</returns>
    Task<List<EmployeeEvaluationDto>> GetAllByTemplate(string template);

    /// <summary>
    /// Get All Employee Evaluations By Email
    /// </summary>
    /// <param name="email"></param>
    /// <returns>List of EmployeeEvaluation</returns>
    Task<List<EmployeeEvaluationDto>> GetAllEvaluationsByEmail(string email);

    /// <summary>
    /// Update Employee Evaluation
    /// </summary>
    /// <param name="oldEvaluation"></param>
    /// <param name="newEvaluation"></param>
    /// <returns>Employee Evaluation</returns>
    Task<EmployeeEvaluationDto> Update(
        EmployeeEvaluationInput oldEvaluation,
        EmployeeEvaluationInput newEvaluation);

    /// <summary>
    /// Check if Employee Evaluation exists
    /// </summary>
    /// <param name="evaluationInput"></param>
    /// <returns>true or false</returns>
    Task<bool> CheckIfExists(EmployeeEvaluationInput evaluationInput);
}
