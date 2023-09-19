using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services;

public class EmployeeEvaluationTemplateService : IEmployeeEvaluationTemplateService
{
    private readonly IUnitOfWork _db;
    public EmployeeEvaluationTemplateService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<bool> CheckIfExists(string template)
    {
        bool exists = await _db.EmployeeEvaluationTemplate
            .Any(x => x.Description.Equals(template, StringComparison.CurrentCultureIgnoreCase));

        return exists;
    }

    public async Task<EmployeeEvaluationTemplateDto> DeleteEmployeeEvaluationTemplate(string template)
    {
        bool exists = await CheckIfExists(template);

        if (!exists) throw new Exception("Employee Evaluation Template not found");

        EmployeeEvaluationTemplateDto employeeEvaluationTemplate = await GetEmployeeEvaluationTemplate(template);

        EmployeeEvaluationTemplateDto deletedEmployeeEvaluationTemplate = await _db.EmployeeEvaluationTemplate
            .Delete(employeeEvaluationTemplate.Id);

        return deletedEmployeeEvaluationTemplate;
    }

    public async Task<List<EmployeeEvaluationTemplateDto>> GetAllEmployeeEvaluationTemplates()
    {
        var employeeEvaluationTemplates = await _db.EmployeeEvaluationTemplate
            .GetAll();

        return employeeEvaluationTemplates;
    }

    public async Task<EmployeeEvaluationTemplateDto> GetEmployeeEvaluationTemplate(string template)
    {
        bool exists = await CheckIfExists(template);

        if (!exists)
            throw new Exception($"Employee Evaluation Template {template} not found");

        EmployeeEvaluationTemplate employeeEvaluationTemplate = await _db.EmployeeEvaluationTemplate
            .Get(x => x.Description.Equals(template, StringComparison.CurrentCultureIgnoreCase))
            .AsNoTracking()
            .FirstAsync();

        return employeeEvaluationTemplate.ToDto();
    }

    public async Task<EmployeeEvaluationTemplateDto> SaveEmployeeEvaluationTemplate(string template)
    {
        bool exists = await CheckIfExists(template);

        if (exists) throw new Exception("Employee Evaluation Template already exists");

        EmployeeEvaluationTemplate employeeEvaluationTemplate = new()
        {
            Id = 0,
            Description = template
        };

        EmployeeEvaluationTemplateDto savedEmployeeEvaluationTemplate = await _db.EmployeeEvaluationTemplate
            .Add(employeeEvaluationTemplate);

        return savedEmployeeEvaluationTemplate;
    }

    public async Task<EmployeeEvaluationTemplateDto> UpdateEmployeeEvaluationTemplate(EmployeeEvaluationTemplateDto employeeEvaluationTemplateDto)
    {
        bool exists = await CheckIfExists(employeeEvaluationTemplateDto.Description);

        if (!exists) throw new Exception("Employee Evaluation Template not found");

        EmployeeEvaluationTemplateDto newEmployeeEvaluationTemplate = await _db.EmployeeEvaluationTemplate
            .Update(new EmployeeEvaluationTemplate(employeeEvaluationTemplateDto));

        return newEmployeeEvaluationTemplate;
    }
}
