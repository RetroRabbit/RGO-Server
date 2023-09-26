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

    public async Task<bool> CheckIfExists(EmployeeEvaluationInput evaluationInput)
    {
        bool exists = await _db.EmployeeEvaluation.Any(x => x.Employee.Email == evaluationInput.EmployeeEmail
            && x.Owner.Email == evaluationInput.OwnerEmail
            && x.Template.Description == evaluationInput.Template
            && x.Subject == evaluationInput.Subject);

        return exists;
    }

    public async Task<EmployeeEvaluationDto> Delete(EmployeeEvaluationInput evaluationInput)
    {
        bool exists = await CheckIfExists(evaluationInput);

        if (!exists)
            throw new Exception("Employee Evaluation not found");

        EmployeeEvaluationDto evaluation = await Get(
            evaluationInput.EmployeeEmail,
            evaluationInput.OwnerEmail,
            evaluationInput.Template,
            evaluationInput.Subject);

        EmployeeEvaluationDto deletedEmployeeEvaluation = await _db.EmployeeEvaluation.Delete(evaluation.Id);

        return deletedEmployeeEvaluation;
    }

    public async Task<List<EmployeeEvaluationDto>> GetAllByEmployee(string email)
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

    public async Task<List<EmployeeEvaluationDto>> GetAllByOwner(string email)
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

    public async Task<List<EmployeeEvaluationDto>> GetAllByTemplate(string template)
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

    public async Task<List<EmployeeEvaluationDto>> GetAllEvaluationsByEmail(string email)
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

    public async Task<EmployeeEvaluationDto> Get(
        string employeeEamil,
        string ownerEmail,
        string template,
        string subject)
    {
        EmployeeEvaluationInput evaluationInput = new EmployeeEvaluationInput(0, ownerEmail, employeeEamil, template, subject);

        bool exists = await CheckIfExists(evaluationInput);

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

    public async Task<EmployeeEvaluationDto> Save(EmployeeEvaluationInput evaluationInput)
    {
        bool exists = await CheckIfExists(evaluationInput);

        if (exists)
            throw new Exception("Employee Evaluation already exists");

        EmployeeDto employeeDto = await _employeeService.GetEmployee(evaluationInput.EmployeeEmail);
        EmployeeDto ownerDto = await _employeeService.GetEmployee(evaluationInput.OwnerEmail);
        EmployeeEvaluationTemplateDto templateDto = await _employeeEvaluationTemplateService.Get(evaluationInput.Template);

        EmployeeEvaluationDto employeeEvaluationDto = new EmployeeEvaluationDto(
            0,
            employeeDto,
            templateDto,
            ownerDto,
            evaluationInput.Subject,
            DateOnly.FromDateTime(DateTime.Now),
            null);

        EmployeeEvaluationDto savedEmployeeEvaluation = await _db.EmployeeEvaluation.Add(new EmployeeEvaluation(employeeEvaluationDto));

        return savedEmployeeEvaluation;
    }

    public async Task<EmployeeEvaluationDto> Update(
        EmployeeEvaluationInput oldEvaluation,
        EmployeeEvaluationInput newEvaluation)
    {
        bool oldExists = await CheckIfExists(oldEvaluation);

        if (!oldExists)
            throw new Exception("Employee Evaluation not found");

        EmployeeEvaluationDto employeeEvaluation = await Get(
            oldEvaluation.EmployeeEmail,
            oldEvaluation.OwnerEmail,
            oldEvaluation.Template,
            oldEvaluation.Subject);

        EmployeeDto employeeDto = await _employeeService.GetEmployee(newEvaluation.EmployeeEmail);
        EmployeeDto ownerDto = await _employeeService.GetEmployee(newEvaluation.OwnerEmail);
        EmployeeEvaluationTemplateDto templateDto = await _employeeEvaluationTemplateService.Get(newEvaluation.Template);

        EmployeeEvaluationDto newEmployeeEvauation = new EmployeeEvaluationDto(
            employeeEvaluation.Id,
            employeeDto,
            templateDto,
            ownerDto,
            newEvaluation.Subject,
            DateOnly.FromDateTime(DateTime.Now),
            null);

        EmployeeEvaluationDto updatedEmployeeEvaluation = await _db.EmployeeEvaluation.Update(new (newEmployeeEvauation));

        return updatedEmployeeEvaluation;
    }
}