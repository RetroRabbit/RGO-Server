using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class DataReportColumnsRepository : BaseRepository<DataReportColumns, DataReportColumnsDto>, IDataReportColumnsRepository
{
    public DataReportColumnsRepository(DatabaseContext db) : base(db)
    {
    }
}