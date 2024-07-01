using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IEmployeeTypeRepository : IRepository<EmployeeType>
{
}

public class EmployeeTypeRepository : BaseRepository<EmployeeType>, IEmployeeTypeRepository
{
    public EmployeeTypeRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}