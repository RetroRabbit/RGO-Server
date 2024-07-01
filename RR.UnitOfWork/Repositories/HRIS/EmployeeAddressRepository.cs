using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IEmployeeAddressRepository : IRepository<EmployeeAddress>
{
}

public class EmployeeAddressRepository : BaseRepository<EmployeeAddress>, IEmployeeAddressRepository
{
    public EmployeeAddressRepository(DatabaseContext db) : base(db)
    {
    }
}