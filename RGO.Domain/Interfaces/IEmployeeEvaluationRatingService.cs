using RGO.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RGO.Services.Interfaces
{
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
        Task<EmployeeEvaluationRatingDto> DeleteEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto);

        /// <summary>
        /// Get Employee Evaluation Rating
        /// </summary>
        /// <param name="employeeEvaluationRatingDto"></param>
        /// <returns></returns>
        Task<EmployeeEvaluationRatingDto> GetEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto);

        /// <summary>
        /// Get All Employee Evaluation Ratings
        /// </summary>
        /// <returns></returns>
        Task<List<EmployeeEvaluationRatingDto>> GetAllEmployeeEvaluationRatings();

        /// <summary>
        /// Update Employee Evaluation Rating
        /// </summary>
        /// <param name="employeeEvaluationRatingDto"></param>
        /// <returns></returns>
        Task<EmployeeEvaluationRatingDto> UpdateEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto);
    }
}
