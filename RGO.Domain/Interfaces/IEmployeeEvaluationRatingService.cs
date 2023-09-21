using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IEmployeeEvaluationRatingService
{
    Task<EmployeeEvaluationRatingDto> SaveEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto);

    Task<EmployeeEvaluationRatingDto> DeleteEmployeeEvaluationRating(string email, string employeeEamil, string ownerEmail, string template, string subject);

    Task<EmployeeEvaluationRatingDto> GetEmployeeEvaluationRating(string email, string employeeEamil, string ownerEmail, string template, string subject);

    Task<List<EmployeeEvaluationRatingDto>> GetAllEmployeeEvaluationRatings();

    Task<List<EmployeeEvaluationRatingDto>> GetAllEmployeeEvaluationRatingsByEvaluation(string employeeEamil, string ownerEmail, string template, string subject);

    Task<List<EmployeeEvaluationRatingDto>> GetAllEmployeeEvaluationRatingsByEmployee(string email);

    Task<EmployeeEvaluationRatingDto> UpdateEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto);

    Task<bool> CheckIfExists(string email, int evaluationId);
}
