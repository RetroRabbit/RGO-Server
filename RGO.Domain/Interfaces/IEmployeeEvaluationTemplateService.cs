using RGO.Models;
namespace RGO.Services.Interfaces;

public interface IEmployeeEvaluationTemplateService
{
    /// <summary>
    /// Save Employee Evaluation Template
    /// </summary>
    /// <param name="employeeEvaluationTemplateDto"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationTemplateDto> SaveEmployeeEvaluationTemplate(EmployeeEvaluationTemplateDto employeeEvaluationTemplateDto);

    /// <summary>
    /// Delete Employee Evaluation Template
    /// </summary>
    /// <param name="employeeEvaluationTemplateDto"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationTemplateDto> DeleteEmployeeEvaluationTemplate(EmployeeEvaluationTemplateDto employeeEvaluationTemplateDto);

    /// <summary>
    /// Get Employee Evaluation Template
    /// </summary>
    /// <param name="employeeEvaluationTemplateDto"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationTemplateDto> GetEmployeeEvaluationTemplate(EmployeeEvaluationTemplateDto employeeEvaluationTemplateDto);

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
}
