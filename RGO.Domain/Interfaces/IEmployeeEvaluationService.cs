using Microsoft.AspNetCore.Mvc;
using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IEmployeeEvaluationService
{
    Task<EmployeeEvaluationDto> SaveEmployeeEvaluation(EmployeeEvaluationInput evaluationInput);

    Task<EmployeeEvaluationDto> DeleteEmployeeEvaluation(EmployeeEvaluationInput evaluationInput);

    Task<EmployeeEvaluationDto> GetEmployeeEvaluation(string employeeEamil, string ownerEmail, string template, string subject);

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
    Task<EmployeeEvaluationDto> UpdateEmployeeEvaluation(EmployeeEvaluationInput oldEvaluation, EmployeeEvaluationInput newEvaluation);

    Task<bool> CheckIfExists(EmployeeEvaluationInput evaluationInput);
}
