using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IDataReportColumnsRepository : IRepository<DataReportColumns>
{
}

public class DataReportColumnsRepository : BaseRepository<DataReportColumns>,
    IDataReportColumnsRepository
{
    public DataReportColumnsRepository(DatabaseContext db) : base(db)
    {
    }
}