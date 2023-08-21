using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class EmployeeRepository : BaseRepository<Employee, EmployeeDto>, IEmployeeRepository
{
    public EmployeeRepository(DatabaseContext db) : base(db)
    {
    }
}
