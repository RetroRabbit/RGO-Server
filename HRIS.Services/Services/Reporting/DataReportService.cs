using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Models.Report;
using HRIS.Models.Report.Response;
using HRIS.Models.Update;
using HRIS.Services.Extensions;
using HRIS.Services.Interfaces.Reporting;
using HRIS.Services.Interfaces.Helper;
using HRIS.Services.Session;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services.Reporting;

public class DataReportService : IDataReportService
{
    private readonly AuthorizeIdentity _identity;
    private readonly IUnitOfWork _db;
    private readonly IDataReportHelper _helper;
    private readonly IDataReportAccessService _access;

    public DataReportService(AuthorizeIdentity identity, IUnitOfWork db, IDataReportHelper helper, IDataReportAccessService access)
    {
        _identity = identity;
        _db = db;
        _helper = helper;
        _access = access;
    }

    public async Task<List<DataReportListResponse>> GetDataReportList()
    {
        return await _db.DataReport.GetReportsForEmployee(_identity.Email) ?? throw new Exception("Not reports found");
    }

    public async Task<object> GetDataReport(string code)
    {
        var report = await _db.DataReport.GetReport(code) ?? throw new Exception($"Report '{code}' not found");

        await _db.DataReport.ConfirmAccessToReport(report.Id, _identity.EmployeeId);

        var viewOnly = await IsReportViewOnlyForEmployee(report.Id);

        var employeeIdList = await _helper.GetEmployeeIdListForReport(report);
        var employeeDataList = await _helper.GetEmployeeData(employeeIdList);
        var mappedEmployeeData = _helper.MapEmployeeData(report, employeeDataList);
        var mappedColumns = _helper.MapReportColumns(report);
        var accessList = new List<ReportAccessResponse>();
        
        if (false == viewOnly)
            accessList = await _access.GetAccessListForReport(report.Id);

        return new
        {
            ReportCode = report.Code,
            ReportName = report.Name,
            ReportId = report.Id,
            ViewOnly = viewOnly,
            Columns = mappedColumns,
            Data = mappedEmployeeData,
            AccessList = accessList
        };
    }

    public async Task<bool> IsReportViewOnlyForEmployee(int reportId)
    {
        var employeeId = await _identity.GetEmployeeId();

        var roles = await _db.EmployeeRole.Get(r => r.EmployeeId == employeeId).Select(r => r.RoleId).ToListAsync();

        var access = await _db.DataReportAccess
            .Get(a => roles.Contains(a.RoleId ?? 0) || a.EmployeeId == employeeId)
            .ToListAsync();

        var roleAccess = access.Any(x => x is { RoleId: not null, ViewOnly: true });
        var specificAccess = access.Any(x => x is { EmployeeId: not null, ViewOnly: true });

        return specificAccess || roleAccess;
    }

    public async Task UpdateReportInput(UpdateReportCustomValue input)
    {
        await _db.DataReport.ConfirmAccessToReport(input.ReportId, _identity.EmployeeId);

        var item = await _db.DataReportValues
            .FirstOrDefault(x =>
                x.ReportId == input.ReportId && x.ColumnId == input.ColumnId && x.EmployeeId == input.EmployeeId);

        if (item != null)
        {
            item.Input = input.Input;
            await _db.DataReportValues.Update(item);
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