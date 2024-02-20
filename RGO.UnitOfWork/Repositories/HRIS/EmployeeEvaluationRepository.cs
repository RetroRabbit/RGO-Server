using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class EmployeeEvaluationRepository : BaseRepository<EmployeeEvaluation, EmployeeEvaluationDto>,
                                            IEmployeeEvaluationRepository
{
    public EmployeeEvaluationRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}