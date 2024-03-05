using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class ChartRepository : BaseRepository<Chart, ChartDto>, IChartRepository
{
    public ChartRepository(DatabaseContext db) : base(db)
    {
    }
}