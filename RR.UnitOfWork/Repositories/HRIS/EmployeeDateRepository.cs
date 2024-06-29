using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IEmployeeDateRepository : IRepository<EmployeeDate>
{
}

public class EmployeeDateRepository : BaseRepository<EmployeeDate>, IEmployeeDateRepository
{
    public EmployeeDateRepository(DatabaseContext db) : base(db)
    {
    }
}