using HRIS.Models.DataReport;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class DataReportValuesRepository : BaseRepository<DataReportValues, DataReportValuesDto>,
    IDataReportValuesRepository
{
    public DataReportValuesRepository(DatabaseContext db) : base(db)
    {
    }
}