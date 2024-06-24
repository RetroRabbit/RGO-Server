using HRIS.Models.Report;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class DataReportColumnMenuRepository : BaseRepository<DataReportColumnMenu, DataReportColumnMenuDto>,
    IDataReportColumnMenuRepository
{
    public DataReportColumnMenuRepository(DatabaseContext db) : base(db)
    {
    }
}