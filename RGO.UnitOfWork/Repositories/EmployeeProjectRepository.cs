using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class EmployeeProjectRepository : BaseRepository<EmployeeProject, EmployeeProjectDto>, IEmployeeProjectRepository
{
    public EmployeeProjectRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}