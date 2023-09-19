using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services;

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

    public async Task<bool> CheckIfExists(EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto)
    {
        bool exists = await _db.EmployeeEvaluationTemplateItem
            .Any(x => x.TemplateId == employeeEvaluationTemplateItemDto.Template!.Id
                && x.Section.Equals(employeeEvaluationTemplateItemDto.Section, StringComparison.CurrentCultureIgnoreCase)
                && x.Question.Equals(employeeEvaluationTemplateItemDto.Question, StringComparison.CurrentCultureIgnoreCase));

        return exists;
    }

    public async Task<EmployeeEvaluationTemplateItemDto> DeleteEmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto)
    {
        bool exists = await CheckIfExists(employeeEvaluationTemplateItemDto);

        if (!exists)
            throw new Exception($"Employee Evaluation Template Item not found");

        var deletedEmployeeEvaluationTemplateItem = await _db.EmployeeEvaluationTemplateItem
            .Delete(employeeEvaluationTemplateItemDto.Id);

        return deletedEmployeeEvaluationTemplateItem;
    }

    public async Task<List<EmployeeEvaluationTemplateItemDto>> GetAllEmployeeEvaluationTemplateItems()
    {
        List<EmployeeEvaluationTemplateItemDto> employeeEvaluationTemplateItems = await _db.EmployeeEvaluationTemplateItem
            .Get()
            .AsNoTracking()
            .Include(x => x.Template)
            .Select(x => x.ToDto())
            .ToListAsync();

        return employeeEvaluationTemplateItems;
    }

    public async Task<List<EmployeeEvaluationTemplateItemDto>> GetAllEmployeeEvaluationTemplateItemsBySection(string section)
    {
        List<EmployeeEvaluationTemplateItemDto> employeeEvaluationTemplateItems = await _db.EmployeeEvaluationTemplateItem
            .Get(x => x.Section.Equals(section, StringComparison.CurrentCultureIgnoreCase))
            .AsNoTracking()
            .Include(x => x.Template)
            .Select(x => x.ToDto())
            .ToListAsync();

        return employeeEvaluationTemplateItems;
    }

    public async Task<List<EmployeeEvaluationTemplateItemDto>> GetAllEmployeeEvaluationTemplateItemsByTemplate(string template)
    {
        bool exists = await _employeeEvaluationTemplateService.CheckIfExists(template);

        if (!exists)
            throw new Exception($"Employee Evaluation Template {template} not found");

        List<EmployeeEvaluationTemplateItemDto> employeeEvaluationTemplateItems = await _db.EmployeeEvaluationTemplateItem
            .Get(x => x.Template.Description.Equals(template, StringComparison.CurrentCultureIgnoreCase))
            .AsNoTracking()
            .Include(x => x.Template)
            .Select(x => x.ToDto())
            .ToListAsync();

        return employeeEvaluationTemplateItems;
    }

    public async Task<EmployeeEvaluationTemplateItemDto> GetEmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto)
    {
        bool exists = await CheckIfExists(employeeEvaluationTemplateItemDto);

        if (!exists)
            throw new Exception($"Employee Evaluation Template Item not found");

        EmployeeEvaluationTemplateItem employeeEvaluationTemplateItem = await _db.EmployeeEvaluationTemplateItem
            .Get(x => x.TemplateId == employeeEvaluationTemplateItemDto.Template!.Id
                && x.Section.Equals(employeeEvaluationTemplateItemDto.Section, StringComparison.CurrentCultureIgnoreCase)
                && x.Question.Equals(employeeEvaluationTemplateItemDto.Question, StringComparison.CurrentCultureIgnoreCase))
            .AsNoTracking()
            .Include(x => x.Template)
            .FirstAsync();

        return employeeEvaluationTemplateItem.ToDto();
    }

    public async Task<EmployeeEvaluationTemplateItemDto> SaveEmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto)
    {
        bool exists = await CheckIfExists(employeeEvaluationTemplateItemDto);

        if (exists)
            throw new Exception($"Employee Evaluation Template Item already exists");

        var savedEmployeeEvaluationTemplateItem = await _db.EmployeeEvaluationTemplateItem
            .Add(new EmployeeEvaluationTemplateItem(employeeEvaluationTemplateItemDto));

        return savedEmployeeEvaluationTemplateItem;
    }

    public async Task<EmployeeEvaluationTemplateItemDto> UpdateEmployeeEvaluationTemplateItem(EmployeeEvaluationTemplateItemDto employeeEvaluationTemplateItemDto)
    {
        bool exists = await CheckIfExists(employeeEvaluationTemplateItemDto);

        if (!exists)
            throw new Exception($"Employee Evaluation Template Item not found");

        var updatedEmployeeEvaluationTemplateItem = await _db.EmployeeEvaluationTemplateItem
            .Update(new EmployeeEvaluationTemplateItem(employeeEvaluationTemplateItemDto));

        return updatedEmployeeEvaluationTemplateItem;
    }
}
