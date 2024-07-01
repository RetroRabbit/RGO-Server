using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IEmployeeEvaluationTemplateItemRepository : IRepository<EmployeeEvaluationTemplateItem>
{
}

public class EmployeeEvaluationTemplateItemRepository : BaseRepository<EmployeeEvaluationTemplateItem>, IEmployeeEvaluationTemplateItemRepository
{
    public EmployeeEvaluationTemplateItemRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}