using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class EmployeeEvaluationRatingRepository : BaseRepository<EmployeeEvaluationRating, EmployeeEvaluationRatingsDto>, IEmployeeEvaluationRatingRepository
{
    public EmployeeEvaluationRatingRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}