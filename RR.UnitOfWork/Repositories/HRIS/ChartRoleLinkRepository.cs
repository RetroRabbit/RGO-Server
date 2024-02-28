using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class ChartRoleLinkRepository : BaseRepository<ChartRoleLink, ChartRoleLinkDto>, IChartRoleLinkRepositories
{
    public ChartRoleLinkRepository(DatabaseContext db) : base(db)
    {
    }
}