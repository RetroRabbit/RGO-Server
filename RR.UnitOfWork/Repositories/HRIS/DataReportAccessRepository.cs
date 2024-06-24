using HRIS.Models;
using HRIS.Models.Report;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class DataReportAccessRepository : BaseRepository<DataReportAccess, DataReportAccessDto>, IDataReportAccessRepository
{
    public DataReportAccessRepository(DatabaseContext db) : base(db)
    {
    }
}