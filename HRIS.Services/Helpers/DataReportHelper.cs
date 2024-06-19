using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces.Helper;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Helpers;

public class DataReportHelper : IDataReportHelper
{
    private readonly IUnitOfWork _db;

    public DataReportHelper(IUnitOfWork db)
    {
        _db = db;
    }

    public async Task<DataReport?> GetReport(string code)
    {
        return await _db.DataReport
            .Get(x => x.Status == ItemStatus.Active && x.Code == code)
            .Include(x => x.DataReportFilter!.Where(y => y.Status == ItemStatus.Active))
            .Include(x => x.DataReportColumns!.Where(y => y.Status == ItemStatus.Active))
            .Include(x => x.DataReportValues)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }

    public async Task<List<int>> GetEmployeeIdListForReport(DataReport report)
    {
        if (report.DataReportFilter == null)
            throw new Exception($"Report '{report.Code}' has no filters");

        var employeeIds = await _db.RawSqlForIntList("SELECT \"id\" FROM \"Employee\"", "id");

        foreach (var g in report.DataReportFilter.GroupBy(x => x.Table))
        {
            var selector = g.FirstOrDefault()?.Select ?? throw new Exception($"No Selector for {g.Key} to filter on");
            var conditions = g.Aggregate(string.Empty,
                (current, dto) => current + $"AND \"{dto.Column}\" {dto.Condition} {dto.Value ?? ""}")[3..];
            var sql = $"SELECT \"{selector}\" FROM \"{g.Key}\" WHERE {conditions}";
            var list = await _db.RawSqlForIntList(sql, selector);
            employeeIds = employeeIds.Where(list.Contains).ToList();
        }

        return employeeIds;
    }

    public async Task<List<Employee>> GetEmployeeData(List<int> employeeIds)
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

    public List<Dictionary<string, object?>> MapEmployeeData(DataReport report, List<Employee> employeeDataList)
    {
        if (report.DataReportColumns == null)
            throw new Exception($"Report '{report.Code}' has no columns");

        var mappingList = report.DataReportColumns;

        var list = new List<Dictionary<string, object?>>();

        foreach (var employee in employeeDataList)
        {
            var customInput = report.DataReportValues?.Where(x => x.EmployeeId == employee.Id).ToList() ??
                              new List<DataReportValues>();
            var dictionary = new Dictionary<string, object?> { { "Id", employee.Id } };
            foreach (var map in mappingList.OrderBy(x => x.Sequence))
            {
                object? value = null;
                if (map.IsCustom)
                    value = map.FieldType switch
                    {
                        DataReportCustom.EmployeeData => employee.EmployeeData
                            ?.Where(x => x.FieldCode?.Code == map.Mapping).FirstOrDefault()?.Value,
                        DataReportCustom.Checkbox => customInput.FirstOrDefault(x => x.ColumnId == map.Id)?.Input ??
                                                     "false",
                        DataReportCustom.Text => customInput.FirstOrDefault(x => x.ColumnId == map.Id)?.Input,
                        _ => value
                    };
                else
                    value = GetValueFromMapping(employee, map.Mapping.Split('.'));

                dictionary.Add(map.Prop, value ?? "");
            }

            list.Add(dictionary);
        }

        return list;
    }

    public object? GetValueFromMapping(object obj, params string[] mappingList)
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

    public List<DataReportColumnsDto> MapReportColumns(DataReport report)
    {
        return report.DataReportColumns?
            .OrderBy(x => x.Sequence)
            .Select(x => x.ToDto())
            .ToList() ?? new List<DataReportColumnsDto>();
    }
}