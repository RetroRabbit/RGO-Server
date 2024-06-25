using HRIS.Models;
using HRIS.Models.Report;
using HRIS.Models.Report.Response;
using HRIS.Models.Update;

namespace HRIS.Services.Interfaces.Reporting;

public interface IDataReportService
{
    Task<List<DataReportListResponse>> GetDataReportList(AuthorizeIdentity identity);
    Task<object> GetDataReport(AuthorizeIdentity identity, string code);
    Task UpdateReportInput(UpdateReportCustomValue input);
    Task<bool> IsReportViewOnlyForEmployee(AuthorizeIdentity identity, int reportId);
}