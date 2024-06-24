using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Models.Report;
using HRIS.Models.Report.Request;
using HRIS.Models.Update;
using HRIS.Services.Extensions;
using HRIS.Services.Interfaces.Reporting;
using HRIS.Services.Interfaces.Helper;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services.Reporting;

public class DataReportControlService : IDataReportControlService
{
    private readonly IUnitOfWork _db;
    private readonly IDataReportHelper _helper;
    private readonly IDataReportCreationService _creation;

    public DataReportControlService(IUnitOfWork db, IDataReportHelper helper, IDataReportCreationService creation)
    {
        _db = db;
        _helper = helper;
        _creation = creation;
    }

    public async Task<List<DataReportColumnMenuDto>> GetColumnMenu()
    {
        var employee = await _db.DataReportColumnMenu
            .Get(x => x.Status == ItemStatus.Active && x.ParentId == null)
            .Include(x => x.Children)!
            .ThenInclude(x => x.Children)!
            .Include(x => x.FieldCode)
            .OrderBy(x => x.Name)
            .Select(x => x.ToDto())
            .ToListAsync();
        var custom = new List<DataReportColumnMenuDto>
        {
            new DataReportColumnMenuDto
            {
                Name = "Text",
                Prop = "Text"
            },
            new DataReportColumnMenuDto
            {
                Name = "CheckBox",
                Prop = "CheckBox"
            }
        };

        var menu = new List<DataReportColumnMenuDto>
        {
            new DataReportColumnMenuDto
            {
                Name = "Employee",
                Children = employee,
                Prop = "Employee"
            },
            new DataReportColumnMenuDto
            {
                Name = "Custom",
                Prop = "Custom",
                Children = custom
            }
        };

        return new List<DataReportColumnMenuDto>
        {
            new DataReportColumnMenuDto
            {
                Name = "+",
                Children = menu,
                Prop = "+"
            },
        };
    }

    public async Task<DataReportColumnsDto?> AddColumnToReport(ReportColumnRequest input)
    {
        var report = await _db.DataReport.GetReport(input.ReportId);

        if (report == null)
            throw new Exception("The report does not seem to exist.");

        var entity = await _db.DataReportColumns
            .Get(x => x.MenuId == input.MenuId || x.MenuId == null && x.FieldType == input.GetColumnType() && x.CustomName == input.Name)
            .FirstOrDefaultAsync();

        if (entity != null && entity.Status == ItemStatus.Active)
            throw new Exception("The column already exist in the table.");

        var menu = await _db.DataReportColumnMenu
            .Get(x => x.Id == input.MenuId)
            .Include(x => x.FieldCode)
            .FirstOrDefaultAsync();

        if (entity != null && entity.Status == ItemStatus.Archive)
        {
            entity.Status = ItemStatus.Active;
            entity.Menu = menu;
            return await _db.DataReportColumns.Update(entity);
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
        return await _db.DataReportColumns.Add(entity);
    }

    public async Task ArchiveColumnFromReport(int columnId)
    {
        var column = await _db.DataReportColumns
            .Get(x => x.Id == columnId && x.Status == ItemStatus.Active)
            .Include(x => x.Menu)
                .ThenInclude(y => y!.FieldCode)
            .FirstOrDefaultAsync();

        if (column == null)
            throw new Exception("Could not locate column to archive.");

        column.Status = ItemStatus.Archive;
        await _db.DataReportColumns.Update(column);
    }

    public async Task EnableColumnFromReport(int columnId)
    {
        var column = await _db.DataReportColumns
            .Get(x => x.Id == columnId && x.Status == ItemStatus.Archive)
            .Include(x => x.Menu)
            .ThenInclude(y => y.FieldCode)
            .FirstOrDefaultAsync();

        if (column == null)
            throw new Exception("Could not locate column to enable.");

        column.Status = ItemStatus.Active;
        await _db.DataReportColumns.Update(column);
    }

    public async Task<DataReportColumnsDto?> MoveColumnOnReport(ReportColumnRequest input)
    {
        var report = await _db.DataReport.GetReport(input.ReportId);

        if (report == null)
            throw new Exception("The report does not seem to exist.");

        if (report.DataReportColumns == null)
            throw new Exception($"Report '{report.Code}' has no columns");

        var column = report.DataReportColumns.FirstOrDefault(x => x.Id == input.Id);

        if (column == null)
            throw new Exception("Could not locate column to update.");

        if (input.Sequence < 0 || input.Sequence > report.DataReportColumns.Count - 1)
            throw new Exception("Column order is out of range");

        var sequence = 0;
        foreach (var c in report.DataReportColumns.OrderBy(x => x.Sequence))
        {
            c.Sequence = sequence++;

            if (c.Sequence == input.Sequence)
                c.Sequence = sequence++;

            if (c.Id == input.Id)
            {
                c.Sequence = input.Sequence;
                sequence--;
            }
        }

        foreach (var c in report.DataReportColumns)
        {
            await _db.DataReportColumns.Update(column);
        }

        return column.ToDto();
    }

    public async Task AddOrUpdateReport(UpdateReportRequest input, AuthorizeIdentity identity)
    {
        if (input.ReportId > 0)
            await _creation.UpdateReport(input);
        else
            await _creation.AddReport(input, await _db.GetActiveEmployeeId(identity));
    }
}