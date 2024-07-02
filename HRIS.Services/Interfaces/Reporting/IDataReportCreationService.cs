using HRIS.Models.Report.Request;

namespace HRIS.Services.Interfaces.Reporting;

public interface IDataReportCreationService
{
    Task AddReport(UpdateReportRequest input);
    Task UpdateReport(UpdateReportRequest input);
}