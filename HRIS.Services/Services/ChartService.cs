using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HRIS.Services.Services;

public partial class ChartService : IChartService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeService _employeeService;
    private readonly IServiceProvider _services;
    private readonly IErrorLoggingService _errorLoggingService;

    public ChartService(IUnitOfWork db, IEmployeeService employeeService, IServiceProvider services, IErrorLoggingService errorLoggingService)
    {
        _db = db;
        _employeeService = employeeService;
        _services = services;
        _errorLoggingService = errorLoggingService;
    }

    public async Task<List<ChartDto>> GetAllCharts()
    {
        return await _db.Chart.Get().Include(chart => chart.Datasets).Select(c => c.ToDto()).ToListAsync();
    }

    public async Task<ChartDto> CreateChart(List<string> dataTypes, List<string> roles, string chartName,
                                            string chartType)
    {
        List<EmployeeDto> employees;

        var roleList = roles.SelectMany(item => item.Split(',')).ToList();

        for (int i = 0; i < dataTypes.Count; i++)
        {
            dataTypes[i] = AllSpaces().Replace(dataTypes[i], "");
        }

        var dataTypeList = dataTypes.SelectMany(item => item.Split(',')).ToList();

        var chart = new Chart
        {
            Name = chartName,
            DataTypes = dataTypes,
            Datasets = new List<ChartDataSet>()
        };

        if (chartType.ToUpper() == "STACKED")
        {

            employees = await _db.Employee
                                     .Get(employee => roleList.Contains(employee.EmployeeType!.Name!))
                                     .Include(employee => employee.EmployeeType)
                                     .Select(employee => employee.ToDto())
                                     .AsNoTracking()
                                     .ToListAsync();

            var allRoleNames = new[] { "Developer", "Designer", "Scrum Master", "Support Staff" };

            var employeeGroups = allRoleNames.ToDictionary(
                roleName => roleName,
                _ => new List<EmployeeDto>()
            );

            foreach (var employee in employees)
            {
                var roleName = employee.EmployeeType!.Name.ToUpper();
                switch (roleName)
                {
                    case "DEVELOPER":
                        employeeGroups["Developer"].Add(employee);
                        break;
                    case "DESIGNER":
                        employeeGroups["Designer"].Add(employee);
                        break;
                    case "SCRUM MASTER":
                        employeeGroups["Scrum Master"].Add(employee);
                        break;
                    default:
                        employeeGroups["Support Staff"].Add(employee);
                        break;
                }
            }

            var roleDictionaries = employeeGroups.ToDictionary(
                pair => pair.Key,
                pair => CreateGraphDataDictionary(pair.Value, dataTypeList)
            );

            var labels = roleDictionaries.Values.SelectMany(dict => dict.Keys).Distinct().OrderBy(label => label).ToList();
            
            foreach (var dictionary in roleDictionaries.Values)
            {
                foreach (var label in labels)
                {
                    if (!dictionary.ContainsKey(label))
                    {
                        dictionary[label] = 0;
                    }
                }
            }

            foreach (var rolePair in roleDictionaries)
            {
                var dataSet = new ChartDataSet
                {
                    Label = rolePair.Key,
                    Data = labels.Select(label => rolePair.Value.ContainsKey(label) ? rolePair.Value[label] : 0).ToList()
                };
                chart.Datasets.Add(dataSet);
            }

            chart.Labels = labels;
            chart.Type = "bar";
            chart.Subtype = "stacked";

        }
        else
        {
            if (roleList[0] == "All")
                employees = await _employeeService.GetAll();
            else
                employees = await _db.Employee
                                     .Get(employee => roleList.Contains(employee.EmployeeType!.Name!))
                                     .Include(employee => employee.EmployeeType)
                                     .Select(employee => employee.ToDto())
                                     .AsNoTracking()
                                     .ToListAsync();

            var dataDictionary = CreateGraphDataDictionary(employees, dataTypeList);
            var labels = dataDictionary.Keys.ToList();
            var data = dataDictionary.Values.ToList();

            var chartDataSet = new ChartDataSet
            {
                Label = dataTypes[0],
                Data = data
            };

            chart.Labels = labels;
            chart.Type = chartType;
            chart.Subtype = "standard";
            chart.Datasets = new List<ChartDataSet> { chartDataSet };
        }
        return await _db.Chart.Add(chart);
    }

    public async Task<ChartDataDto> GetChartData(List<string> dataTypes)
    {
        var employees = await _employeeService.GetAll();
        var dataTypeList = dataTypes.SelectMany(item => item.Split(',')).ToList();
        var dataDictionary = employees
                             .GroupBy(employee =>
                             {
                                 var keyBuilder = new StringBuilder();
                                 foreach (var dataType in dataTypeList)
                                 {
                                     var propertyInfo = typeof(EmployeeDto).GetProperty(dataType);
                                     if (propertyInfo == null)
                                         continue;

                                     var value = propertyInfo.GetValue(employee);

                                     if (value == null)
                                         continue;

                                     keyBuilder.Append(propertyInfo.GetValue(employee));
                                 }

                                 return keyBuilder.ToString();
                             })
                             .Where(x => string.IsNullOrWhiteSpace(x.Key) == false)
                             .ToDictionary(group => group.Key ?? "Unknown", group => group.Count());

        var labels = dataDictionary.Keys.ToList();
        var data = dataDictionary.Values.ToList();
        var chartDataDto = new ChartDataDto
        {
            Labels = labels,
            Data = data
        };
        return chartDataDto;
    }

    public async Task<ChartDto> DeleteChart(int chartId)
    {
        return await _db.Chart.Delete(chartId);
    }

    public async Task<ChartDto> UpdateChart(ChartDto chartDto)
    {
        for (int i = 0; i < chartDto.DataTypes!.Count; i++)
        {
            chartDto.DataTypes![i] = AllSpaces().Replace(chartDto.DataTypes![i], "");
        }

        var charts = await _db.Chart.GetAll();
        var chartData = charts
                        .Where(chartData => chartData.Id == chartDto.Id)
                        .Select(chartData => chartData)
                        .FirstOrDefault();
        if (chartData == null)
        {
            var exception = new Exception("No chart data record found");
            throw _errorLoggingService.LogException(exception);
        }
        var updatedChart = await _db.Chart.Update(new Chart(chartDto));

        return updatedChart;
    }

    public string[] GetColumnsFromTable()
    {
        var entityType = typeof(Employee);
        var quantifiableColumnNames = entityType.GetProperties()
                                                .Where(p => IsQuantifiableType(p.PropertyType) &&
                                                            !p.Name.Equals("Id") &&
                                                            !p.Name.Equals("EmployeeTypeId") &&
                                                            !p.Name.Equals("PhysicalAddressId") &&
                                                            !p.Name.Equals("PostalAddressId") &&
                                                            !p.Name.Equals("ClientAllocated") &&
                                                            !p.Name.Equals("TeamLead") &&
                                                            !p.Name.Equals("SalaryDays") &&
                                                            !p.Name.Equals("DateOfBirth") &&
                                                            !p.Name.Equals("EngagementDate") &&
                                                            !p.Name.Equals("LeaveInterval") &&
                                                            !p.Name.Equals("PassportExpirationDate") &&
                                                            !p.Name.Equals("PayRate") &&
                                                            !p.Name.Equals("Salary") &&
                                                            !p.Name.Equals("TerminationDate"))
                                                .Select(p => p.Name)
                                                .ToArray();
        quantifiableColumnNames = quantifiableColumnNames.Concat(new[] { "Age" }).ToArray();
        return quantifiableColumnNames;
    }

    public async Task<byte[]?> ExportCsvAsync(List<string> dataTypes)
    {
        var employees = await _db.Employee.GetAll();
        var dataTypeList = dataTypes.SelectMany(item => item.Split(',')).ToList();
        var propertyNames = new List<string>();

        if (dataTypeList.Contains("Age"))
            propertyNames.Add("Age");

        foreach (var typeName in dataTypeList)
        {
            if (typeName == "Age")
                continue;

            var propertyInfo = typeof(EmployeeDto).GetProperty(typeName);

            if (propertyInfo == null)
            {
                var exception = new Exception($"Invalid property name: {typeName}");
                throw _errorLoggingService.LogException(exception);
            }

            propertyNames.Add(typeName);
        }

        var csvData = new StringBuilder();
        csvData.Append("First Name,Last Name");

        foreach (var propertyName in propertyNames) csvData.Append("," + propertyName);
        csvData.AppendLine();

        foreach (var employee in employees)
        {
            var formattedData = $"{employee.Name},{employee.Surname}";
            foreach (var dataType in propertyNames)
                if (BaseDataType.HasCustom(dataType))
                {
                    var obj = BaseDataType.GetCustom(dataType);
                    var val = obj.GenerateData(employee, _services);

                    if (val != null)
                        formattedData += $",{val.Replace(",", "").Trim()}";
                }
                else
                {
                    var propertyInfo = typeof(EmployeeDto).GetProperty(dataType);
                    if (propertyInfo != null)
                    {
                        var val = propertyInfo.GetValue(employee);

                        if (val != null)
                            formattedData += $",{val.ToString()!.Replace(",", "").Trim()}";
                    }
                }

            csvData.AppendLine(formattedData);
        }

        var csvContent = Encoding.UTF8.GetBytes(csvData.ToString());
        return csvContent;
    }

    private bool IsQuantifiableType(Type type)
    {
        var actualType = Nullable.GetUnderlyingType(type) ?? type;
        var isQuantifiable = typeof(IConvertible).IsAssignableFrom(actualType) && actualType != typeof(string);
        return isQuantifiable;
    }

    [GeneratedRegex("\\s+")]
    private static partial Regex AllSpaces();

    private Dictionary<string, int> CreateGraphDataDictionary(List<EmployeeDto> employees, List<string> dataTypeList)
    {
        var dataDictionary = employees
                                .GroupBy(employee =>
                                {
                                    var keyBuilder = new StringBuilder();
                                    foreach (var dataType in dataTypeList)
                                        if (BaseDataType.HasCustom(dataType))
                                        {
                                            var obj = BaseDataType.GetCustom(dataType);
                                            var val = obj.GenerateData(employee, _services);
                                            if (val == null)
                                                continue;
                                            keyBuilder.Append(val);
                                        }
                                        else
                                        {
                                            var propertyInfo = typeof(EmployeeDto).GetProperty(dataType);
                                            if (propertyInfo == null)
                                                continue;
                                            var val = propertyInfo.GetValue(employee);
                                            if (val == null)
                                                continue;

                                            keyBuilder.Append(val + ", ");
                                        }

                                    if (keyBuilder.Length > 2) keyBuilder.Length -= 2;
                                    return keyBuilder.ToString();
                                })
                                .Where(x => string.IsNullOrWhiteSpace(x.Key) == false)
                                .ToDictionary(group => group.Key ?? "Unknown", group => group.Count());

        return dataDictionary;
    }
}