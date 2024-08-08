using HRIS.Models.Report.Request;

namespace HRIS.Services.Interfaces.Reporting
{
    public interface IDataReportFilterService
    {
        Task<object> AddReportFilter(ReportFilterRequest input);
        Task UpdateReportFilter(ReportFilterRequest input);
        Task ArchiveReportFilter(int id);
        Task<object> GetDataReportFilters(string code);
    }
}
