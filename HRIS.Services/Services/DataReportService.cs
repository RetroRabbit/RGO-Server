using HRIS.Models.DataReport;
using HRIS.Models.DataReport.Request;
using HRIS.Models.Enums;
using HRIS.Models.Update;
using HRIS.Services.Extensions;
using HRIS.Services.Interfaces;
using HRIS.Services.Interfaces.Helper;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using RR.UnitOfWork.Migrations;

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

    public async Task<List<DataReportColumnMenuDto>> GetColumnMenu()
    {
        return await _db.DataReportColumnMenu
                        .Get(x => x.Status == ItemStatus.Active && x.ParentId == null)
                        .Include(x => x.Children)!
                            .ThenInclude(x => x.Children)!
                        .Include(x => x.FieldCode)
                        .OrderBy(x => x.Name)
                        .Select(x => x.ToDto())
                        .ToListAsync();
    }

    public async Task<DataReportColumnsDto?> AddColumnToReport(ReportColumnRequest input)
    {
        var report = await _helper.GetReport(input.ReportId);

        if (report == null)
            throw new Exception("The report does not seem to exist.");

        var existingColumn = await _db.DataReportColumns.Any(x => x.MenuId == input.MenuId || (x.MenuId == null && x.FieldType == input.GetColumnType() && x.CustomName == input.Name));

        if (existingColumn)
            throw new Exception("The column already exist in the table.");

        return await _db.DataReportColumns.Add(new DataReportColumns
        {
            Id = 0,
            MenuId = input.MenuId,
            FieldType = input.GetColumnType(),
            CustomName = input.Name,
            CustomProp = input.Name?.StripToKeyWord(),
            ReportId = input.ReportId,
            Sequence = report.DataReportColumns?.Count ?? 0,
            Status = ItemStatus.Active
        });
    }

    public async Task ArchiveColumnFromReport(int columnId)
    {
        var column = await _db.DataReportColumns
            .Get(x => x.Id == columnId && x.Status == ItemStatus.Active)
            .Include(x => x.Menu)
            .ThenInclude(y => y.FieldCode)
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
        var report = await _helper.GetReport(input.ReportId);

        if (report == null)
            throw new Exception("The report does not seem to exist.");

        if (report.DataReportColumns == null)
            throw new Exception($"Report '{report.Code}' has no columns");

        var column = report.DataReportColumns.FirstOrDefault(x => x.Id == input.Id);

        if (column == null)
            throw new Exception("Could not locate column to update.");

        if(input.Sequence < 0 || input.Sequence > report.DataReportColumns.Count - 1)
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
}