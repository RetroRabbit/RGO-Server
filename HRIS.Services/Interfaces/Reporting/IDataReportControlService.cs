using HRIS.Models;
using HRIS.Models.Report;
using HRIS.Models.Report.Request;

namespace HRIS.Services.Interfaces.Reporting;

public interface IDataReportControlService
{
    Task<List<DataReportColumnMenuDto>> GetColumnMenu();
    Task<DataReportColumnsDto?> AddColumnToReport(ReportColumnRequest input);
    Task ArchiveColumnFromReport(int columnId);
    Task EnableColumnFromReport(int columnId);
    Task<DataReportColumnsDto?> MoveColumnOnReport(ReportColumnRequest input);
    Task AddOrUpdateReport(UpdateReportRequest input, AuthorizeIdentity identity);
}