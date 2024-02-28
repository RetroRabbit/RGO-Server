using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IEmployeeEvaluationRatingService
{
    /// <summary>
    ///     Check if Employee Evaluation Rating exists
    /// </summary>
    /// <param name="email"></param>
    /// <param name="evaluationId"></param>
    /// <returns>True or False</returns>
    Task<bool> CheckIfExists(EvaluationRatingInput rating);

    /// <summary>
    ///     Get Employee Evaluation Rating
    /// </summary>
    /// <param name="email"></param>
    /// <param name="evaluationInput"></param>
    /// <returns>Employee Evaluation Rating</returns>
    Task<EmployeeEvaluationRatingDto> Get(EvaluationRatingInput rating);

    /// <summary>
    ///     Save Employee Evaluation Rating
    /// </summary>
    /// <param name="employeeEvaluationRatingDto"></param>
    /// <returns>Employee Evaluation Rating</returns>
    Task<EmployeeEvaluationRatingDto> Save(EvaluationRatingInput rating);

    /// <summary>
    ///     Update Employee Evaluation Rating
    /// </summary>
    /// <param name="employeeEvaluationRatingDto"></param>
    /// <returns>Employee Evaluation Rating</returns>
    Task<EmployeeEvaluationRatingDto> Update(EvaluationRatingInput rating);

    /// <summary>
    ///     Delete Employee Evaluation Rating
    /// </summary>
    /// <param name="email"></param>
    /// <param name="evaluationInput"></param>
    /// <returns>Employee Evaluation Rating</returns>
    Task<EmployeeEvaluationRatingDto> Delete(EvaluationRatingInput rating);

    /// <summary>
    ///     Get All Employee Evaluation Ratings
    /// </summary>
    /// <returns>List of Employee Evaluation Rating</returns>
    Task<List<EmployeeEvaluationRatingDto>> GetAll();

    /// <summary>
    ///     Get All Employee Evaluation Ratings By Employee
    /// </summary>
    /// <param name="email"></param>
    /// <returns>List of Employee Evaluation Rating</returns>
    Task<List<EmployeeEvaluationRatingDto>> GetAllByEmployee(string email);

    /// <summary>
    ///     Get All Employee Evaluation Ratings By Evaluation
    /// </summary>
    /// <param name="evaluationInput"></param>
    /// <returns>List of Employee Evaluation Rating</returns>
    Task<List<EmployeeEvaluationRatingDto>> GetAllByEvaluation(EmployeeEvaluationInput evaluationInput);
}