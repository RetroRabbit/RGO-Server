using HRIS.Models;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Interfaces.HRIS;

namespace RR.UnitOfWork.Repositories.HRIS;

public class DataReportRepository : BaseRepository<DataReport, DataReportDto>, IDataReportRepository
{
    public DataReportRepository(DatabaseContext db) : base(db)
    {
    }
}