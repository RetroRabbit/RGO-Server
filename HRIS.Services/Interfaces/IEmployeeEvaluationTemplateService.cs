using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IEmployeeEvaluationTemplateService
{
    /// <summary>
    ///     Save Employee Evaluation Template
    /// </summary>
    /// <param name="employeeEvaluationTemplateDto"></param>
    /// <returns>Employee Evaluation Template</returns>
    Task<EmployeeEvaluationTemplateDto> Save(string template);

    /// <summary>
    ///     Delete Employee Evaluation Template
    /// </summary>
    /// <param name="employeeEvaluationTemplateDto"></param>
    /// <returns>Employee Evaluation Template</returns>
    Task<EmployeeEvaluationTemplateDto> Delete(string template);

    /// <summary>
    ///     Get Employee Evaluation Template
    /// </summary>
    /// <param name="employeeEvaluationTemplateDto"></param>
    /// <returns>Employee Evaluation Template</returns>
    Task<EmployeeEvaluationTemplateDto> Get(string template);

    /// <summary>
    ///     Get All Employee Evaluation Templates
    /// </summary>
    /// <returns>List of Employee Evaluation Templates</returns>
    Task<List<EmployeeEvaluationTemplateDto>> GetAll();

    /// <summary>
    ///     Update Employee Evaluation Template
    /// </summary>
    /// <param name="employeeEvaluationTemplateDto"></param>
    /// <returns>Employee Evaluation Template</returns>
    Task<EmployeeEvaluationTemplateDto> Update(EmployeeEvaluationTemplateDto employeeEvaluationTemplateDto);

    /// <summary>
    ///     Check if Employee Evaluation Template exists
    /// </summary>
    /// <param name="template"></param>
    /// <returns>True or False</returns>
    Task<bool> CheckIfExists(string template);
}