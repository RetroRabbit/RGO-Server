using HRIS.Models.Enums;
using HRIS.Models.Report;
using HRIS.Models.Report.Request;
using HRIS.Services.Extensions;
using HRIS.Services.Interfaces.Reporting;
using HRIS.Services.Session;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services.Reporting;

public class DataReportControlService : IDataReportControlService
{
    private readonly AuthorizeIdentity _identity;
    private readonly IUnitOfWork _db;
    private readonly IDataReportCreationService _creation;
    private readonly IDataReportFilterService _filter;

    public DataReportControlService(AuthorizeIdentity identity, IUnitOfWork db, IDataReportCreationService creation, IDataReportFilterService filter)
    {
        _identity = identity;
        _db = db;
        _creation = creation;
        _filter = filter;
    }

    public async Task<List<DataReportColumnMenuDto>> GetColumnMenu()
    {
        var employee = await _db.DataReportColumnMenu
            .Get(x => x.Status == ItemStatus.Active && x.ParentId == null)
            .Include(x => x.Children)!
            .ThenInclude(x => x.Children)
            .Include(x => x.FieldCode)
            .OrderBy(x => x.Name)
            .Select(x => x.ToDto())
            .ToListAsync();
        var custom = new List<DataReportColumnMenuDto>
        {
            new()
            {
                Name = "Text",
                Prop = "Text"
            },
            new()
            {
                Name = "CheckBox",
                Prop = "CheckBox"
            }
        };

        var menu = new List<DataReportColumnMenuDto>
        {
            new()
            {
                Name = "Employee",
                Children = employee,
                Prop = "Employee"
            },
            new()
            {
                Name = "Custom",
                Prop = "Custom",
                Children = custom
            }
        };

        return new List<DataReportColumnMenuDto>
        {
            new()
            {
                Name = "+",
                Children = menu,
                Prop = "+"
            },
        };
    }

    public async Task<DataReportColumnsDto?> AddColumnToReport(ReportColumnRequest input)
    {
        await _db.DataReport.ConfirmEditAccess(input.ReportId, _identity.EmployeeId);

        var report = await _db.DataReport.GetReport(input.ReportId);

        if (report == null)
            throw new CustomException("The report does not seem to exist.");

        var entity = await _db.DataReportColumns
            .Get(x => x.ReportId == report.Id && (x.MenuId == input.MenuId ||
                                                  x.MenuId == null && x.FieldType == input.GetColumnType() &&
                                                  x.CustomName == input.Name))
            .FirstOrDefaultAsync();

        if (entity is { Status: ItemStatus.Active })
            throw new CustomException("The column already exist in the table.");

        var menu = await _db.DataReportColumnMenu
            .Get(x => x.Id == input.MenuId)
            .Include(x => x.FieldCode)
            .FirstOrDefaultAsync();

        if (entity is { Status: ItemStatus.Archive })
        {
            entity.Status = ItemStatus.Active;
            entity.Menu = menu;
            return (await _db.DataReportColumns.Update(entity)).ToDto();
        }

        var columnType = input.GetColumnType();

        entity = new DataReportColumns
        {
            Id = 0,
            MenuId = columnType == DataReportColumnType.Employee ? input.MenuId : null,
            FieldType = columnType,
            CustomName = columnType == DataReportColumnType.Employee ? null : input.Name,
            CustomProp = columnType == DataReportColumnType.Employee ? null : input.Name?.StripToKeyWord(),
            ReportId = input.ReportId,
            Sequence = report.DataReportColumns?.Count ?? 0,
            Status = ItemStatus.Active,
            Menu = menu
        };
        return (await _db.DataReportColumns.Add(entity)).ToDto();
    }

    public async Task ArchiveColumnFromReport(int columnId)
    {
        var column = await _db.DataReportColumns
            .Get(x => x.Id == columnId && x.Status == ItemStatus.Active)
            .Include(x => x.Menu)
                .ThenInclude(y => y!.FieldCode)
        .FirstOrDefaultAsync();

        if (column == null)
            throw new CustomException("Could not locate column to archive.");

        await _db.DataReport.ConfirmEditAccess(column.ReportId, _identity.EmployeeId);

        column.Status = ItemStatus.Archive;
        await _db.DataReportColumns.Update(column);
    }

    public async Task<DataReportColumnsDto?> MoveColumnOnReport(ReportColumnRequest input)
    {
        await _db.DataReport.ConfirmEditAccess(input.ReportId, _identity.EmployeeId);

        var report = await _db.DataReport.GetReport(input.ReportId);

        if (report == null)
            throw new CustomException("The report does not seem to exist.");

        if (report.DataReportColumns == null)
            throw new CustomException($"Report '{report.Code}' has no columns");

        var column = report.DataReportColumns.FirstOrDefault(x => x.Id == input.Id);

        if (column == null)
            throw new CustomException("Could not locate column to update.");

        if (input.Sequence < 0 || input.Sequence > report.DataReportColumns.Count - 1)
            throw new CustomException("Column order is out of range");

        var sequence = 0;
        foreach (var c in report.DataReportColumns.OrderBy(x => x.Sequence))
        {
            c.Sequence = sequence++;

            if (c.Sequence == input.Sequence)
                c.Sequence = sequence++;

            if (c.Id != input.Id) continue;
            c.Sequence = input.Sequence;
            sequence--;
        }

        foreach (var c in report.DataReportColumns)
        {
            await _db.DataReportColumns.Update(c);
        }

        return column.ToDto();
    }

    public async Task AddOrUpdateReport(UpdateReportRequest input)
    {
        if (input.ReportId > 0)
            await _creation.UpdateReport(input);
        else
            await _creation.AddReport(input);
    }

    public async Task AddOrUpdateReportFilter(ReportFilterRequest input)
    {
        if (input.ReportFilterId > 0)
            await _filter.UpdateReportFilter(input);
        else
            await _filter.AddReportFilter(input);
    }

    public async Task DeleteReportFilterfromList(int id)
    {
        await _filter.ArchiveReportFilter(id) ;
    }

    
}