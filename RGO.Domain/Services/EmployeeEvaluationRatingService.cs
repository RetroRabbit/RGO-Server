using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;

namespace RGO.Services.Services;

public class EmployeeEvaluationRatingService : IEmployeeEvaluationRatingService
{
    private readonly IUnitOfWork _db;

    public EmployeeEvaluationRatingService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<bool> CheckIfExists(string email, EmployeeEvaluationDto evaluation)
    {
        bool exists = await _db.EmployeeEvaluationRating
            .Any(x => x.Employee.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase)
                && x.Evaluation.Id == evaluation.Id);

        return exists;
    }

    public async Task<EmployeeEvaluationRatingDto> DeleteEmployeeEvaluationRating(string email, EmployeeEvaluationDto evaluation)
    {
        bool exists = await CheckIfExists(email, evaluation);

        if (!exists)
            throw new Exception($"Employee Evaluation Rating not found");

        EmployeeEvaluationRatingDto employeeEvaluationRating = await GetEmployeeEvaluationRating(email, evaluation);

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
            .Get(x => x.Employee.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase))
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

    public async Task<List<EmployeeEvaluationRatingDto>> GetAllEmployeeEvaluationRatingsByEvaluation(EmployeeEvaluationDto employeeEvaluationDto)
    {
        List<EmployeeEvaluationRatingDto> employeeEvaluationRatings = await _db.EmployeeEvaluationRating
            .Get(x => x.Evaluation.Id == employeeEvaluationDto.Id)
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

    public async Task<EmployeeEvaluationRatingDto> GetEmployeeEvaluationRating(string email, EmployeeEvaluationDto evaluation)
    {
        bool exists = await CheckIfExists(email, evaluation);

        if (!exists) throw new Exception($"Employee Evaluation Rating not found");

        EmployeeEvaluationRating employeeEvaluationRating = await _db.EmployeeEvaluationRating
            .Get(x => x.Employee.Email.Equals(email, StringComparison.CurrentCultureIgnoreCase)
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
        bool exists = await CheckIfExists(employeeEvaluationRatingDto.Employee!.Email, employeeEvaluationRatingDto.Evaluation!);

        if (!exists) throw new Exception("Employee Evaluation Rating already exists");

        EmployeeEvaluationRating employeeEvaluationRating = new(employeeEvaluationRatingDto);

        EmployeeEvaluationRatingDto savedEmployeeEvaluationRating = await _db.EmployeeEvaluationRating
            .Add(employeeEvaluationRating);

        return savedEmployeeEvaluationRating;
    }

    public async Task<EmployeeEvaluationRatingDto> UpdateEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto)
    {
        bool exists = await CheckIfExists(employeeEvaluationRatingDto.Employee!.Email, employeeEvaluationRatingDto.Evaluation!);

        if (!exists) throw new Exception("Employee Evaluation Rating not found");

        EmployeeEvaluationRating employeeEvaluationRating = new(employeeEvaluationRatingDto);

        EmployeeEvaluationRatingDto updatedEmployeeEvaluationRating = await _db.EmployeeEvaluationRating
            .Update(employeeEvaluationRating);

        return updatedEmployeeEvaluationRating;
    }
}