using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IEmployeeBankingRepository : IRepository<EmployeeBanking>
{
}

public class EmployeeBankingRepository : BaseRepository<EmployeeBanking>, IEmployeeBankingRepository
{
    public EmployeeBankingRepository(DatabaseContext db) : base(db)
    {
    }
}