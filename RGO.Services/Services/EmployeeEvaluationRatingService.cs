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

    public async Task<bool> CheckIfExists(EvaluationRatingInput rating)
    {
        bool exists = await _db.EmployeeEvaluationRating
            .Any(x => x.Employee.Email== rating.EmployeeEmail
                && x.Evaluation.Employee.Email == rating.Evaluation.EmployeeEmail
                && x.Evaluation.Owner.Email == rating.Evaluation.OwnerEmail
                && x.Evaluation.Template.Description == rating.Evaluation.Template
                && x.Evaluation.Subject == rating.Evaluation.Subject
                && x.Description == rating.Description
                && x.Comment == rating.Comment
                && x.Score == rating.Score);

        return exists;
    }

    public async Task<EmployeeEvaluationRatingDto> Get(EvaluationRatingInput rating)
    {
        EmployeeEvaluationInput evaluationInputToCheck = new(
            rating.Evaluation.Id,
            rating.Evaluation.OwnerEmail,
            rating.Evaluation.EmployeeEmail,
            rating.Evaluation.Template,
            rating.Evaluation.Subject);

        bool evaluationExists = await _employeeEvaluationService.CheckIfExists(evaluationInputToCheck);

        if (!evaluationExists)
            throw new Exception($"Employee Evaluation not found");


        bool exists = await CheckIfExists(rating);

        if (!exists) throw new Exception($"Employee Evaluation Rating not found");

        EmployeeEvaluationRating employeeEvaluationRating = await _db.EmployeeEvaluationRating
            .Get(x => x.Employee.Email == rating.EmployeeEmail
                && x.Evaluation.Employee.Email == rating.Evaluation.EmployeeEmail
                && x.Evaluation.Owner.Email == rating.Evaluation.OwnerEmail
                && x.Evaluation.Template.Description == rating.Evaluation.Template
                && x.Evaluation.Subject == rating.Evaluation.Subject
                && x.Description == rating.Description
                && x.Comment == rating.Comment
                && x.Score == rating.Score)
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

        bool exists = await CheckIfExists(rating);

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
        bool exists = await CheckIfExists(rating);

        if (!exists) throw new Exception("Employee Evaluation Rating not found");

        var ratingDto = await Get(rating);
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

        bool exists = await CheckIfExists(rating);

        if (!exists)
            throw new Exception($"Employee Evaluation Rating not found");

        EmployeeEvaluationRatingDto employeeEvaluationRating = await Get(rating);

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