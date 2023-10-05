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
            .Any(x => x.Description == template);

        return exists;
    }

    public async Task<EmployeeEvaluationTemplateDto> Delete(string template)
    {
        bool exists = await CheckIfExists(template);

        if (!exists) throw new Exception("Employee Evaluation Template not found");

        EmployeeEvaluationTemplateDto employeeEvaluationTemplate = await Get(template);

        EmployeeEvaluationTemplateDto deletedEmployeeEvaluationTemplate = await _db.EmployeeEvaluationTemplate
            .Delete(employeeEvaluationTemplate.Id);

        return deletedEmployeeEvaluationTemplate;
    }

    public async Task<List<EmployeeEvaluationTemplateDto>> GetAll()
    {
        var employeeEvaluationTemplates = await _db.EmployeeEvaluationTemplate
            .GetAll();

        return employeeEvaluationTemplates;
    }

    public async Task<EmployeeEvaluationTemplateDto> Get(string template)
    {
        bool exists = await CheckIfExists(template);

        if (!exists)
            throw new Exception($"Employee Evaluation Template {template} not found");

        EmployeeEvaluationTemplate employeeEvaluationTemplate = await _db.EmployeeEvaluationTemplate
            .Get(x => x.Description == template)
            .AsNoTracking()
            .FirstAsync();

        return employeeEvaluationTemplate.ToDto();
    }

    public async Task<EmployeeEvaluationTemplateDto> Save(string template)
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

    public async Task<EmployeeEvaluationTemplateDto> Update(EmployeeEvaluationTemplateDto employeeEvaluationTemplateDto)
    {
        bool exists = await CheckIfExists(employeeEvaluationTemplateDto.Description);

        if (!exists) throw new Exception("Employee Evaluation Template not found");

        EmployeeEvaluationTemplateDto newEmployeeEvaluationTemplate = await _db.EmployeeEvaluationTemplate
            .Update(new EmployeeEvaluationTemplate(employeeEvaluationTemplateDto));

        return newEmployeeEvaluationTemplate;
    }
}
