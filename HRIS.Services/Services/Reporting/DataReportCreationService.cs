using HRIS.Models.Enums;
using HRIS.Models.Report.Request;
using HRIS.Services.Interfaces.Reporting;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services.Reporting;

public class DataReportCreationService : IDataReportCreationService
{
    private readonly IUnitOfWork _db;

    public DataReportCreationService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task AddReport(UpdateReportRequest input, int activeEmployeeId)
    {
        var report = await _db.DataReport.Add(new DataReport { Name = input.Name, Code = input.Code, Status = ItemStatus.Active });
        await _db.DataReportAccess.Add(new DataReportAccess { EmployeeId = activeEmployeeId, ReportId = report.Id, ViewOnly = false, Status = ItemStatus.Active });
    }

    public async Task UpdateReport(UpdateReportRequest input)
    {
        var report = await _db.DataReport.GetReport(input.ReportId) ?? throw new Exception($"Failed to create report");
        report.Name = input.Name;
        report.Code = input.Code;
        await _db.DataReport.Update(report);
    }
}