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

    public Task<EmployeeEvaluationRatingDto> DeleteEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto)
    {
        throw new NotImplementedException();
    }

    public Task<List<EmployeeEvaluationRatingDto>> GetAllEmployeeEvaluationRatings()
    {
        throw new NotImplementedException();
    }

    public Task<EmployeeEvaluationRatingDto> GetEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto)
    {
        throw new NotImplementedException();
    }

    public Task<EmployeeEvaluationRatingDto> SaveEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto)
    {
        throw new NotImplementedException();
    }

    public Task<EmployeeEvaluationRatingDto> UpdateEmployeeEvaluationRating(EmployeeEvaluationRatingDto employeeEvaluationRatingDto)
    {
        throw new NotImplementedException();
    }
}