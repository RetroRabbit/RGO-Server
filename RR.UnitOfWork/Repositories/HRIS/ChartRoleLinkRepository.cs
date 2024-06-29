using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IChartRoleLinkRepositories : IRepository<ChartRoleLink>
{
}

public class ChartRoleLinkRepository : BaseRepository<ChartRoleLink>, IChartRoleLinkRepositories
{
    public ChartRoleLinkRepository(DatabaseContext db) : base(db)
    {
    }
}