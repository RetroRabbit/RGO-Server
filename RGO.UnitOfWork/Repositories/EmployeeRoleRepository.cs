using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class EmployeeRoleRepository : BaseRepository<EmployeeRole, EmployeeRoleDto>, IEmployeeRoleRepository
{
    public EmployeeRoleRepository(DatabaseContext db) : base(db)
    {
    }
}