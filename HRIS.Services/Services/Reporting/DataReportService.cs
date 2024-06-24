using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Models.Report;
using HRIS.Models.Report.Response;
using HRIS.Models.Update;
using HRIS.Services.Extensions;
using HRIS.Services.Interfaces;
using HRIS.Services.Interfaces.Reporting;
using HRIS.Services.Interfaces.Helper;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services.Reporting;

public class DataReportService : IDataReportService
{
    private readonly IUnitOfWork _db;
    private readonly IDataReportHelper _helper;
    private readonly IEmployeeRoleService _employeeService;

    public DataReportService(IUnitOfWork db, IDataReportHelper helper, IEmployeeRoleService employeeService)
    {
        _db = db;
        _helper = helper;
        _employeeService = employeeService;
    }

    public async Task<List<DataReportListResponse>> GetDataReportList(AuthorizeIdentity identity)
    {
        return await _db.DataReport.GetReportsForEmployee(identity.Email) ?? throw new Exception("Not reports found");
    }

    public async Task<object> GetDataReport(string code)
    {
        var report = await _db.DataReport.GetReport(code) ?? throw new Exception($"Report '{code}' not found");
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
            .FirstOrDefault(x =>
                x.ReportId == input.ReportId && x.ColumnId == input.ColumnId && x.EmployeeId == input.EmployeeId);

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