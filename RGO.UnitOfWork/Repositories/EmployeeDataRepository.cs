using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class EmployeeDataRepository : BaseRepository<EmployeeData, EmployeeDataDto>, IEmployeeDataRepository
{
    public EmployeeDataRepository(DatabaseContext db) : base(db)
    {
    }
}
