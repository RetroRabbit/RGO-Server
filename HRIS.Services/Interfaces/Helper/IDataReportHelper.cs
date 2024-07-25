using HRIS.Models.Enums;
using HRIS.Models.Report;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Interfaces.Helper;

public interface IDataReportHelper
{
    Task<List<int>> GetEmployeeIdListForReport(DataReport report);
    Task<List<Employee>> GetEmployeeData(List<int> employeeIds);
    List<Dictionary<string, object?>> MapEmployeeData(DataReport report, List<Employee> employeeDataList);
    object? GetValueFromMapping(object obj, params string[] mappingList);
    List<DataReportColumnsDto> MapReportColumns(DataReport report);
    List<DataReportFilterDto> GetDataReportFilter(DataReport report);
}