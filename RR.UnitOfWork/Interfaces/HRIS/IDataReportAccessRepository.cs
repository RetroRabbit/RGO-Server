using HRIS.Models.Report;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Interfaces.HRIS;

public interface IDataReportAccessRepository : IRepository<DataReportAccess, DataReportAccessDto>
{
}