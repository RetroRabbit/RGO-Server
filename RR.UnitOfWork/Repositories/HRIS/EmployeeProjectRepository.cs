using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IEmployeeProjectRepository : IRepository<EmployeeProject>
{
}

public class EmployeeProjectRepository : BaseRepository<EmployeeProject>, IEmployeeProjectRepository
{
    public EmployeeProjectRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}