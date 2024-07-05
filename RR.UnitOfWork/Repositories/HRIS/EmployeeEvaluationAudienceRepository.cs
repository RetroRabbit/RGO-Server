using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IEmployeeEvaluationAudienceRepository : IRepository<EmployeeEvaluationAudience>
{
}

public class EmployeeEvaluationAudienceRepository : BaseRepository<EmployeeEvaluationAudience>, IEmployeeEvaluationAudienceRepository
{
    public EmployeeEvaluationAudienceRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}