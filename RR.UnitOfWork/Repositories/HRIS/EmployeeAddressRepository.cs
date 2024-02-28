using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class EmployeeAddressRepository : BaseRepository<EmployeeAddress, EmployeeAddressDto>, IEmployeeAddressRepository
{
    public EmployeeAddressRepository(DatabaseContext db) : base(db)
    {
    }
}