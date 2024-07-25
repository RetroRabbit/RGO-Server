using HRIS.Models.Report.Response;
using HRIS.Models.Update;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Interfaces.Reporting;

public interface IDataReportService
{
    Task<List<DataReportListResponse>> GetDataReportList();
    Task<DataReport> DeleteReportfromList(string code);
    Task<object> GetDataReport(string code);
    Task<object> GetDataReportFilters(string code);
    Task UpdateReportInput(UpdateReportCustomValue input);
    Task<bool> IsReportViewOnlyForEmployee(int reportId);
}