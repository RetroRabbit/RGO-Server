using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IEmployeeEvaluationRepository : IRepository<EmployeeEvaluation>
{
}

public class EmployeeEvaluationRepository : BaseRepository<EmployeeEvaluation>, IEmployeeEvaluationRepository
{
    public EmployeeEvaluationRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}