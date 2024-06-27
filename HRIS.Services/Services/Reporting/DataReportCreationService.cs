﻿using HRIS.Models.Enums;
using HRIS.Models.Report.Request;
using HRIS.Services.Interfaces.Reporting;
using HRIS.Services.Session;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services.Reporting;

public class DataReportCreationService : IDataReportCreationService
{
    private readonly AuthorizeIdentity _identity;
    private readonly IUnitOfWork _db;

    public DataReportCreationService(AuthorizeIdentity identity, IUnitOfWork db)
    {
        _identity = identity;
        _db = db;
    }

    public async Task AddReport(UpdateReportRequest input)
    {
        var report = await _db.DataReport.Add(new DataReport { Name = input.Name, Code = input.Code, Status = ItemStatus.Active });
        await _db.DataReportAccess.Add(new DataReportAccess { EmployeeId = await _identity.GetEmployeeId(), ReportId = report.Id, ViewOnly = false, Status = ItemStatus.Active });

        var nameMenuItemId = await _db.DataReportColumnMenu.Get(x => x.Status == ItemStatus.Active && x.Prop == "Name").Select(x => x.Id).FirstOrDefaultAsync();
        if(nameMenuItemId > 0)
            await _db.DataReportColumns.Add(new DataReportColumns { ReportId = report.Id, Status = ItemStatus.Active, Sequence = 0, FieldType = DataReportColumnType.Employee, MenuId = nameMenuItemId });

        var surnameMenuItemId = await _db.DataReportColumnMenu.Get(x => x.Status == ItemStatus.Active && x.Prop == "Surname").Select(x => x.Id).FirstOrDefaultAsync();
        if(surnameMenuItemId > 0)
            await _db.DataReportColumns.Add(new DataReportColumns { ReportId = report.Id, Status = ItemStatus.Active, Sequence = 1, FieldType = DataReportColumnType.Employee, MenuId = surnameMenuItemId });
    }

    public async Task UpdateReport(UpdateReportRequest input)
    {
        await _db.DataReport.ConfirmAccessToReport(input.ReportId, _identity.EmployeeId);

        var report = await _db.DataReport.GetReport(input.ReportId) ?? throw new Exception($"Failed to create report");
        report.Name = input.Name;
        report.Code = input.Code;
        await _db.DataReport.Update(report);
    }
}