using HRIS.Models.Enums;
using HRIS.Models.Report.Request;
using HRIS.Models.Report.Response;
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

    public async Task<List<ReportAccessResponse>> GetAccessListForReport(int reportId)
    {
        var access = (await _db.DataReportAccess
            .Get(x => x.ReportId == reportId && x.Status == ItemStatus.Active)
            .AsNoTracking()
            .ToListAsync());

        var employeeIdList = access.Where(x => x.EmployeeId.HasValue).Select(x => x.EmployeeId).ToList();

        var roles = await _db.Role.GetAll();
        var employees = await _db.Employee.Get(x => employeeIdList.Contains(x.Id)).ToListAsync();

        return (from a in access
            let emp = employees.FirstOrDefault(x => x.Id == a.EmployeeId)
            let role = roles.FirstOrDefault(x => x.Id == a.RoleId)
            select new ReportAccessResponse
            {
                Id = a.Id,
                EmployeeId = a.EmployeeId,
                RoleId = a.RoleId,
                ViewOnly = a.ViewOnly,
                Name = a.EmployeeId.HasValue ? $"{emp?.Name} {emp?.Surname}" : $"{role?.Description}"
            }).ToList();
    }

    public async Task ArchiveReportAccess(int accessId)
    {
        var a = await _db.DataReportAccess.Get(x => x.Id == accessId).FirstOrDefaultAsync();
        if(a == null)
            return;
        a.Status = ItemStatus.Archive;
        await _db.DataReportAccess.Update(a);
    }
}