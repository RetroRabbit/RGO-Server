using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IEmployeeEvaluationRatingRepository : IRepository<EmployeeEvaluationRating>
{
}

public class EmployeeEvaluationRatingRepository : BaseRepository<EmployeeEvaluationRating>, IEmployeeEvaluationRatingRepository
{
    public EmployeeEvaluationRatingRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}