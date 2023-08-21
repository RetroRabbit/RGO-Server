using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class EmployeeAddressRepository : BaseRepository<EmployeeAddress, EmployeeAddressDto>, IEmployeeAddressRepository
{
    public EmployeeAddressRepository(DatabaseContext db) : base(db)
    {
    }
}