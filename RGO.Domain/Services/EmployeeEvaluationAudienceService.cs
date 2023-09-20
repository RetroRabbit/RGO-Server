using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services;

public class EmployeeEvaluationAudienceService : IEmployeeEvaluationAudienceService
{
    private readonly IUnitOfWork _db;

    public EmployeeEvaluationAudienceService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<bool> CheckIfExists(EmployeeEvaluationDto evaluation, string email)
    {
        bool exists = await _db.EmployeeEvaluationAudience
            .Any(x => x.Employee.Email == email
                && x.Evaluation.Id == evaluation.Id);

        return exists;
    }

    public async Task<EmployeeEvaluationAudienceDto> Delete(EmployeeEvaluationDto evaluation, string email)
    {
        bool exists = await CheckIfExists(evaluation, email);

        if (!exists)
            throw new Exception($"Employee Evaluation Audience not found");

        EmployeeEvaluationAudienceDto employeeEvaluationAudience = await Get(evaluation, email);

        EmployeeEvaluationAudienceDto deletedEmployeeEvaluationAudience = await _db.EmployeeEvaluationAudience
            .Delete(employeeEvaluationAudience.Id);

        return deletedEmployeeEvaluationAudience;
    }

    public async Task<EmployeeEvaluationAudienceDto> Get(EmployeeEvaluationDto evaluation, string email)
    {
        bool exists = await CheckIfExists(evaluation, email);

        if (!exists)
            throw new Exception($"Employee Evaluation Audience not found");

        EmployeeEvaluationAudience employeeEvaluationAudience = await _db.EmployeeEvaluationAudience
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

    public async Task<List<EmployeeEvaluationAudienceDto>> GetAllbyEvaluation(EmployeeEvaluationDto evaluation)
    {
        var employeeEvaluationAudiences = await _db.EmployeeEvaluationAudience
            .Get(x => x.Evaluation.Id == evaluation.Id)
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

    public async Task<EmployeeEvaluationAudienceDto> Save(EmployeeEvaluationAudienceDto employeeEvaluationAudienceDto)
    {
        bool exists = await CheckIfExists(employeeEvaluationAudienceDto.Evaluation!, employeeEvaluationAudienceDto.Employee!.Email);

        if (!exists) throw new Exception($"Employee Evaluation Audience not found");

        EmployeeEvaluationAudience employeeEvaluationAudience = new EmployeeEvaluationAudience(employeeEvaluationAudienceDto);

        EmployeeEvaluationAudienceDto savedEmployeeEvaluationAudience = await _db.EmployeeEvaluationAudience
            .Add(employeeEvaluationAudience);

        return savedEmployeeEvaluationAudience;
    }

    public async Task<EmployeeEvaluationAudienceDto> Update(EmployeeEvaluationAudienceDto employeeEvaluationAudienceDto)
    {
        bool exists = await CheckIfExists(employeeEvaluationAudienceDto.Evaluation!, employeeEvaluationAudienceDto.Employee!.Email);

        if (!exists) throw new Exception($"Employee Evaluation Audience not found");

        EmployeeEvaluationAudience employeeEvaluationAudience = new EmployeeEvaluationAudience(employeeEvaluationAudienceDto);

        EmployeeEvaluationAudienceDto updatedEmployeeEvaluationAudience = await _db.EmployeeEvaluationAudience
            .Update(employeeEvaluationAudience);

        return updatedEmployeeEvaluationAudience;
    }
}
