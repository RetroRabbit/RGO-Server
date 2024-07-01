using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IDataReportValuesRepository : IRepository<DataReportValues>
{
}

public class DataReportValuesRepository : BaseRepository<DataReportValues>,
    IDataReportValuesRepository
{
    public DataReportValuesRepository(DatabaseContext db) : base(db)
    {
    }
}