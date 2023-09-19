using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services;

public class EmployeeEvaluationService : IEmployeeEvaluationService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeService _employeeService;
    private readonly IEmployeeEvaluationTemplateService _employeeEvaluationTemplateService;

    public EmployeeEvaluationService(
        IUnitOfWork db,
        IEmployeeService employeeService,
        IEmployeeEvaluationTemplateService employeeEvaluationTemplateService)
    {
        _db = db;
        _employeeService = employeeService;
        _employeeEvaluationTemplateService = employeeEvaluationTemplateService;
    }

    public async Task<bool> CheckIfExists(int id)
    {
        bool exists = await _db.EmployeeEvaluation.Any(x => x.Id == id);
        return exists;
    }

    public async Task<EmployeeEvaluationDto> DeleteEmployeeEvaluationById(int id)
    {
        bool exists = await CheckIfExists(id);

        if (!exists)
            throw new Exception("Employee Evaluation not found");

        EmployeeEvaluationDto deletedEmployeeEvaluation = await _db.EmployeeEvaluation.Delete(id);

        return deletedEmployeeEvaluation;
    }

    public async Task<List<EmployeeEvaluationDto>> GetAllEmployeeEvaluationByEmployee(string email)
    {
        bool exists = await _employeeService.CheckUserExist(email);

        if (!exists)
            throw new Exception($"Employee with {email} not found");

        List<EmployeeEvaluationDto> employeeEvaluations = await _db.EmployeeEvaluation
            .Get(x => x.Employee.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase))
            .AsNoTracking()
            .Include(x => x.Employee)
            .Include(x => x.Employee.EmployeeType)
            .Include(x => x.Template)
            .Include(x => x.Owner)
            .Include(x => x.Owner.EmployeeType)
            .Select(x => x.ToDto())
            .ToListAsync();

        return employeeEvaluations;
    }

    public async Task<List<EmployeeEvaluationDto>> GetAllEmployeeEvaluationByOwner(string email)
    {
        bool exists = await _employeeService.CheckUserExist(email);

        if (!exists)
            throw new Exception($"Employee with {email} not found");

        List<EmployeeEvaluationDto> employeeEvaluations = await _db.EmployeeEvaluation
            .Get(x => x.Owner.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase))
            .AsNoTracking()
            .Include(x => x.Employee)
            .Include(x => x.Employee.EmployeeType)
            .Include(x => x.Template)
            .Include(x => x.Owner)
            .Include(x => x.Owner.EmployeeType)
            .Select(x => x.ToDto())
            .ToListAsync();

        return employeeEvaluations;
    }

    public async Task<List<EmployeeEvaluationDto>> GetAllEmployeeEvaluationByTemplate(string template)
    {
        bool exists = await _employeeEvaluationTemplateService.CheckIfExists(template);

        if (!exists)
            throw new Exception($"Employee Evaluation Template {template} not found");

        List<EmployeeEvaluationDto> employeeEvaluations = await _db.EmployeeEvaluation
            .Get(x => x.Template.Description == template)
            .AsNoTracking()
            .Include(x => x.Employee)
            .Include(x => x.Employee.EmployeeType)
            .Include(x => x.Template)
            .Include(x => x.Owner)
            .Include(x => x.Owner.EmployeeType)
            .Select(x => x.ToDto())
            .ToListAsync();

        return employeeEvaluations;
    }

    public async Task<List<EmployeeEvaluationDto>> GetAllEmployeeEvaluations()
    {
        List<EmployeeEvaluationDto> employeeEvaluations = await _db.EmployeeEvaluation
            .Get()
            .AsNoTracking()
            .Include(x => x.Employee)
            .Include(x => x.Employee.EmployeeType)
            .Include(x => x.Template)
            .Include(x => x.Owner)
            .Include(x => x.Owner.EmployeeType)
            .Select(x => x.ToDto())
            .ToListAsync();

        return employeeEvaluations;
    }

    public async Task<EmployeeEvaluationDto> GetEmployeeEvaluationById(int id)
    {
        bool exists = await CheckIfExists(id);

        if (!exists)
            throw new Exception("Employee Evaluation not found");

        EmployeeEvaluation employeeEvaluation = await _db.EmployeeEvaluation
            .Get(x => x.Id == id)
            .AsNoTracking()
            .Include(x => x.Employee)
            .Include(x => x.Employee.EmployeeType)
            .Include(x => x.Template)
            .Include(x => x.Owner)
            .Include(x => x.Owner.EmployeeType)
            .FirstAsync();

        return employeeEvaluation.ToDto();
    }

    public async Task<EmployeeEvaluationDto> SaveEmployeeEvaluation(EmployeeEvaluationDto employeeEvaluationDto)
    {
        bool exists = await CheckIfExists(employeeEvaluationDto.Id);

        if (exists)
            throw new Exception("Employee Evaluation already exists");

        EmployeeEvaluation employeeEvaluation = new EmployeeEvaluation(employeeEvaluationDto);

        EmployeeEvaluationDto savedEmployeeEvaluation = await _db.EmployeeEvaluation.Add(employeeEvaluation);

        return savedEmployeeEvaluation;
    }

    public async Task<EmployeeEvaluationDto> UpdateEmployeeEvaluation(EmployeeEvaluationDto employeeEvaluationDto)
    {
        bool exists = await CheckIfExists(employeeEvaluationDto.Id);

        if (!exists)
            throw new Exception("Employee Evaluation not found");

        EmployeeEvaluation employeeEvaluation = new EmployeeEvaluation(employeeEvaluationDto);

        EmployeeEvaluationDto updatedEmployeeEvaluation = await _db.EmployeeEvaluation.Update(employeeEvaluation);

        return updatedEmployeeEvaluation;
    }
}