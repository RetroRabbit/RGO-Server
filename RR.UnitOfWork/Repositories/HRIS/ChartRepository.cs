using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IChartRepository : IRepository<Chart>
{
}

public class ChartRepository : BaseRepository<Chart>, IChartRepository
{
    public ChartRepository(DatabaseContext db) : base(db)
    {
    }
}