using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IEmployeeDataRepository : IRepository<EmployeeData>
{
}

public class EmployeeDataRepository : BaseRepository<EmployeeData>, IEmployeeDataRepository
{
    public EmployeeDataRepository(DatabaseContext db) : base(db)
    {
    }
}