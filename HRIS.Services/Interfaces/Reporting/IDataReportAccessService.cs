using HRIS.Models.Report.Request;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Interfaces.Reporting;

public interface IDataReportAccessService
{
    Task AddOrUpdateReportAccess(UpdateReportAccessRequest input);
    Task ArchiveAccessNotFound(DataReport report, UpdateReportAccessRequest input);
    bool InvalidAccessParams(UpdateReportAccessRequest.UpdateReportAccessItemRequest request);
}