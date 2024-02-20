using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class EmployeeDataRepository : BaseRepository<EmployeeData, EmployeeDataDto>, IEmployeeDataRepository
{
    public EmployeeDataRepository(DatabaseContext db) : base(db)
    {
    }
}