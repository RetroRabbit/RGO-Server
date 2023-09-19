using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class EmployeeEvaluationAudienceRepository : BaseRepository<EmployeeEvaluationAudience, EmployeeEvaluationAudienceDto>, IEmployeeEvaluationAudienceRepository
{
    public EmployeeEvaluationAudienceRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}