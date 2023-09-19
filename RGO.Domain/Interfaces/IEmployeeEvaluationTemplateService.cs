using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IEmployeeEvaluationTemplateService
{
    /// <summary>
    /// Save Employee Evaluation Template
    /// </summary>
    /// <param name="employeeEvaluationTemplateDto"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationTemplateDto> SaveEmployeeEvaluationTemplate(string template);

    /// <summary>
    /// Delete Employee Evaluation Template
    /// </summary>
    /// <param name="employeeEvaluationTemplateDto"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationTemplateDto> DeleteEmployeeEvaluationTemplate(string template);

    /// <summary>
    /// Get Employee Evaluation Template
    /// </summary>
    /// <param name="employeeEvaluationTemplateDto"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationTemplateDto> GetEmployeeEvaluationTemplate(string template);

    /// <summary>
    /// Get All Employee Evaluation Templates
    /// </summary>
    /// <returns></returns>
    Task<List<EmployeeEvaluationTemplateDto>> GetAllEmployeeEvaluationTemplates();

    /// <summary>
    /// Update Employee Evaluation Template
    /// </summary>
    /// <param name="employeeEvaluationTemplateDto"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationTemplateDto> UpdateEmployeeEvaluationTemplate(EmployeeEvaluationTemplateDto employeeEvaluationTemplateDto);

    /// <summary>
    /// Check if Employee Evaluation Template exists
    /// </summary>
    /// <param name="template"></param>
    /// <returns></returns>
    Task<bool> CheckIfExists(string template);
}
