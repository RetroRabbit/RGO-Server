using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Models.Update;
using HRIS.Services.Interfaces;
using HRIS.Services.Interfaces.Helper;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class DataReportService : IDataReportService
{
    private readonly IUnitOfWork _db;
    private readonly IDataReportHelper _helper;

    public DataReportService(IUnitOfWork db, IDataReportHelper helper)
    {
        _db = db;
        _helper = helper;
    }

    public async Task<List<DataReportDto>> GetDataReportList()
    {
        return await _db.DataReport.GetAll(x => x.Status == ItemStatus.Active);
    }

    public async Task<object> GetDataReport(string code)
    {
        var report = await _helper.GetReport(code) ?? throw new Exception($"Report '{code}' not found");

        var employeeIdList = await _helper.GetEmployeeIdListForReport(report);

        var employeeDataList = await _helper.GetEmployeeData(employeeIdList);

        var mappedEmployeeData = _helper.MapEmployeeData(report, employeeDataList);

        var mappedColumns = _helper.MapReportColumns(report);

        return new
        {
            ReportName = report.Name,
            ReportId = report.Id,
            Columns = mappedColumns,
            Data = mappedEmployeeData
        };
    }

    public async Task UpdateReportInput(UpdateReportCustomValue input)
    {
        var item = await _db.DataReportValues
            .FirstOrDefault(x => x.ReportId == input.ReportId && x.ColumnId == input.ColumnId && x.EmployeeId == input.EmployeeId);

        if (item != null)
        {
            item.Input = input.Input;
            await _db.DataReportValues.Update(new DataReportValues(item));
            return;
        }

        await _db.DataReportValues.Add(new DataReportValues
        {
            ReportId = input.ReportId,
            ColumnId = input.ColumnId,
            EmployeeId = input.EmployeeId,
            Input = input.Input
        });
    }
}