using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class EmployeeRepository : BaseRepository<Employee, EmployeeDto>, IEmployeeRepository
{
    public EmployeeRepository(DatabaseContext db) : base(db)
    {
    }
}