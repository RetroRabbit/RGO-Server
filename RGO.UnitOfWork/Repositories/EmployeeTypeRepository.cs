using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class EmployeeTypeRepository :BaseRepository<EmployeeType, EmployeeTypeDto>, IEmployeeTypeRepository
{
    public EmployeeTypeRepository(DatabaseContext databaseContext) : base(databaseContext)
    {
    }
}