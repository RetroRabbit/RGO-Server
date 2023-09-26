using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IEmployeeEvaluationRatingService
{
    /// <summary>
    /// Save Employee Evaluation Rating
    /// </summary>
    /// <param name="employeeEvaluationRatingDto"></param>
    /// <returns>Employee Evaluation Rating</returns>
    Task<EmployeeEvaluationRatingDto> Save(EmployeeEvaluationRatingDto employeeEvaluationRatingDto);

    /// <summary>
    /// Delete Employee Evaluation Rating
    /// </summary>
    /// <param name="email"></param>
    /// <param name="evaluationInput"></param>
    /// <returns>Employee Evaluation Rating</returns>
    Task<EmployeeEvaluationRatingDto> Delete(string email, EmployeeEvaluationInput evaluationInput);

    /// <summary>
    /// Get Employee Evaluation Rating
    /// </summary>
    /// <param name="email"></param>
    /// <param name="evaluationInput"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationRatingDto> Get(string email, EmployeeEvaluationInput evaluationInput);

    /// <summary>
    /// Get All Employee Evaluation Ratings
    /// </summary>
    /// <returns>List of Employee Evaluation Rating</returns>
    Task<List<EmployeeEvaluationRatingDto>> GetAll();

    /// <summary>
    /// Get All Employee Evaluation Ratings By Evaluation
    /// </summary>
    /// <param name="evaluationInput"></param>
    /// <returns>List of Employee Evaluation Rating</returns>
    Task<List<EmployeeEvaluationRatingDto>> GetAllByEvaluation(EmployeeEvaluationInput evaluationInput);

    /// <summary>
    /// Get All Employee Evaluation Ratings By Employee
    /// </summary>
    /// <param name="email"></param>
    /// <returns>List of Employee Evaluation Rating</returns>
    Task<List<EmployeeEvaluationRatingDto>> GetAllByEmployee(string email);

    /// <summary>
    /// Update Employee Evaluation Rating
    /// </summary>
    /// <param name="employeeEvaluationRatingDto"></param>
    /// <returns>Employee Evaluation Rating</returns>
    Task<EmployeeEvaluationRatingDto> Update(EmployeeEvaluationRatingDto employeeEvaluationRatingDto);

    /// <summary>
    /// Check if Employee Evaluation Rating exists
    /// </summary>
    /// <param name="email"></param>
    /// <param name="evaluationId"></param>
    /// <returns>True or False</returns>
    Task<bool> CheckIfExists(string email, int evaluationId);
}
