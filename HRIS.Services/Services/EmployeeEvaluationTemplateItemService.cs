using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeEvaluationTemplateItemService : IEmployeeEvaluationTemplateItemService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeEvaluationTemplateService _employeeEvaluationTemplateService;

    public EmployeeEvaluationTemplateItemService(
        IUnitOfWork db,
        IEmployeeEvaluationTemplateService employeeEvaluationTemplateService)
    {
        _db = db;
        _employeeEvaluationTemplateService = employeeEvaluationTemplateService;
    }

    public async Task<bool> CheckIfExists(string template, string section, string question)
    {
        var exists = await _db.EmployeeEvaluationTemplateItem
                              .Any(x => x.Template.Description == template
                                        && x.Section == section
                                        && x.Question == question);

        return exists;
    }

    public async Task<EmployeeEvaluationTemplateItemDto> Delete(string template, string section, string question)
    {
        var exists = await CheckIfExists(template, section, question);

        if (!exists)
            throw new Exception("Employee Evaluation Template Item not found");

        var employeeEvaluationTemplateItemDto = await Get(template, section, question);

        var deletedEmployeeEvaluationTemplateItem = await _db.EmployeeEvaluationTemplateItem
                                                             .Delete(employeeEvaluationTemplateItemDto.Id);

        return deletedEmployeeEvaluationTemplateItem;
    }

    public async Task<List<EmployeeEvaluationTemplateItemDto>> GetAll()
    {
        var employeeEvaluationTemplateItems = await _db.EmployeeEvaluationTemplateItem
                                                       .Get()
                                                       .AsNoTracking()
                                                       .Include(x => x.Template)
                                                       .Select(x => x.ToDto())
                                                       .ToListAsync();

        return employeeEvaluationTemplateItems;
    }

    public async Task<List<EmployeeEvaluationTemplateItemDto>> GetAllBySection(string section)
    {
        var employeeEvaluationTemplateItems = await _db.EmployeeEvaluationTemplateItem
                                                       .Get(x => x.Section == section)
                                                       .AsNoTracking()
                                                       .Include(x => x.Template)
                                                       .Select(x => x.ToDto())
                                                       .ToListAsync();

        return employeeEvaluationTemplateItems;
    }

    public async Task<List<EmployeeEvaluationTemplateItemDto>> GetAllByTemplate(string template)
    {
        var exists = await _employeeEvaluationTemplateService.CheckIfExists(template);

        if (!exists)
            throw new Exception($"Employee Evaluation Template {template} not found");

        var employeeEvaluationTemplateItems = await _db.EmployeeEvaluationTemplateItem
                                                       .Get(x => x.Template.Description == template)
                                                       .AsNoTracking()
                                                       .Include(x => x.Template)
                                                       .Select(x => x.ToDto())
                                                       .ToListAsync();

        return employeeEvaluationTemplateItems;
    }

    public async Task<EmployeeEvaluationTemplateItemDto> Get(string template, string section, string question)
    {
        var exists = await CheckIfExists(template, section, question);

        if (!exists)
            throw new Exception("Employee Evaluation Template Item not found");

        var employeeEvaluationTemplateItem = await _db.EmployeeEvaluationTemplateItem
                                                      .Get(x => x.Template.Description == template
                                                                && x.Section == section
                                                                && x.Question == question)
                                                      .AsNoTracking()
                                                      .Include(x => x.Template)
                                                      .FirstAsync();

        return employeeEvaluationTemplateItem.ToDto();
    }

    public async Task<EmployeeEvaluationTemplateItemDto> Save(string template, string section, string question)
    {
        var exists = await CheckIfExists(template, section, question);

        if (exists)
            throw new Exception("Employee Evaluation Template Item already exists");

        var employeeEvaluationTemplateItemDto = new EmployeeEvaluationTemplateItemDto{
         Id = 0,
         Template = await _employeeEvaluationTemplateService.Get(template),
         Section = section,
         Question = question
         };

        var savedEmployeeEvaluationTemplateItem = await _db.EmployeeEvaluationTemplateItem
                                                           .Add(new
                                                                    EmployeeEvaluationTemplateItem(employeeEvaluationTemplateItemDto));

        return savedEmployeeEvaluationTemplateItem;
    }

    public async Task<EmployeeEvaluationTemplateItemDto> Update(
        EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto)
    {
        var exists = await CheckIfExists(
                                         employeeEvaluationTemplateItemDto.Template!.Description,
                                         employeeEvaluationTemplateItemDto.Section,
                                         employeeEvaluationTemplateItemDto.Question);

        if (!exists)
            throw new Exception("Employee Evaluation Template Item not found");

        var updatedEmployeeEvaluationTemplateItem = await _db.EmployeeEvaluationTemplateItem
                                                             .Update(new
                                                                         EmployeeEvaluationTemplateItem(employeeEvaluationTemplateItemDto));

        return updatedEmployeeEvaluationTemplateItem;
    }
}