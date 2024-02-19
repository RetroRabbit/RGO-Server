using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class EmployeeBankingRepository : BaseRepository<EmployeeBanking, EmployeeBankingDto>, IEmployeeBankingRepository
{
    public EmployeeBankingRepository(DatabaseContext db) : base(db)
    {
    }
}