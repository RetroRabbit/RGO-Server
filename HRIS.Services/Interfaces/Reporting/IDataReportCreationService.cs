using HRIS.Models.Report.Request;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Interfaces.Reporting;

public interface IDataReportCreationService
{
    Task AddReport(UpdateReportRequest input, int activeEmployeeId);
    Task UpdateReport(UpdateReportRequest input);
}