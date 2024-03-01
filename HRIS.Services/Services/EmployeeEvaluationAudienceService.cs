using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeEvaluationAudienceService : IEmployeeEvaluationAudienceService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeEvaluationService _employeeEvaluationService;
    private readonly IEmployeeService _employeeService;

    public EmployeeEvaluationAudienceService(
        IUnitOfWork db,
        IEmployeeService employeeService,
        IEmployeeEvaluationService employeeEvaluationService)
    {
        _db = db;
        _employeeService = employeeService;
        _employeeEvaluationService = employeeEvaluationService;
    }

    public async Task<bool> CheckIfExists(EmployeeEvaluationDto evaluation, string email)
    {
        var exists = await _db.EmployeeEvaluationAudience
                              .Any(x => x.Employee.Email == email
                                        && x.Evaluation.Id == evaluation.Id);

        return exists;
    }

    public async Task<EmployeeEvaluationAudienceDto> Delete(string email, EmployeeEvaluationInput evaluationInput)
    {
        var evaluationDto = await _employeeEvaluationService.Get(
                                                                 evaluationInput.EmployeeEmail,
                                                                 evaluationInput.OwnerEmail,
                                                                 evaluationInput.Template,
                                                                 evaluationInput.Subject);

        var exists = await CheckIfExists(evaluationDto, email);

        if (!exists)
            throw new Exception("Employee Evaluation Audience not found");

        var employeeEvaluationAudience = await Get(evaluationDto, email);

        var deletedEmployeeEvaluationAudience = await _db.EmployeeEvaluationAudience
                                                         .Delete(employeeEvaluationAudience.Id);

        return deletedEmployeeEvaluationAudience;
    }

    public async Task<EmployeeEvaluationAudienceDto> Get(EmployeeEvaluationDto evaluation, string email)
    {
        var exists = await CheckIfExists(evaluation, email);

        if (!exists)
            throw new Exception("Employee Evaluation Audience not found");

        var employeeEvaluationAudience = await _db.EmployeeEvaluationAudience
                                                  .Get(x => x.Employee.Email == email
                                                            && x.Evaluation.Id == evaluation.Id)
                                                  .AsNoTracking()
                                                  .Include(x => x.Employee)
                                                  .Include(x => x.Employee.EmployeeType)
                                                  .Include(x => x.Evaluation)
                                                  .Include(x => x.Evaluation.Employee)
                                                  .Include(x => x.Evaluation.Employee.EmployeeType)
                                                  .Include(x => x.Evaluation.Template)
                                                  .FirstAsync();

        return employeeEvaluationAudience.ToDto();
    }

    public async Task<List<EmployeeEvaluationAudienceDto>> GetAll()
    {
        var employeeEvaluationAudiences = await _db.EmployeeEvaluationAudience
                                                   .Get()
                                                   .AsNoTracking()
                                                   .Include(x => x.Employee)
                                                   .Include(x => x.Employee.EmployeeType)
                                                   .Include(x => x.Evaluation)
                                                   .Include(x => x.Evaluation.Employee)
                                                   .Include(x => x.Evaluation.Employee.EmployeeType)
                                                   .Include(x => x.Evaluation.Template)
                                                   .Select(x => x.ToDto())
                                                   .ToListAsync();

        return employeeEvaluationAudiences;
    }

    public async Task<List<EmployeeEvaluationAudienceDto>> GetAllbyEmployee(string email)
    {
        var employeeExists = await _employeeService.CheckUserExist(email);

        if (!employeeExists)
            throw new Exception($"Employee with {email} not found");

        var employeeEvaluationAudiences = await _db.EmployeeEvaluationAudience
                                                   .Get(x => x.Employee.Email == email)
                                                   .AsNoTracking()
                                                   .Include(x => x.Employee)
                                                   .Include(x => x.Employee.EmployeeType)
                                                   .Include(x => x.Evaluation)
                                                   .Include(x => x.Evaluation.Employee)
                                                   .Include(x => x.Evaluation.Employee.EmployeeType)
                                                   .Include(x => x.Evaluation.Template)
                                                   .Select(x => x.ToDto())
                                                   .ToListAsync();

        return employeeEvaluationAudiences;
    }

    public async Task<List<EmployeeEvaluationAudienceDto>> GetAllbyEvaluation(EmployeeEvaluationInput evaluation)
    {
        var evaluationExists = await _employeeEvaluationService.CheckIfExists(evaluation);

        if (!evaluationExists)
            throw new Exception("Employee Evaluation not found");

        var employeeEvaluationAudiences = await _db.EmployeeEvaluationAudience
                                                   .Get(x => x.Evaluation.Owner.Email == evaluation.OwnerEmail
                                                             && x.Evaluation.Employee.Email == evaluation.EmployeeEmail
                                                             && x.Evaluation.Template.Description == evaluation.Template
                                                             && x.Evaluation.Subject == evaluation.Subject)
                                                   .AsNoTracking()
                                                   .Include(x => x.Employee)
                                                   .Include(x => x.Employee.EmployeeType)
                                                   .Include(x => x.Evaluation)
                                                   .Include(x => x.Evaluation.Employee)
                                                   .Include(x => x.Evaluation.Employee.EmployeeType)
                                                   .Include(x => x.Evaluation.Template)
                                                   .Select(x => x.ToDto())
                                                   .ToListAsync();

        return employeeEvaluationAudiences;
    }

    public async Task<EmployeeEvaluationAudienceDto> Save(string email, EmployeeEvaluationInput evaluationInput)
    {
        var evaluationDto = await _employeeEvaluationService.Get(
                                                                 evaluationInput.EmployeeEmail,
                                                                 evaluationInput.OwnerEmail,
                                                                 evaluationInput.Template,
                                                                 evaluationInput.Subject);

        var employeeDto = await _employeeService.GetEmployee(email);

        var exists = await CheckIfExists(evaluationDto, email);

        if (exists) throw new Exception("Employee Evaluation Audience not found");

        var employeeEvaluationAudienceDto = new EmployeeEvaluationAudienceDto { Id = 0,  Evaluation = evaluationDto, Employee = employeeDto };
        var savedEmployeeEvaluationAudience = await _db.EmployeeEvaluationAudience
                                                       .Add(new
                                                                EmployeeEvaluationAudience(employeeEvaluationAudienceDto));

        return savedEmployeeEvaluationAudience;
    }

    public async Task<EmployeeEvaluationAudienceDto> Update(EmployeeEvaluationAudienceDto employeeEvaluationAudienceDto)
    {
        var exists = await CheckIfExists(employeeEvaluationAudienceDto.Evaluation!,
                                         employeeEvaluationAudienceDto.Employee!.Email!);

        if (!exists) throw new Exception("Employee Evaluation Audience not found");

        var employeeEvaluationAudience = new EmployeeEvaluationAudience(employeeEvaluationAudienceDto);

        var updatedEmployeeEvaluationAudience = await _db.EmployeeEvaluationAudience
                                                         .Update(employeeEvaluationAudience);

        return updatedEmployeeEvaluationAudience;
    }
}
