using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class EmployeeDateRepository : BaseRepository<EmployeeDate, EmployeeDateDto>, IEmployeeDateRepository
{
    public EmployeeDateRepository(DatabaseContext db) : base(db)
    {
    }
}