using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class EmployeeEvaluationAudienceRepository :
    BaseRepository<EmployeeEvaluationAudience, EmployeeEvaluationAudienceDto>, IEmployeeEvaluationAudienceRepository
{
    public EmployeeEvaluationAudienceRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}