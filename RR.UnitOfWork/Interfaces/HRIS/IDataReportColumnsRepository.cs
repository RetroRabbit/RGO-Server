using HRIS.Models.DataReport;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Interfaces.HRIS;

public interface IDataReportColumnsRepository : IRepository<DataReportColumns, DataReportColumnsDto>
{
}