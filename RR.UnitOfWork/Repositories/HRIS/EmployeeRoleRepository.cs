using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class EmployeeRoleRepository : BaseRepository<EmployeeRole, EmployeeRoleDto>, IEmployeeRoleRepository
{
    public EmployeeRoleRepository(DatabaseContext db) : base(db)
    {
    }
}