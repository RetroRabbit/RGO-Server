using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class EmployeeTypeRepository : BaseRepository<EmployeeType, EmployeeTypeDto>, IEmployeeTypeRepository
{
    public EmployeeTypeRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}