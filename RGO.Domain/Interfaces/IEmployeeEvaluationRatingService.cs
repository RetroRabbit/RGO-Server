﻿using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IEmployeeEvaluationRatingService
{
    Task<EmployeeEvaluationRatingDto> SaveEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto);

    Task<EmployeeEvaluationRatingDto> DeleteEmployeeEvaluationRating(string email, EmployeeEvaluationInput evaluationInput);

    Task<EmployeeEvaluationRatingDto> GetEmployeeEvaluationRating(string email, EmployeeEvaluationInput evaluationInput);

    Task<List<EmployeeEvaluationRatingDto>> GetAllEmployeeEvaluationRatings();

    Task<List<EmployeeEvaluationRatingDto>> GetAllEmployeeEvaluationRatingsByEvaluation(EmployeeEvaluationInput evaluationInput);

    Task<List<EmployeeEvaluationRatingDto>> GetAllEmployeeEvaluationRatingsByEmployee(string email);

    Task<EmployeeEvaluationRatingDto> UpdateEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto);

    Task<bool> CheckIfExists(string email, int evaluationId);
}
