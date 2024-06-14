using HRIS.Models;
using HRIS.Models.Update;

namespace HRIS.Services.Interfaces;

public interface IDataReportService
{
    Task<List<DataReportDto>> GetDataReportList();
    Task<object> GetDataReport(string code);
    Task UpdateReportInput(UpdateReportCustomValue input);
}