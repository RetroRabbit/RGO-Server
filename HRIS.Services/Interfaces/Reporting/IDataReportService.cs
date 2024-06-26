using HRIS.Models;
using HRIS.Models.Report;
using HRIS.Models.Report.Response;
using HRIS.Models.Update;

namespace HRIS.Services.Interfaces.Reporting;

public interface IDataReportService
{
    Task<List<DataReportListResponse>> GetDataReportList();
    Task<object> GetDataReport(string code);
    Task UpdateReportInput(UpdateReportCustomValue input);
    Task<bool> IsReportViewOnlyForEmployee(int reportId);
}