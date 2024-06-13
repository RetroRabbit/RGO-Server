using HRIS.Models;
using RR.UnitOfWork.Entities;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Interfaces.HRIS;

public interface IDataReportRepository : IRepository<DataReport, DataReportDto>
{
}