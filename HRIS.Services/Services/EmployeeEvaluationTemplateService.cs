using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeEvaluationTemplateService : IEmployeeEvaluationTemplateService
{
    private readonly IUnitOfWork _db;
    private readonly IErrorLoggingService _errorLoggingService;

    public EmployeeEvaluationTemplateService(IUnitOfWork db, IErrorLoggingService errorLoggingService)
    {
        _db = db;
        _errorLoggingService = errorLoggingService;
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
        {
            var exception = new Exception("Employee Evaluation Template not found");
            throw _errorLoggingService.LogException(exception);
        }

        var employeeEvaluationTemplate = await Get(template);

        var deletedEmployeeEvaluationTemplate = await _db.EmployeeEvaluationTemplate
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
        var exists = await CheckIfExists(template);

        if (!exists)
        {
            var exception = new Exception($"Employee Evaluation Template {template} not found");
            throw _errorLoggingService.LogException(exception);
        }

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
        {
            var exception = new Exception("Employee Evaluation Template already exists");
            throw _errorLoggingService.LogException(exception);
        }

        EmployeeEvaluationTemplate employeeEvaluationTemplate = new()
        {
            Id = 0,
            Description = template
        };

        var savedEmployeeEvaluationTemplate = await _db.EmployeeEvaluationTemplate
                                                       .Add(employeeEvaluationTemplate);

        return savedEmployeeEvaluationTemplate;
    }

    public async Task<EmployeeEvaluationTemplateDto> Update(EmployeeEvaluationTemplateDto employeeEvaluationTemplateDto)
    {
        var exists = await CheckIfExists(employeeEvaluationTemplateDto.Description);

        if (!exists)
        {
            var exception = new Exception("Employee Evaluation Template not found");
            throw _errorLoggingService.LogException(exception);
        }

        var newEmployeeEvaluationTemplate = await _db.EmployeeEvaluationTemplate
                                                     .Update(new
                                                                 EmployeeEvaluationTemplate(employeeEvaluationTemplateDto));

        return newEmployeeEvaluationTemplate;
    }
}