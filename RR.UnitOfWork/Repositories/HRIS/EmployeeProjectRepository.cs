using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class EmployeeProjectRepository : BaseRepository<EmployeeProject, EmployeeProjectDto>, IEmployeeProjectRepository
{
    public EmployeeProjectRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}