using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IDataReportFilterRepository : IRepository<DataReportFilter>
{
}

public class DataReportFilterRepository : BaseRepository<DataReportFilter>,
    IDataReportFilterRepository
{
    public DataReportFilterRepository(DatabaseContext db) : base(db)
    {
    }
}