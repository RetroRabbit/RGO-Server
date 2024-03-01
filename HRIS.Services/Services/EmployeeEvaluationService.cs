using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeEvaluationService : IEmployeeEvaluationService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeEvaluationTemplateService _employeeEvaluationTemplateService;
    private readonly IEmployeeService _employeeService;

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
        var exists = await _db.EmployeeEvaluation
                              .Any(evaluation =>
                                       evaluation.Employee.Email == evaluationInput.EmployeeEmail &&
                                       evaluation.Owner.Email == evaluationInput.OwnerEmail &&
                                       evaluation.Template.Description == evaluationInput.Template &&
                                       evaluation.Subject == evaluationInput.Subject);
        return exists;
    }

    public async Task<EmployeeEvaluationDto> Delete(EmployeeEvaluationInput evaluationInput)
    {
        var evaluation = await Get(
                                   evaluationInput.EmployeeEmail,
                                   evaluationInput.OwnerEmail,
                                   evaluationInput.Template,
                                   evaluationInput.Subject);

        var deletedEmployeeEvaluation = await _db.EmployeeEvaluation.Delete(evaluation.Id);

        return deletedEmployeeEvaluation;
    }

    public async Task<List<EmployeeEvaluationDto>> GetAllByEmployee(string email)
    {
        var exists = await _employeeService.CheckUserExist(email);

        if (!exists)
            throw new Exception($"Employee with {email} not found");

        var employeeEvaluations = await _db.EmployeeEvaluation
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
        var exists = await _employeeService.CheckUserExist(email);

        if (!exists)
            throw new Exception($"Employee with {email} not found");

        var employeeEvaluations = await _db.EmployeeEvaluation
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
        var exists = await _employeeEvaluationTemplateService.CheckIfExists(template);

        if (!exists)
            throw new Exception($"Employee Evaluation Template {template} not found");

        var employeeEvaluations = await _db.EmployeeEvaluation
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
        var employeeEvaluations = await _db.EmployeeEvaluation
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
        string employeeEmail,
        string ownerEmail,
        string template,
        string subject)
    {
        EmployeeEvaluationInput evaluationInput = new EmployeeEvaluationInput
        {
            Id = 0,
            OwnerEmail = ownerEmail,
            EmployeeEmail = employeeEmail,
            Template = template,
            Subject = subject
        };

        var exists = await CheckIfExists(evaluationInput);

        if (!exists) throw new Exception("Employee Evaluation not found");

        var employeeEvaluation = await _db.EmployeeEvaluation
                                          .Get(evaluation =>
                                                   evaluation.Employee.Email == employeeEmail &&
                                                   evaluation.Owner.Email == ownerEmail &&
                                                   evaluation.Template.Description == template &&
                                                   evaluation.Subject == subject)
                                          .AsNoTracking()
                                          .Include(evaluation => evaluation.Employee)
                                          .ThenInclude(employee => employee.EmployeeType)
                                          .Include(evaluation => evaluation.Owner)
                                          .ThenInclude(owner => owner.EmployeeType)
                                          .Include(evaluation => evaluation.Template)
                                          .FirstAsync();

        return employeeEvaluation.ToDto();
    }

    public async Task<EmployeeEvaluationDto> Save(EmployeeEvaluationInput evaluationInput)
    {
        var exists = await CheckIfExists(evaluationInput);

        if (exists)
            throw new Exception("Employee Evaluation already exists");

        var employeeDto = await _employeeService.GetEmployee(evaluationInput.EmployeeEmail);
        var ownerDto = await _employeeService.GetEmployee(evaluationInput.OwnerEmail);
        var templateDto = await _employeeEvaluationTemplateService.Get(evaluationInput.Template);

        EmployeeEvaluationDto employeeEvaluationDto = new EmployeeEvaluationDto
        {
            Id = 0,
            Employee = employeeDto,
            Template = templateDto,
            Owner = ownerDto,
            Subject = evaluationInput.Subject,
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = null
        };

        var savedEmployeeEvaluation = await _db.EmployeeEvaluation.Add(new EmployeeEvaluation(employeeEvaluationDto));

        return savedEmployeeEvaluation;
    }

    public async Task<EmployeeEvaluationDto> Update(
        EmployeeEvaluationInput oldEvaluation,
        EmployeeEvaluationInput newEvaluation)
    {
        var oldExists = await CheckIfExists(oldEvaluation);

        if (!oldExists)
            throw new Exception("Employee Evaluation not found");

        var employeeEvaluation = await Get(
                                           oldEvaluation.EmployeeEmail,
                                           oldEvaluation.OwnerEmail,
                                           oldEvaluation.Template,
                                           oldEvaluation.Subject);

        var employeeDto = await _employeeService.GetEmployee(newEvaluation.EmployeeEmail);
        var ownerDto = await _employeeService.GetEmployee(newEvaluation.OwnerEmail);
        var templateDto = await _employeeEvaluationTemplateService.Get(newEvaluation.Template);

        EmployeeEvaluationDto newEmployeeEvauation = new EmployeeEvaluationDto
        {
            Id = employeeEvaluation.Id,
            Employee = employeeDto,
            Template = templateDto,
            Owner = ownerDto,
            Subject = newEvaluation.Subject,
            StartDate = DateOnly.FromDateTime(DateTime.Now),
            EndDate = null 
        };


        var updatedEmployeeEvaluation =
            await _db.EmployeeEvaluation.Update(new EmployeeEvaluation(newEmployeeEvauation));

        return updatedEmployeeEvaluation;
    }
}
