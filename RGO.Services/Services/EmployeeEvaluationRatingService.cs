using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services;

public class EmployeeEvaluationRatingService : IEmployeeEvaluationRatingService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeService _employeeService;
    private readonly IEmployeeEvaluationService _employeeEvaluationService;

    public EmployeeEvaluationRatingService(IUnitOfWork db, IEmployeeEvaluationService employeeEvaluationService, IEmployeeService employeeService)
    {
        _db = db;
        _employeeEvaluationService = employeeEvaluationService;
        _employeeService = employeeService;
    }

    public async Task<bool> CheckIfExists(string email, int evaluationId)
    {
        bool exists = await _db.EmployeeEvaluationRating
            .Any(x => x.Employee.Email== email
                && x.Evaluation.Id == evaluationId);

        return exists;
    }

    public async Task<EmployeeEvaluationRatingDto> Get(string email, EmployeeEvaluationInput evaluationInput)
    {
        bool evaluationExists = await _employeeEvaluationService.CheckIfExists(evaluationInput);

        if (!evaluationExists)
            throw new Exception($"Employee Evaluation not found");

        EmployeeEvaluationDto evaluation = await _employeeEvaluationService.Get(
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

    public async Task<EmployeeEvaluationRatingDto> Save(EvaluationRatingInput rating)
    {
        EmployeeDto employee = await _employeeService.GetEmployee(rating.EmployeeEmail);
        EmployeeEvaluationDto evaluation = await _employeeEvaluationService.Get(
            rating.Evaluation.EmployeeEmail,
            rating.Evaluation.OwnerEmail,
            rating.Evaluation.Template,
            rating.Evaluation.Subject);

        bool exists = await CheckIfExists(employee!.Email, evaluation.Id);

        if (exists) throw new Exception("Employee Evaluation Rating already exists");

        EmployeeEvaluationRatingDto evaluationRating = new EmployeeEvaluationRatingDto(
            0,
            evaluation,
            employee,
            rating.Description,
            rating.Score,
            rating.Comment);

        EmployeeEvaluationRatingDto savedEmployeeEvaluationRating = await _db.EmployeeEvaluationRating
            .Add(new(evaluationRating));

        return savedEmployeeEvaluationRating;
    }

    public async Task<EmployeeEvaluationRatingDto> Update(EvaluationRatingInput rating)
    {
        bool exists = await CheckIfExists(rating.EmployeeEmail, (int)rating.Evaluation.Id!);

        if (!exists) throw new Exception("Employee Evaluation Rating not found");

        var ratingDto = await Get(rating.EmployeeEmail, rating.Evaluation);
        var ratingDtoToUpdate = new EmployeeEvaluationRatingDto(
            ratingDto.Id,
            ratingDto.Evaluation,
            ratingDto.Employee,
            rating.Description,
            rating.Score,
            rating.Comment);

        EmployeeEvaluationRatingDto updatedEmployeeEvaluationRating = await _db.EmployeeEvaluationRating
            .Update(new(ratingDtoToUpdate));

        return updatedEmployeeEvaluationRating;
    }

    public async Task<EmployeeEvaluationRatingDto> Delete(EvaluationRatingInput rating)
    {
        bool evaluationExists = await _employeeEvaluationService.CheckIfExists(rating.Evaluation);

        if (!evaluationExists)
            throw new Exception($"Employee Evaluation not found");

        EmployeeEvaluationDto evaluation = await _employeeEvaluationService.Get(
            rating.Evaluation.EmployeeEmail,
            rating.Evaluation.OwnerEmail,
            rating.Evaluation.Template,
            rating.Evaluation.Subject);

        bool exists = await CheckIfExists(rating.EmployeeEmail, evaluation.Id);

        if (!exists)
            throw new Exception($"Employee Evaluation Rating not found");

        EmployeeEvaluationRatingDto employeeEvaluationRating = await Get(
            rating.EmployeeEmail,
            rating.Evaluation);

        EmployeeEvaluationRatingDto deletedEmployeeEvaluationRating = await _db.EmployeeEvaluationRating
            .Delete(employeeEvaluationRating.Id);

        return deletedEmployeeEvaluationRating;
    }

    public async Task<List<EmployeeEvaluationRatingDto>> GetAll()
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

    public async Task<List<EmployeeEvaluationRatingDto>> GetAllByEmployee(string email)
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

    public async Task<List<EmployeeEvaluationRatingDto>> GetAllByEvaluation(EmployeeEvaluationInput evaluationInput)
    {
        bool exists = await _employeeEvaluationService.CheckIfExists(evaluationInput);

        if (!exists)
            throw new Exception($"Employee Evaluation not found");

        EmployeeEvaluationDto evaluation = await _employeeEvaluationService.Get(
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
}