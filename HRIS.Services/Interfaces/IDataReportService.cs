using HRIS.Models.DataReport;
using HRIS.Models.DataReport.Request;
using HRIS.Models.Update;

namespace HRIS.Services.Interfaces;

public interface IDataReportService
{
    Task<List<DataReportDto>> GetDataReportList();
    Task<object> GetDataReport(string code);
    Task UpdateReportInput(UpdateReportCustomValue input);
    Task<List<DataReportColumnMenuDto>> GetColumnMenu();
    Task<DataReportColumnsDto?> AddColumnToReport(ReportColumnRequest input);
    Task ArchiveColumnFromReport(int columnId);
    Task EnableColumnFromReport(int columnId);
    Task<DataReportColumnsDto?> MoveColumnOnReport(ReportColumnRequest input);
}