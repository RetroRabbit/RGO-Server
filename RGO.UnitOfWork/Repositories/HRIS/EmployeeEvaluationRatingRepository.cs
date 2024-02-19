using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class EmployeeEvaluationRatingRepository : BaseRepository<EmployeeEvaluationRating, EmployeeEvaluationRatingDto>,
                                                  IEmployeeEvaluationRatingRepository
{
    public EmployeeEvaluationRatingRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}