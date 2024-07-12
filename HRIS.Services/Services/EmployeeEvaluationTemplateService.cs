using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeEvaluationTemplateService : IEmployeeEvaluationTemplateService
{
    private readonly IUnitOfWork _db;

    public EmployeeEvaluationTemplateService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<bool> CheckIfExists(string template)
    {
        var exists = await _db.EmployeeEvaluationTemplate
                              .Any(x => x.Description == template);

        return exists;
    }

    public async Task<EmployeeEvaluationTemplateDto> Delete(string template)
    {
        var exists = await CheckIfExists(template);

        if (!exists)
            throw new CustomException("Employee Evaluation Template not found");

        var employeeEvaluationTemplate = await Get(template);

        var deletedEmployeeEvaluationTemplate = await _db.EmployeeEvaluationTemplate
                                                         .Delete(employeeEvaluationTemplate.Id);

        return deletedEmployeeEvaluationTemplate.ToDto();
    }

    public async Task<List<EmployeeEvaluationTemplateDto>> GetAll()
    {
        var employeeEvaluationTemplates = await _db.EmployeeEvaluationTemplate
                                                   .GetAll();

        return employeeEvaluationTemplates.Select(x => x.ToDto()).ToList();
    }

    public async Task<EmployeeEvaluationTemplateDto> Get(string template)
    {
        var exists = await CheckIfExists(template);

        if (!exists)
            throw new CustomException($"Employee Evaluation Template {template} not found");

        var employeeEvaluationTemplate = await _db.EmployeeEvaluationTemplate
                                                  .Get(x => x.Description == template)
                                                  .AsNoTracking()
                                                  .FirstAsync();

        return employeeEvaluationTemplate.ToDto();
    }

    public async Task<EmployeeEvaluationTemplateDto> Save(string template)
    {
        var exists = await CheckIfExists(template);

        if (exists)
            throw new CustomException("Employee Evaluation Template already exists");

        EmployeeEvaluationTemplate employeeEvaluationTemplate = new()
        {
            Id = 0,
            Description = template
        };

        var savedEmployeeEvaluationTemplate = await _db.EmployeeEvaluationTemplate
                                                       .Add(employeeEvaluationTemplate);

        return savedEmployeeEvaluationTemplate.ToDto();
    }

    public async Task<EmployeeEvaluationTemplateDto> Update(EmployeeEvaluationTemplateDto employeeEvaluationTemplateDto)
    {
        var exists = await CheckIfExists(employeeEvaluationTemplateDto.Description);

        if (!exists)
            throw new CustomException("Employee Evaluation Template not found");

        var newEmployeeEvaluationTemplate = await _db.EmployeeEvaluationTemplate
                                                     .Update(new
                                                                 EmployeeEvaluationTemplate(employeeEvaluationTemplateDto));

        return newEmployeeEvaluationTemplate.ToDto();
    }
}