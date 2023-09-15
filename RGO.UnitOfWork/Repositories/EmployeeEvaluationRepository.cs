using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class EmployeeEvaluationRepository : BaseRepository<EmployeeEvaluation, EmployeeEvaluationDto>, IEmployeeEvaluationRepository
{
    public EmployeeEvaluationRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}
