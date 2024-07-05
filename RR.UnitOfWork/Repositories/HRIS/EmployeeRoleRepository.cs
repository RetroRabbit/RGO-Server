using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IEmployeeRoleRepository : IRepository<EmployeeRole>
{
}

public class EmployeeRoleRepository : BaseRepository<EmployeeRole>, IEmployeeRoleRepository
{
    public EmployeeRoleRepository(DatabaseContext db) : base(db)
    {
    }
}