﻿using System.ComponentModel;
using System.Reflection;
using HRIS.Models.Enums;
using HRIS.Models.Report;
using HRIS.Services.Extensions;
using HRIS.Services.Interfaces.Helper;
using HRIS.Services.Services;
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

    public async Task<List<int>> GetEmployeeIdListForReport(DataReport report)
    {
        if (report.DataReportFilter == null)
            throw new CustomException($"Report '{report.Code}' has no filters");

        var employeeIds = await _db.RawSqlForIntList("SELECT \"id\" FROM \"Employee\"", "id");

        foreach (var g in report.DataReportFilter.GroupBy(x => x.Table))
        {
            var selector = g.FirstOrDefault()?.Select ?? throw new CustomException($"No Selector for {g.Key} to filter on");
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
            throw new CustomException($"Report '{report.Code}' has no columns");

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
                var prop = "";
                switch (map.FieldType)
                {
                    case DataReportColumnType.Employee:
                        if (map.Menu!.FieldCodeId == null)
                        {
                            value = GetValueFromMapping(employee, map.Menu.Mapping!.Split('.'));
                            prop = map.Menu.Prop;
                        }
                        if (map.Menu!.FieldCodeId != null)
                        {
                            value = employee.EmployeeData?.Where(x => x.FieldCode?.Code == map.Menu.FieldCode!.Code)
                                            .FirstOrDefault()?.Value;
                            prop = map.Menu.FieldCode!.Code;
                        }
                        break;
                    case DataReportColumnType.Checkbox:
                        value = customInput.FirstOrDefault(x => x.ColumnId == map.Id)?.Input ?? "false";
                        prop = map.CustomProp;
                        break;
                    case DataReportColumnType.Text:
                        value = customInput.FirstOrDefault(x => x.ColumnId == map.Id)?.Input;
                        prop = map.CustomProp;
                        break;
                }

                prop ??= "";
                if(!dictionary.ContainsKey(prop))
                    dictionary.Add(prop, value ?? "");
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
            try
            {
                var property = currentType.GetProperty(propertyName);
                if (property == null)
                    return null;

                currentType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

                if (currentType.IsEnum)
                {
                    currentObject = property.GetValue(currentObject)?.ToString();
                }
                else if (currentType == typeof(DateTime))
                {
                    currentObject = ((DateTime?)property.GetValue(currentObject))?.ToString("dd-MM-yyyy");
                }
                else
                {
                    currentObject = property.GetValue(currentObject);
                }
            }
            catch
            {
                return null;
            }
        }

        return currentObject;
    }

    public static string GetEnumDescription(Enum? value)
    {
        if (value == null) return null;
        FieldInfo field = value.GetType().GetField(value.ToString());
        DescriptionAttribute attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
        return attribute == null ? value.ToString() : attribute.Description;
    }

    public List<DataReportColumnsDto> MapReportColumns(DataReport report)
    {
        return report.DataReportColumns?
            .OrderBy(x => x.Sequence)
            .Select(x => x.ToDto())
            .ToList() ?? new List<DataReportColumnsDto>();
    }
}