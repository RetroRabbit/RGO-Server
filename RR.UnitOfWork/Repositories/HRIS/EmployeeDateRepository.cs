using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class EmployeeDateRepository : BaseRepository<EmployeeDate, EmployeeDateDto>, IEmployeeDateRepository
{
    public EmployeeDateRepository(DatabaseContext db) : base(db)
    {
    }
}