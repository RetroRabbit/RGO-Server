using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class EmployeeEvaluationTemplateRepository :
    BaseRepository<EmployeeEvaluationTemplate, EmployeeEvaluationTemplateDto>, IEmployeeEvaluationTemplateRepository
{
    public EmployeeEvaluationTemplateRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}