using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public interface IDataReportAccessRepository : IRepository<DataReportAccess>
{
}

public class DataReportAccessRepository : BaseRepository<DataReportAccess>, IDataReportAccessRepository
{
    public DataReportAccessRepository(DatabaseContext db) : base(db)
    {
    }
}