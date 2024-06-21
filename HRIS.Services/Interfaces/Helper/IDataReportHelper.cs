using HRIS.Models.Enums;
using RR.UnitOfWork.Entities.HRIS;
using HRIS.Models.DataReport;

namespace HRIS.Services.Interfaces.Helper;

public interface IDataReportHelper
{
    Task<DataReport?> GetReport(int id);
    Task<DataReport?> GetReport(string code);
    Task<List<int>> GetEmployeeIdListForReport(DataReport report);
    Task<List<Employee>> GetEmployeeData(List<int> employeeIds);
    List<Dictionary<string, object?>> MapEmployeeData(DataReport report, List<Employee> employeeDataList);
    object? GetValueFromMapping(object obj, params string[] mappingList);
    List<DataReportColumnsDto> MapReportColumns(DataReport report);
}