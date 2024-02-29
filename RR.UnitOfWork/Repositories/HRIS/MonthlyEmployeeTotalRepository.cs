using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class MonthlyEmployeeTotalRepository : BaseRepository<MonthlyEmployeeTotal, MonthlyEmployeeTotalDto>,
                                              IMonthlyEmployeeTotalRepository
{
    public MonthlyEmployeeTotalRepository(DatabaseContext db) : base(db)
    {
    }
}