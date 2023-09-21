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

    public async Task<bool> CheckIfExists(string employeeEamil, string ownerEmail, string template, string subject)
    {
        bool exists = await _db.EmployeeEvaluation.Any(x => x.Employee.Email == employeeEamil
            && x.Owner.Email == ownerEmail
            && x.Template.Description == template
            && x.Subject == subject);

        return exists;
    }

    public async Task<EmployeeEvaluationDto> DeleteEmployeeEvaluation(string employeeEamil, string ownerEmail, string template, string subject)
    {
        bool exists = await CheckIfExists(employeeEamil, ownerEmail, template, subject);

        if (!exists)
            throw new Exception("Employee Evaluation not found");

        EmployeeEvaluationDto evaluation = await GetEmployeeEvaluation(employeeEamil, ownerEmail, template, subject);

        EmployeeEvaluationDto deletedEmployeeEvaluation = await _db.EmployeeEvaluation.Delete(evaluation.Id);

        return deletedEmployeeEvaluation;
    }

    public async Task<List<EmployeeEvaluationDto>> GetAllEmployeeEvaluationByEmployee(string email)
    {
        bool exists = await _employeeService.CheckUserExist(email);

        if (!exists)
            throw new Exception($"Employee with {email} not found");

        List<EmployeeEvaluationDto> employeeEvaluations = await _db.EmployeeEvaluation
            .Get(x => x.Employee.Email == email)
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
            .Get(x => x.Owner.Email == email)
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

    public async Task<List<EmployeeEvaluationDto>> GetAllEmployeeEvaluations(string email)
    {
        List<EmployeeEvaluationDto> employeeEvaluations = await _db.EmployeeEvaluation
            .Get(x => x.Owner.Email == email
                || x.Employee.Email == email)
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

    public async Task<EmployeeEvaluationDto> GetEmployeeEvaluation(string employeeEamil, string ownerEmail, string template, string subject)
    {
        bool exists = await CheckIfExists(employeeEamil, ownerEmail, template, subject);

        if (!exists) throw new Exception("Employee Evaluation not found");

        EmployeeEvaluation employeeEvaluation = await _db.EmployeeEvaluation
            .Get(x => x.Employee.Email == employeeEamil
                && x.Owner.Email == ownerEmail
                && x.Template.Description == template
                && x.Subject == subject)
            .AsNoTracking()
            .Include(x => x.Employee)
            .Include(x => x.Employee.EmployeeType)
            .Include(x => x.Template)
            .Include(x => x.Owner)
            .Include(x => x.Owner.EmployeeType)
            .FirstAsync();

        return employeeEvaluation.ToDto();
    }

    public async Task<EmployeeEvaluationDto> SaveEmployeeEvaluation(string employeeEmail, string ownerEmail, string template, string subject)
    {
        bool exists = await CheckIfExists(employeeEmail, ownerEmail, template, subject);

        if (exists)
            throw new Exception("Employee Evaluation already exists");

        EmployeeDto employeeDto = await _employeeService.GetEmployee(employeeEmail);
        EmployeeDto ownerDto = await _employeeService.GetEmployee(ownerEmail);
        EmployeeEvaluationTemplateDto templateDto = await _employeeEvaluationTemplateService.GetEmployeeEvaluationTemplate(template);

        EmployeeEvaluationDto employeeEvaluationDto = new EmployeeEvaluationDto(0, employeeDto, templateDto, ownerDto, subject, DateOnly.FromDateTime(DateTime.Now), null);

        EmployeeEvaluationDto savedEmployeeEvaluation = await _db.EmployeeEvaluation.Add(new EmployeeEvaluation(employeeEvaluationDto));

        return savedEmployeeEvaluation;
    }

    public async Task<EmployeeEvaluationDto> UpdateEmployeeEvaluation(EmployeeEvaluationDto employeeEvaluationDto)
    {
        bool exists = await CheckIfExists(employeeEvaluationDto.Employee!.Email, employeeEvaluationDto.Owner!.Email, employeeEvaluationDto.Template!.Description, employeeEvaluationDto.Subject);

        if (!exists)
            throw new Exception("Employee Evaluation not found");

        EmployeeEvaluation employeeEvaluation = new EmployeeEvaluation(employeeEvaluationDto);

        EmployeeEvaluationDto updatedEmployeeEvaluation = await _db.EmployeeEvaluation.Update(employeeEvaluation);

        return updatedEmployeeEvaluation;
    }
}