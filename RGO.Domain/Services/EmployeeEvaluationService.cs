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
        /*bool exists = await _db.EmployeeEvaluation.Any(x => x.Employee.Email == evaluationInput.EmployeeEmail
            && x.Owner.Email == evaluationInput.OwnerEmail
            && x.Template.Description == evaluationInput.Template
            && x.Subject == evaluationInput.Subject);*/

        var evaluations = from evaluation in _db.EmployeeEvaluation.Get()
                          join employee in _db.Employee.Get() on evaluation.EmployeeId equals employee.Id
                          join owner in _db.Employee.Get() on evaluation.OwnerId equals owner.Id
                          join template in _db.EmployeeEvaluationTemplate.Get() on evaluation.TemplateId equals template.Id
                          where employee.Email == evaluationInput.EmployeeEmail
                              && owner.Email == evaluationInput.OwnerEmail
                              && template.Description == evaluationInput.Template
                              && evaluation.Subject == evaluationInput.Subject
                          select evaluation;

        bool exists = evaluations.Any();

        /*var list = from evaluation in _db.EmployeeEvaluation.Get()
                   join employee in _db.Employee.Get() on evaluation.EmployeeId equals employee.Id
                   join owner in _db.Employee.Get() on evaluation.OwnerId equals owner.Id
                   join template in _db.EmployeeEvaluationTemplate.Get() on evaluation.TemplateId equals template.Id
                   select new
                   {
                       evaluation,
                       employee,
                       owner,
                       template
                   };*/
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
        EmployeeEvaluationInput evaluationInput = new(0, ownerEmail, employeeEamil, template, subject);

        bool exists = await CheckIfExists(evaluationInput);

        if (!exists) throw new Exception("Employee Evaluation not found");

        /*EmployeeEvaluation employeeEvaluation = await _db.EmployeeEvaluation
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
            .FirstAsync();*/

        var query = from evaluation in _db.EmployeeEvaluation.Get()
                    join employee in _db.Employee.Get() on evaluation.EmployeeId equals employee.Id
                    join employeeType in _db.EmployeeType.Get() on employee.EmployeeTypeId equals employeeType.Id
                    join owner in _db.Employee.Get() on evaluation.OwnerId equals owner.Id
                    join ownerType in _db.EmployeeType.Get() on owner.EmployeeTypeId equals ownerType.Id
                    join template1 in _db.EmployeeEvaluationTemplate.Get() on evaluation.TemplateId equals template1.Id
                    where employee.Email == employeeEamil
                        && owner.Email == ownerEmail
                        && template1.Description == template
                        && evaluation.Subject == subject
                    select new
                    {
                        Evaluation = evaluation,
                        Employee = employee,
                        EmployeeType = employeeType,
                        Owner = owner,
                        OwnerType = ownerType,
                        Template = template1
                    };

        var result = await query.AsNoTracking().FirstAsync();

        EmployeeEvaluation employeeEvaluation = result.Evaluation;

        employeeEvaluation.Employee = result.Employee;
        employeeEvaluation.Employee.EmployeeType = result.EmployeeType;
        employeeEvaluation.Owner = result.Owner;
        employeeEvaluation.Owner.EmployeeType = result.OwnerType;
        employeeEvaluation.Template = result.Template;

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

        EmployeeEvaluationDto employeeEvaluationDto = new(
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

        EmployeeEvaluationDto newEmployeeEvauation = new(
            employeeEvaluation.Id,
            employeeDto,
            templateDto,
            ownerDto,
            newEvaluation.Subject,
            DateOnly.FromDateTime(DateTime.Now),
            null);

        EmployeeEvaluationDto updatedEmployeeEvaluation = await _db.EmployeeEvaluation.Update(new(newEmployeeEvauation));

        return updatedEmployeeEvaluation;
    }
}