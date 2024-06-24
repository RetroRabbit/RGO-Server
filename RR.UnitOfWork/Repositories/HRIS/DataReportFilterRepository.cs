using HRIS.Models.Report;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class DataReportFilterRepository : BaseRepository<DataReportFilter, DataReportFilterDto>,
    IDataReportFilterRepository
{
    public DataReportFilterRepository(DatabaseContext db) : base(db)
    {
    }
}