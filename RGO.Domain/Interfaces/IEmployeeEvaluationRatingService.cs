using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IEmployeeEvaluationRatingService
{
    /// <summary>
    /// Save Employee Evaluation Rating
    /// </summary>
    /// <param name="employeeEvaluationRatingDto"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationRatingDto> SaveEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto);

    /// <summary>
    /// Delete Employee Evaluation Rating
    /// </summary>
    /// <param name="employeeEvaluationRatingDto"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationRatingDto> DeleteEmployeeEvaluationRating(string email, EmployeeEvaluationDto evaluation);

    /// <summary>
    /// Get Employee Evaluation Rating
    /// </summary>
    /// <param name="employeeEvaluationRatingDto"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationRatingDto> GetEmployeeEvaluationRating(string email, EmployeeEvaluationDto evaluation);

    /// <summary>
    /// Get All Employee Evaluation Ratings
    /// </summary>
    /// <returns></returns>
    Task<List<EmployeeEvaluationRatingDto>> GetAllEmployeeEvaluationRatings();

    /// <summary>
    /// Get All Employee Evaluation Ratings By Evaluation
    /// </summary>
    /// <param name="employeeEvaluationDto"></param>
    /// <returns></returns>
    Task<List<EmployeeEvaluationRatingDto>> GetAllEmployeeEvaluationRatingsByEvaluation(EmployeeEvaluationDto employeeEvaluationDto);

    /// <summary>
    /// Get All Employee Evaluation Ratings By Employee
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<List<EmployeeEvaluationRatingDto>> GetAllEmployeeEvaluationRatingsByEmployee(string email);

    /// <summary>
    /// Update Employee Evaluation Rating
    /// </summary>
    /// <param name="employeeEvaluationRatingDto"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationRatingDto> UpdateEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto);

    /// <summary>
    /// Check if Employee Evaluation Rating exists
    /// </summary>
    /// <param name="employeeEvaluationRatingDto"></param>
    /// <returns></returns>
    Task<bool> CheckIfExists(string email, EmployeeEvaluationDto evaluation);
}
