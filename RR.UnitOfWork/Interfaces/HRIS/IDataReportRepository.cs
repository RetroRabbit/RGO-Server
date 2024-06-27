using HRIS.Models.Report;
using HRIS.Models.Report.Response;
using RR.UnitOfWork.Entities;
using RR.UnitOfWork.Entities.HRIS;

namespace RR.UnitOfWork.Interfaces.HRIS;

public interface IDataReportRepository : IRepository<DataReport, DataReportDto>
{
    Task<DataReport?> GetReport(string code);
    Task<DataReport?> GetReport(int id);
    Task ConfirmAccessToReport(int reportId, int employeeId);
    Task<List<DataReportListResponse>?> GetReportsForEmployee(string employeeEmail);
}