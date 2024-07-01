using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IMonthlyEmployeeTotalRepository : IRepository<MonthlyEmployeeTotal>
{
}

public class MonthlyEmployeeTotalRepository : BaseRepository<MonthlyEmployeeTotal>, IMonthlyEmployeeTotalRepository
{
    public MonthlyEmployeeTotalRepository(DatabaseContext db) : base(db)
    {
    }
}