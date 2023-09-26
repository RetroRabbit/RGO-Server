using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IEmployeeEvaluationAudienceService
{
    /// <summary>
    /// Check if Employee Evaluation Audience exists
    /// </summary>
    /// <param name="evaluation"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<bool> CheckIfExists(EmployeeEvaluationDto evaluation, string email);

    /// <summary>
    /// Get Employee Evaluation Audience
    /// </summary>
    /// <param name="evaluation"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationAudienceDto> Get(EmployeeEvaluationDto evaluation, string email);

    /// <summary>
    /// Save Employee Evaluation Audience
    /// </summary>
    /// <param name="email"></param>
    /// <param name="evaluationInput"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationAudienceDto> Save(string email, EmployeeEvaluationInput evaluationInput);

    /// <summary>
    /// Delete Employee Evaluation Audience
    /// </summary>
    /// <param name="evaluation"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationAudienceDto> Delete(string email, EmployeeEvaluationInput evaluationInput);

    /// <summary>
    /// Update Employee Evaluation Audience
    /// </summary>
    /// <param name="employeeEvaluationAudienceDto"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationAudienceDto> Update(EmployeeEvaluationAudienceDto employeeEvaluationAudienceDto);

    /// <summary>
    /// Get all Employee Evaluation Audiences
    /// </summary>
    /// <returns></returns>
    Task<List<EmployeeEvaluationAudienceDto>> GetAll();

    /// <summary>
    /// Get all Employee Evaluation Audiences by Evaluation
    /// </summary>
    /// <param name="evaluation"></param>
    /// <returns></returns>
    Task<List<EmployeeEvaluationAudienceDto>> GetAllbyEvaluation(EmployeeEvaluationInput evaluation);

    /// <summary>
    /// Get all Employee Evaluation Audiences by Employee
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<List<EmployeeEvaluationAudienceDto>> GetAllbyEmployee(string email);
}
