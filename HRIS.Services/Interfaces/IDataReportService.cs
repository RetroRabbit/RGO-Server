using HRIS.Models;

namespace HRIS.Services.Interfaces;

public interface IDataReportService
{
    Task<List<DataReportDto>> GetDataReportList();
    Task<List<Dictionary<string, object?>>> GetDataReport(string code);
}