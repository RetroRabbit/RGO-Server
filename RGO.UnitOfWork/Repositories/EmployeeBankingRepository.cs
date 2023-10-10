using RGO.Models;
using RGO.UnitOfWork.Entities;
using RGO.UnitOfWork.Interfaces;

namespace RGO.UnitOfWork.Repositories;

public class EmployeeBankingRepository : BaseRepository<EmployeeBanking, EmployeeBankingDto>, IEmployeeBankingRepository
{
    public EmployeeBankingRepository(DatabaseContext db) : base(db)
    {

    }
}