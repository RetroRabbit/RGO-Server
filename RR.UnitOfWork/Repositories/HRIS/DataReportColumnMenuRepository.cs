using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IDataReportColumnMenuRepository : IRepository<DataReportColumnMenu>
{
}

public class DataReportColumnMenuRepository : BaseRepository<DataReportColumnMenu>,
    IDataReportColumnMenuRepository
{
    public DataReportColumnMenuRepository(DatabaseContext db) : base(db)
    {
    }
}