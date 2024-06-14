using System.Collections.Generic;
using System.Diagnostics;
using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Models.Update;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public class DataReportService : IDataReportService
{
    private readonly IUnitOfWork _db;

    public DataReportService(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<List<DataReportDto>> GetDataReportList()
    {
        return await _db.DataReport.Get(x => x.Status == ItemStatus.Active).Select(x => x.ToDto()).ToListAsync();
    }

    public async Task<object> GetDataReport(string code)
    {
        var report = await GetReport(code) ?? throw new Exception($"Report '{code}' not found");

        var employeeIdList = await GetEmployeeIdListForReport(report);

        var employeeDataList = await GetEmployeeData(employeeIdList);

        var mappedEmployeeData = MapEmployeeData(report, employeeDataList);

        var mappedColumns = MapReportColumns(report);

        return new
        {
            ReportId = report.Id,
            Columns = mappedColumns,
            Data = mappedEmployeeData
        };
    }

    public async Task UpdateReportInput(UpdateReportCustomValue input)
    {
        var item = await _db.DataReportValues
            .Get(x => x.ReportId == input.ReportId && x.ColumnId == input.ColumnId && x.EmployeeId == input.EmployeeId)
            .FirstOrDefaultAsync();

        if (item != null)
        {
            item.Input = input.Input;
            await _db.DataReportValues.Update(item);
        }
        else
            await _db.DataReportValues.Add(new DataReportValues
            {
                ReportId = input.ReportId,
                ColumnId = input.ColumnId,
                EmployeeId = input.EmployeeId,
                Input = input.Input
            });

    }

    private static List<DataReportColumnsDto> MapReportColumns(DataReport report)
    {
        return report.DataReportColumns?
            .OrderBy(x => x.Sequence)
            .Select(x => x.ToDto())
            .ToList() ?? new List<DataReportColumnsDto>();
    }

    private static List<Dictionary<string, object?>> MapEmployeeData(DataReport report, List<Employee> employeeDataList)
    {
        if (report.DataReportColumns == null)
            throw new Exception($"Report '{report.Code}' has no columns");

        var mappingList = report.DataReportColumns;

        var list = new List<Dictionary<string, object?>>();

        foreach (var employee in employeeDataList)
        {
            var customInput = report.DataReportValues?.Where(x => x.EmployeeId == employee.Id).ToList() ?? new List<DataReportValues>();
            var dictionary = new Dictionary<string, object?> { { "Id", employee.Id } };
            foreach (var map in mappingList.OrderBy(x => x.Sequence))
            {
                object? value = null;
                if (map.IsCustom)
                {
                    value = map.FieldType switch
                    {
                        DataReportCustom.EmployeeData => employee.EmployeeData?.Where(x => x.FieldCode?.Code == map.Mapping).FirstOrDefault()?.Value,
                        DataReportCustom.Checkbox => customInput.FirstOrDefault(x => x.ColumnId == map.Id)?.Input ?? "false",
                        DataReportCustom.Text => customInput.FirstOrDefault(x => x.ColumnId == map.Id)?.Input,
                        _ => value
                    };
                }
                else
                {
                    value = GetValueFromMapping(employee, map.Mapping.Split('.'));
                }

                dictionary.Add(map.Prop, value ?? "");
            }
            list.Add(dictionary);
        }

        return list;
    }

    private static object? GetValueFromMapping(object obj, params string[] mappingList)
    {
        var currentObject = obj;
        var currentType = obj.GetType();

        foreach (var propertyName in mappingList)
        {
            var property = currentType.GetProperty(propertyName);
            if (property == null)
                return null;

            currentObject = property.GetValue(currentObject);
            currentType = property.PropertyType;
        }

        return currentObject;
    }

    private async Task<List<Employee>> GetEmployeeData(List<int> employeeIds)
    {
        return await _db.Employee
            .Get(x => employeeIds.Contains(x.Id))
            .Include(x => x.PhysicalAddress)
            .Include(x => x.PostalAddress)
            .Include(x => x.EmployeeCertification)
            .Include(x => x.EmployeeDocument)
            .Include(x => x.EmployeeQualification)
            .Include(x => x.EmployeeSalaryDetails)
            .Include(x => x.EmployeeType)
            .Include(x => x.ChampionEmployee)
            .Include(x => x.TeamLeadAssigned)
            .Include(x => x.ClientAssigned)
            .Include(x => x.EmployeeData)!.ThenInclude(y => y.FieldCode)
            .AsNoTracking()
            .ToListAsync();
    }

    private async Task<DataReport?> GetReport(string code)
    {
        return await _db.DataReport
            .Get(x => x.Status == ItemStatus.Active && x.Code == code)
            .Include(x => x.DataReportFilter!.Where(y => y.Status == ItemStatus.Active))
            .Include(x => x.DataReportColumns!.Where(y => y.Status == ItemStatus.Active))
            .Include(x => x.DataReportValues)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    private async Task<List<int>> GetEmployeeIdListForReport(DataReport report)
    {
        if (report.DataReportFilter == null)
            throw new Exception($"Report '{report.Code}' has no filters");

        var employeeIds = await _db.RawSqlForIntList("SELECT \"id\" FROM \"Employee\"", "id");

        foreach (var g in report.DataReportFilter.GroupBy(x => x.Table))
        {
            var selector = g.FirstOrDefault()?.Select;
            var conditions = g.Aggregate(string.Empty, (current, dto) => current + $"AND \"{dto.Column}\" {dto.Condition} {dto.Value ?? ""}")[3..];
            var sql = $"SELECT \"{selector}\" FROM \"{g.Key}\" WHERE {conditions}";
            var list = await _db.RawSqlForIntList(sql, selector);
            employeeIds = employeeIds.Where(list.Contains).ToList();
        }

        return employeeIds;
    }
}