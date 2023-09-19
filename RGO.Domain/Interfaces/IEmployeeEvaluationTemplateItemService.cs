using RGO.Models;

namespace RGO.Services.Interfaces;

public interface IEmployeeEvaluationTemplateItemService
{
    /// <summary>
    /// Save Employee Evaluation Template Item
    /// </summary>
    /// <param name="employeeEvaluationTemplateItemDto"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationTemplateItemDto> SaveEmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto);

    /// <summary>
    /// Delete Employee Evaluation Template Item
    /// </summary>
    /// <param name="employeeEvaluationTemplateItemDto"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationTemplateItemDto> DeleteEmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto);

    /// <summary>
    /// Get Employee Evaluation Template Item
    /// </summary>
    /// <param name="employeeEvaluationTemplateItemDto"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationTemplateItemDto> GetEmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto);

    /// <summary>
    /// Get All Employee Evaluation Template Items
    /// </summary>
    /// <returns></returns>
    Task<List<EmployeeEvaluationTemplateItemDto>> GetAllEmployeeEvaluationTemplateItems();

    /// <summary>
    /// Get All Employee Evaluation Template Items By Template name
    /// </summary>
    /// <param name="template"></param>
    /// <returns></returns>
    Task<List<EmployeeEvaluationTemplateItemDto>> GetAllEmployeeEvaluationTemplateItemsByTemplate(string template);

    /// <summary>
    /// Get All Employee Evaluation Template Items By Section
    /// </summary>
    /// <param name="section"></param>
    /// <returns></returns>
    Task<List<EmployeeEvaluationTemplateItemDto>> GetAllEmployeeEvaluationTemplateItemsBySection(string section);

    /// <summary>
    /// Update Employee Evaluation Template Item
    /// </summary>
    /// <param name="employeeEvaluationTemplateItemDto"></param>
    /// <returns></returns>
    Task<EmployeeEvaluationTemplateItemDto> UpdateEmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto);

    /// <summary>
    /// Check if Employee Evaluation Template Item exists
    /// </summary>
    /// <param name="employeeEvaluationTemplateItemDto"></param>
    /// <returns></returns>
    Task<bool> CheckIfExists(EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto);
}

