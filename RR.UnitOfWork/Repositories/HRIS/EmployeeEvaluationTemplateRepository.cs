using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IEmployeeEvaluationTemplateRepository : IRepository<EmployeeEvaluationTemplate>
{
}

public class EmployeeEvaluationTemplateRepository : BaseRepository<EmployeeEvaluationTemplate>, IEmployeeEvaluationTemplateRepository
{
    public EmployeeEvaluationTemplateRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}