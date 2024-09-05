using System.Text;
using System.Text.RegularExpressions;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Session;
using Microsoft.EntityFrameworkCore;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities.HRIS;

namespace HRIS.Services.Services;

public partial class ChartService : IChartService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeService _employeeService;
    private readonly IServiceProvider _services;
    private readonly AuthorizeIdentity _identity;
    private List<BaseDataType> GetDataTypes() => BaseDataType.Charts;

    public ChartService(IUnitOfWork db, IEmployeeService employeeService, IServiceProvider services, AuthorizeIdentity identity)
    {
        _db = db;
        _employeeService = employeeService;
        _services = services;
        _identity = identity;
    }

    public async Task<bool> CheckIfChartsExists(int Id)
    {
        return await _db.Employee.Any(employee => employee.Id == Id);
    }

    public async Task<List<ChartDto>> GetAllCharts()
    {
        var charts = await _db.Chart.Get().Include(chart => chart.Datasets).Select(c => c.ToDto()).ToListAsync();
        for (int i = 0; i < charts.Count; i++)
        {
            for (int j = 0; j < charts[i].DataTypes!.Count; j++)
            {
                charts[i].DataTypes![j] = CapitalLetters().Replace(charts[i].DataTypes![j], "$1 $2");
            }
        }
        return charts;
    }

    public async Task<List<ChartDto>> GetEmployeeChartsById(int employeeId)
    {
        var exists = await CheckIfChartsExists(employeeId);
        if (exists == false)
            throw new CustomException("Chart not found");

        if (!_identity.IsSupport && employeeId != _identity.EmployeeId)
            throw new CustomException("Unauthorized access.");

        var charts = await _db.Chart.Get()
            .Where(chart => chart.EmployeeId == employeeId)
            .Include(chart => chart.Datasets).Select(c => c.ToDto()).ToListAsync();

        for (int i = 0; i < charts.Count; i++)
        {
            for (int j = 0; j < charts[i].DataTypes!.Count; j++)
            {
                charts[i].DataTypes![j] = CapitalLetters().Replace(charts[i].DataTypes![j], "$1 $2");
            }
        }
        return charts;
    }

    public async Task<ChartDto> CreateChart(List<string> dataTypes, List<string> roles, string chartName,
                                            string chartType, int employeeId)
    {
        var exists = await CheckIfChartsExists(employeeId);
        if (exists == false)
            throw new CustomException("Chart not found");

        if (!_identity.IsSupport && employeeId != _identity.EmployeeId)
            throw new CustomException("Unauthorized access.");

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
            Datasets = new List<ChartDataSet>(),
            EmployeeId = employeeId,
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
            {
                employees = await _employeeService.GetAll();
                roleList.RemoveAt(0);
            }
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
            chart.Roles = roleList;
            chart.Type = chartType;
            chart.Subtype = "standard";
            chart.Datasets = new List<ChartDataSet> { chartDataSet };
        }
        return (await _db.Chart.Add(chart)).ToDto();
    }

    public async Task<ChartDataDto> GetChartData(List<string> dataTypes)
    {
        if (!_identity.IsSupport)
            throw new CustomException("Unauthorized access.");
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

    public async Task<ChartDto> DeleteChart(int id)
    {
        var exists = await _db.Chart.Any(chart => chart.Id == id); ;
        if (exists == false)
            throw new CustomException("Chart not found");

        if (!_identity.IsSupport && id != _identity.EmployeeId)
            throw new CustomException("Unauthorized access.");

        return (await _db.Chart.Delete(id)).ToDto();
    }

    public async Task<ChartDto> UpdateChart(ChartDto chartDto)
    {
        var exists = await CheckIfChartsExists(chartDto.EmployeeId);
        if (!exists) throw new CustomException("No chart data record found");
        if (!_identity.IsSupport && chartDto.Id != _identity.EmployeeId)
            throw new CustomException("Unauthorized access.");
        return (await _db.Chart.Update(new Chart(chartDto))).ToDto();
    }

    public string[] GetColumnsFromTable()
    {
        if (!_identity.IsSupport)
            throw new CustomException("Unauthorized access.");
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

        for (int i = 0; i < quantifiableColumnNames.Length; i++)
        {
            quantifiableColumnNames[i] = CapitalLetters().Replace(quantifiableColumnNames[i], "$1 $2");
        }
        return quantifiableColumnNames;
    }

    public async Task<byte[]?> ExportCsvAsync(List<string> dataTypes)
    {
        if (!_identity.IsSupport)
            throw new CustomException("Unauthorized access.");

        var employees = await _db.Employee.GetAll();

        if (dataTypes == null || !dataTypes.Any())
            throw new CustomException("Data types list is empty or null.");

        var dataTypeList = dataTypes.SelectMany(item => item.Split(',')).Where(item => !string.IsNullOrWhiteSpace(item)).ToList();
        var propertyNames = new List<string>();

        if (dataTypeList.Contains("Age"))
            propertyNames.Add("Age");

        foreach (var typeName in dataTypeList)
        {
            if (typeName == "Age")
                continue;

            var propertyInfo = typeof(EmployeeDto).GetProperty(typeName);

            if (propertyInfo == null && GetDataTypes().All(x => x.Name != typeName))
            {
                throw new CustomException($"Invalid property name: {typeName}");
            }

            propertyNames.Add(typeName);
        }

        var csvData = new StringBuilder();
        csvData.Append("First Name,Last Name");

        foreach (var propertyName in propertyNames)
            csvData.Append("," + propertyName);
        csvData.AppendLine();

        foreach (var employee in employees)
        {
            var employeeDto = employee.ToDto();

            var formattedData = $"{employee.Name ?? ""},{employee.Surname ?? ""}";

            foreach (var dataType in propertyNames)
            {
                if (GetDataTypes().Any(x => x.Name == dataType))
                {
                    var obj = GetDataTypes().First(x => x.Name == dataType);
                    var val = obj.GenerateData(employeeDto, _services);

                    formattedData += $",{val?.Replace(",", "").Trim() ?? ""}";
                }
                else
                {
                    var propertyInfo = typeof(EmployeeDto).GetProperty(dataType);
                    if (propertyInfo != null)
                    {
                        var val = propertyInfo.GetValue(employeeDto);

                        var valueString = val switch
                        {
                            DateTime dateTime => dateTime.ToString("yyyy-MM-dd"),
                            bool boolValue => boolValue ? "True" : "False",
                            _ => val?.ToString() ?? ""
                        };

                        formattedData += $",{valueString.Replace(",", "").Trim()}";
                    }
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

    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex CapitalLetters();
}
