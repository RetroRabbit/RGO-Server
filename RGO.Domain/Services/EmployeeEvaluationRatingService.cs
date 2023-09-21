using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services;

public class EmployeeEvaluationRatingService : IEmployeeEvaluationRatingService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeEvaluationService _employeeEvaluationService;

    public EmployeeEvaluationRatingService(IUnitOfWork db, IEmployeeEvaluationService employeeEvaluationService)
    {
        _db = db;
        _employeeEvaluationService = employeeEvaluationService;
    }

    public async Task<bool> CheckIfExists(string email, int evaluationId)
    {
        bool exists = await _db.EmployeeEvaluationRating
            .Any(x => x.Employee.Email== email
                && x.Evaluation.Id == evaluationId);

        return exists;
    }

    public async Task<EmployeeEvaluationRatingDto> DeleteEmployeeEvaluationRating(string email, EmployeeEvaluationInput evaluationInput)
    {
        bool evaluationExists = await _employeeEvaluationService.CheckIfExists(evaluationInput);

        if (!evaluationExists)
            throw new Exception($"Employee Evaluation not found");

        EmployeeEvaluationDto evaluation = await _employeeEvaluationService.GetEmployeeEvaluation(
            evaluationInput.EmployeeEmail,
            evaluationInput.OwnerEmail,
            evaluationInput.Template,
            evaluationInput.Subject);

        bool exists = await CheckIfExists(email, evaluation.Id);

        if (!exists)
            throw new Exception($"Employee Evaluation Rating not found");

        EmployeeEvaluationRatingDto employeeEvaluationRating = await GetEmployeeEvaluationRating(
            email,
            evaluationInput);

        EmployeeEvaluationRatingDto deletedEmployeeEvaluationRating = await _db.EmployeeEvaluationRating
            .Delete(employeeEvaluationRating.Id);

        return deletedEmployeeEvaluationRating;
    }

    public async Task<List<EmployeeEvaluationRatingDto>> GetAllEmployeeEvaluationRatings()
    {
        List<EmployeeEvaluationRatingDto> employeeEvaluationRatings = await _db.EmployeeEvaluationRating
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

        return employeeEvaluationRatings;
    }

    public async Task<List<EmployeeEvaluationRatingDto>> GetAllEmployeeEvaluationRatingsByEmployee(string email)
    {
        List<EmployeeEvaluationRatingDto> employeeEvaluationRatings = await _db.EmployeeEvaluationRating
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

        return employeeEvaluationRatings;
    }

    public async Task<List<EmployeeEvaluationRatingDto>> GetAllEmployeeEvaluationRatingsByEvaluation(EmployeeEvaluationInput evaluationInput)
    {
        bool exists = await _employeeEvaluationService.CheckIfExists(evaluationInput);

        if (!exists)
            throw new Exception($"Employee Evaluation not found");

        EmployeeEvaluationDto evaluation = await _employeeEvaluationService.GetEmployeeEvaluation(
            evaluationInput.EmployeeEmail,
            evaluationInput.OwnerEmail,
            evaluationInput.Template,
            evaluationInput.Subject);

        List<EmployeeEvaluationRatingDto> employeeEvaluationRatings = await _db.EmployeeEvaluationRating
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

        return employeeEvaluationRatings;
    }

    public async Task<EmployeeEvaluationRatingDto> GetEmployeeEvaluationRating(string email, EmployeeEvaluationInput evaluationInput)
    {
        bool evaluationExists = await _employeeEvaluationService.CheckIfExists(evaluationInput);

        if (!evaluationExists)
            throw new Exception($"Employee Evaluation not found");

        EmployeeEvaluationDto evaluation = await _employeeEvaluationService.GetEmployeeEvaluation(
            evaluationInput.EmployeeEmail,
            evaluationInput.OwnerEmail,
            evaluationInput.Template,
            evaluationInput.Subject);

        bool exists = await CheckIfExists(email, evaluation.Id);

        if (!exists) throw new Exception($"Employee Evaluation Rating not found");

        EmployeeEvaluationRating employeeEvaluationRating = await _db.EmployeeEvaluationRating
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

        return employeeEvaluationRating.ToDto();
    }
    
    public async Task<EmployeeEvaluationRatingDto> SaveEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto)
    {
        bool exists = await CheckIfExists(employeeEvaluationRatingDto.Employee!.Email, employeeEvaluationRatingDto.Evaluation!.Id);

        if (!exists) throw new Exception("Employee Evaluation Rating already exists");

        EmployeeEvaluationRating employeeEvaluationRating = new(employeeEvaluationRatingDto);

        EmployeeEvaluationRatingDto savedEmployeeEvaluationRating = await _db.EmployeeEvaluationRating
            .Add(employeeEvaluationRating);

        return savedEmployeeEvaluationRating;
    }

    public async Task<EmployeeEvaluationRatingDto> UpdateEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto)
    {
        bool exists = await CheckIfExists(employeeEvaluationRatingDto.Employee!.Email, employeeEvaluationRatingDto.Evaluation!.Id);

        if (!exists) throw new Exception("Employee Evaluation Rating not found");

        EmployeeEvaluationRating employeeEvaluationRating = new(employeeEvaluationRatingDto);

        EmployeeEvaluationRatingDto updatedEmployeeEvaluationRating = await _db.EmployeeEvaluationRating
            .Update(employeeEvaluationRating);

        return updatedEmployeeEvaluationRating;
    }
}