using HRIS.Models.Report.Request;
using HRIS.Models.Report.Response;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Interfaces.Reporting;

public interface IDataReportAccessService
{
    Task AddOrUpdateReportAccess(UpdateReportAccessRequest input);
    Task ArchiveReportAccess(int accessId);
    bool InvalidAccessParams(UpdateReportAccessRequest.UpdateReportAccessItemRequest request);
    Task<List<ReportAccessResponse>> GetAccessListForReport(int reportId);
    Task<object> GetReportAccessAvailability(int reportId);
}