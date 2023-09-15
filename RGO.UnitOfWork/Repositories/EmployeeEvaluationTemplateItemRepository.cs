using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class EmployeeEvaluationTemplateItemRepository : BaseRepository<EmployeeEvaluationTemplateItem, EmployeeEvaluationTemplateItemDto>, IEmployeeEvaluationTemplateItemRepository
{
    public EmployeeEvaluationTemplateItemRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}
