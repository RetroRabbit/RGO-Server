using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IEmployeeEvaluationTemplateItemService
{
    /// <summary>
    ///     Save Employee Evaluation Template Item
    /// </summary>
    /// <param name="template"></param>
    /// <param name="section"></param>
    /// <param name="question"></param>
    /// <returns>Employee Evaluation Template Item</returns>
    Task<EmployeeEvaluationTemplateItemDto> Save(string template, string section, string question);

    /// <summary>
    ///     Delete Employee Evaluation Template Item
    /// </summary>
    /// <param name="template"></param>
    /// <param name="section"></param>
    /// <param name="question"></param>
    /// <returns>Employee Evaluation Template Item</returns>
    Task<EmployeeEvaluationTemplateItemDto> Delete(string template, string section, string question);

    /// <summary>
    ///     Get Employee Evaluation Template Item
    /// </summary>
    /// <param name="template"></param>
    /// <param name="section"></param>
    /// <param name="question"></param>
    /// <returns>Employee Evaluation Template Item</returns>
    Task<EmployeeEvaluationTemplateItemDto> Get(string template, string section, string question);

    /// <summary>
    ///     Get All Employee Evaluation Template Items
    /// </summary>
    /// <returns>List of Employee Evaluation Template Items</returns>
    Task<List<EmployeeEvaluationTemplateItemDto>> GetAll();

    /// <summary>
    ///     Get All Employee Evaluation Template Items By Template name
    /// </summary>
    /// <param name="template"></param>
    /// <returns>List of Employee Evaluation Template Items</returns>
    Task<List<EmployeeEvaluationTemplateItemDto>> GetAllByTemplate(string template);

    /// <summary>
    ///     Get All Employee Evaluation Template Items By Section
    /// </summary>
    /// <param name="section"></param>
    /// <returns>List of Employee Evaluation Template Items</returns>
    Task<List<EmployeeEvaluationTemplateItemDto>> GetAllBySection(string section);

    /// <summary>
    ///     Update Employee Evaluation Template Item
    /// </summary>
    /// <param name="employeeEvaluationTemplateItemDto"></param>
    /// <returns>Employee Evaluation Template Item</returns>
    Task<EmployeeEvaluationTemplateItemDto> Update(EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto);

    /// <summary>
    ///     Check if Employee Evaluation Template Item exists
    /// </summary>
    /// <param name="template"></param>
    /// <param name="section"></param>
    /// <param name="question"></param>
    /// <returns>True or False</returns>
    Task<bool> CheckIfExists(string template, string section, string question);
}