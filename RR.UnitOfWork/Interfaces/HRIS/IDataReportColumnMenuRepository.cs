using HRIS.Models.Report;
using RR.UnitOfWork.Entities;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Interfaces.HRIS;

public interface IDataReportColumnMenuRepository : IRepository<DataReportColumnMenu, DataReportColumnMenuDto>
{
}