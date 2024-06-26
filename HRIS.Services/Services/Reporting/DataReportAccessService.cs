using HRIS.Models.Enums;
using HRIS.Models.Report.Request;
using HRIS.Services.Interfaces.Helper;
using HRIS.Services.Interfaces.Reporting;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services.Reporting;

public class DataReportAccessService : IDataReportAccessService
{
    private readonly IUnitOfWork _db;

    public DataReportAccessService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task AddOrUpdateReportAccess(UpdateReportAccessRequest input)
    {
        var report = await _db.DataReport.GetReport(input.ReportId) ?? throw new Exception("The report does not seem to exist.");

        report.DataReportAccess ??= new List<DataReportAccess>();

        //await ArchiveAccessNotFound(report, input);

        foreach (var request in input.Access)
        {
            if (InvalidAccessParams(request))
                continue;

            var a = report.DataReportAccess.FirstOrDefault(x => x.EmployeeId == request.EmployeeId || x.RoleId == request.RoleId);
            if (a == null)
            {
                await _db.DataReportAccess.Add(new DataReportAccess
                {
                    EmployeeId = request.EmployeeId,
                    RoleId = request.RoleId,
                    ReportId = report.Id,
                    ViewOnly = request.ViewOnly,
                    Status = ItemStatus.Active
                });
                continue;
            }

            a.EmployeeId = request.EmployeeId;
            a.RoleId = request.RoleId;
            a.ViewOnly = request.ViewOnly;
            a.Status = ItemStatus.Active;
            await _db.DataReportAccess.Update(a);
        }
    }

    public bool InvalidAccessParams(UpdateReportAccessRequest.UpdateReportAccessItemRequest request)
    {
        return request is { EmployeeId: null, RoleId: null } or { EmployeeId: not null, RoleId: not null };
    }

    public async Task ArchiveAccessNotFound(DataReport report, UpdateReportAccessRequest input)
    {
        foreach (var access in report.DataReportAccess ?? new List<DataReportAccess>())
        {
            var a = input.Access.FirstOrDefault(x => x.EmployeeId == access.EmployeeId || x.RoleId == access.RoleId);
            if (a != null) continue;
            access.Status = ItemStatus.Archive;
            await _db.DataReportAccess.Update(access);
        }
    }
}