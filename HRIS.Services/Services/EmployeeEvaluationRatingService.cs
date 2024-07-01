using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class EmployeeEvaluationRatingService : IEmployeeEvaluationRatingService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeEvaluationService _employeeEvaluationService;
    private readonly IEmployeeService _employeeService;
    private readonly IErrorLoggingService _errorLoggingService;

    public EmployeeEvaluationRatingService(IUnitOfWork db, IEmployeeEvaluationService employeeEvaluationService,
                                           IEmployeeService employeeService, IErrorLoggingService errorLoggingService)
    {
        _db = db;
        _employeeEvaluationService = employeeEvaluationService;
        _employeeService = employeeService;
        _errorLoggingService = errorLoggingService;
    }

    public async Task<bool> CheckIfExists(EvaluationRatingInput rating)
    {
        var exists = await _db.EmployeeEvaluationRating
                              .Any(x => x.Employee.Email == rating.EmployeeEmail
                                        && x.Evaluation.Employee.Email == rating.Evaluation!.EmployeeEmail
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
        EmployeeEvaluationInput evaluationInputToCheck = new EmployeeEvaluationInput
        {
            Id =rating.Evaluation!.Id,
            OwnerEmail = rating.Evaluation.OwnerEmail,
            EmployeeEmail = rating.Evaluation.EmployeeEmail,
            Template = rating.Evaluation.Template,
            Subject = rating.Evaluation.Subject
        };

        var evaluationExists = await _employeeEvaluationService.CheckIfExists(evaluationInputToCheck);

        if (!evaluationExists)
        {
            var exception = new Exception("Employee Evaluation not found");
            throw _errorLoggingService.LogException(exception);
        }

        var exists = await CheckIfExists(rating);

        if (!exists)
        {
            var exception = new Exception("Employee Evaluation Rating not found");
            throw _errorLoggingService.LogException(exception);
        }

        var employeeEvaluationRating = await _db.EmployeeEvaluationRating
                                                .Get(x => x.Employee.Email == rating.EmployeeEmail
                                                          && x.Evaluation.Employee.Email ==
                                                          rating.Evaluation.EmployeeEmail
                                                          && x.Evaluation.Owner.Email == rating.Evaluation.OwnerEmail
                                                          && x.Evaluation.Template.Description ==
                                                          rating.Evaluation.Template
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
        var employee = await _employeeService.GetEmployee(rating.EmployeeEmail);
        var evaluation = await _employeeEvaluationService.Get(
                                                              rating.Evaluation!.EmployeeEmail,
                                                              rating.Evaluation.OwnerEmail,
                                                              rating.Evaluation.Template,
                                                              rating.Evaluation.Subject);

        var exists = await CheckIfExists(rating);

        if (exists)
        {
            var exception = new Exception("Employee Evaluation Rating already exists");
            throw _errorLoggingService.LogException(exception);
        }

        var evaluationRating = new EmployeeEvaluationRatingDto
        {
            Id = 0,
            Evaluation = evaluation,
            Employee = employee,
            Description = rating.Description,
            Score = rating.Score,
            Comment = rating.Comment
        };

        var savedEmployeeEvaluationRating = await _db.EmployeeEvaluationRating
                                                     .Add(new EmployeeEvaluationRating(evaluationRating));

        return savedEmployeeEvaluationRating.ToDto();
    }

    public async Task<EmployeeEvaluationRatingDto> Update(EvaluationRatingInput rating)
    {
        var exists = await CheckIfExists(rating);

        if (!exists)
        {
            var exception = new Exception("Employee Evaluation Rating not found");
            throw _errorLoggingService.LogException(exception);
        }

        var ratingDto = await Get(rating);
        var ratingDtoToUpdate = new EmployeeEvaluationRatingDto
        {
            Id = ratingDto.Id,
            Evaluation = ratingDto.Evaluation,
            Employee = ratingDto.Employee,
            Description = rating.Description,
            Score = rating.Score,
            Comment = rating.Comment
        };

        var updatedEmployeeEvaluationRating = await _db.EmployeeEvaluationRating
                                                       .Update(new EmployeeEvaluationRating(ratingDtoToUpdate));

        return updatedEmployeeEvaluationRating.ToDto();
    }

    public async Task<EmployeeEvaluationRatingDto> Delete(EvaluationRatingInput rating)
    {
        var evaluationExists = await _employeeEvaluationService.CheckIfExists(rating.Evaluation!);

        if (!evaluationExists)
        {
            var exception = new Exception("Employee Evaluation not found");
            throw _errorLoggingService.LogException(exception);
        }

        var exists = await CheckIfExists(rating);

        if (!exists)
        {
            var exception = new Exception("Employee Evaluation Rating not found");
            throw _errorLoggingService.LogException(exception);
        }

        var employeeEvaluationRating = await Get(rating);

        var deletedEmployeeEvaluationRating = await _db.EmployeeEvaluationRating
                                                       .Delete(employeeEvaluationRating.Id);

        return deletedEmployeeEvaluationRating.ToDto();
    }

    public async Task<List<EmployeeEvaluationRatingDto>> GetAll()
    {
        var employeeEvaluationRatings = await _db.EmployeeEvaluationRating
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
        var employeeEvaluationRatings = await _db.EmployeeEvaluationRating
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
        var exists = await _employeeEvaluationService.CheckIfExists(evaluationInput);

        if (!exists)
        {
            var exception = new Exception("Employee Evaluation not found");
            throw _errorLoggingService.LogException(exception);
        }

        var evaluation = await _employeeEvaluationService.Get(
                                                              evaluationInput.EmployeeEmail,
                                                              evaluationInput.OwnerEmail,
                                                              evaluationInput.Template,
                                                              evaluationInput.Subject);

        var employeeEvaluationRatings = await _db.EmployeeEvaluationRating
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
