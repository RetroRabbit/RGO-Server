using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class EmployeeEvaluationTemplateRepository : BaseRepository<EmployeeEvaluationTemplate, EmployeeEvaluationTemplateDto>, IEmployeeEvaluationTemplateRepository
{
    public EmployeeEvaluationTemplateRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}
