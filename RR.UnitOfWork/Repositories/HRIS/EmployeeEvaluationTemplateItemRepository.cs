using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class EmployeeEvaluationTemplateItemRepository :
    BaseRepository<EmployeeEvaluationTemplateItem, EmployeeEvaluationTemplateItemDto>,
    IEmployeeEvaluationTemplateItemRepository
{
    public EmployeeEvaluationTemplateItemRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}