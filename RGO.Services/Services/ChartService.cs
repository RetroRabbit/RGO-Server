using Microsoft.EntityFrameworkCore;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;

namespace RGO.Services.Services;

public class ChartService : IChartService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeService _employeeService;
    private readonly IServiceProvider _services;

    public ChartService(IUnitOfWork db, IEmployeeService employeeService, IServiceProvider services) 
    {
        _db = db;
        _employeeService = employeeService;
        _services = services;
    }

    public  async Task<List<ChartDto>> GetAllCharts() 
    {
        return await _db.Chart.GetAll();
    }

    public async Task<int> GetTotalEmployees()
    {
            var employees = await _employeeService.GetAll();
            return employees.Count;
    }

    public async Task<EmployeeCountDto> GetEmployeesCount()
    {
        var employees = await _employeeService.GetAll();
        var devsQuery = _db.Employee.Get().Where(e => e.EmployeeTypeId == 2);
        var designersQuery = _db.Employee.Get().Where(e => e.EmployeeTypeId == 3);
        var scrumMastersQuery = _db.Employee.Get().Where(e => e.EmployeeTypeId == 4);
        var businessSupportQuery = _db.Employee.Get().Where(e => e.EmployeeTypeId == 5);

        var listOfDevs = await devsQuery.ToListAsync();
        var listOfDesigners = await designersQuery.ToListAsync();
        var listOfScrumMasters = await scrumMastersQuery.ToListAsync();
        var listOfBusinessSupport = await businessSupportQuery.ToListAsync();

        var listOfDevsOnBench = await devsQuery.Where(c => c.ClientAllocated == 1).ToListAsync();
        var listOfDesignersOnBench = await designersQuery.Where(c => c.ClientAllocated == 1).ToListAsync();
        var listOfScrumMastersOnBench = await scrumMastersQuery.Where(c => c.ClientAllocated == 1).ToListAsync();

        var totalnumberOfEmployeesOnBench = listOfDevsOnBench.Count +
           listOfDesignersOnBench.Count +
           listOfScrumMastersOnBench.Count;

        var listOfDevsDesignersAndScrumsOnClients = await _db.Employee
           .Get()
           .Where(e => (e.EmployeeTypeId == 2 || e.EmployeeTypeId == 3 || e.EmployeeTypeId == 4) && e.ClientAllocated != 1)
           .ToListAsync();


        var totalNumberOfEmployeesDevsScrumsAndDevs = totalnumberOfEmployeesOnBench + listOfDevsDesignersAndScrumsOnClients.Count;
        var billableEmployees = totalNumberOfEmployeesDevsScrumsAndDevs > 0
               ? (double)listOfDevsDesignersAndScrumsOnClients.Count / totalNumberOfEmployeesDevsScrumsAndDevs * 100
               : 0;

        var currentMonthTotal = await GetCurrentMonthTotal();
        var previousMonthTotal = await GetPreviousMonthTotal();

        var employeeTotalDifference = previousMonthTotal.EmployeeTotal - currentMonthTotal.EmployeeTotal;
        var isIncrease = employeeTotalDifference > 0;

        return new EmployeeCountDto
        {
            DevsCount = listOfDevs.Count,
            DesignersCount = listOfDesigners.Count,
            ScrumMastersCount = listOfScrumMasters.Count,
            BusinessSupportCount = listOfBusinessSupport.Count,
            DevsOnBenchCount = listOfDevsOnBench.Count,
            DesignersOnBenchCount = listOfDesignersOnBench.Count,
            ScrumMastersOnBenchCount = listOfScrumMastersOnBench.Count,
            TotalNumberOfEmployeesOnBench = totalnumberOfEmployeesOnBench,
            BillableEmployeesPercentage = Math.Round(billableEmployees,0),
            EmployeeTotalDifference = employeeTotalDifference,
            isIncrease= isIncrease,
        }; 
    }

    public async Task<ChurnRateDto> CalculateChurnRate()
    {
        var currentMonthTotal = await GetCurrentMonthTotal();
        var previousMonthTotal = await GetPreviousMonthTotal();

        if (previousMonthTotal != null && previousMonthTotal.EmployeeTotal > 0 && previousMonthTotal.DeveloperTotal > 0 && previousMonthTotal.DesignerTotal > 0 &&
            previousMonthTotal.ScrumMasterTotal > 0 && previousMonthTotal.BusinessSupportTotal > 0)
        {
            var churnRate = ((double)(currentMonthTotal.EmployeeTotal - previousMonthTotal.EmployeeTotal) / previousMonthTotal.EmployeeTotal) * 100;

            var devChurnRate = ((double)(currentMonthTotal.DeveloperTotal - previousMonthTotal.DeveloperTotal) / previousMonthTotal.DeveloperTotal) * 100;
            var designerChurnRate = ((double)(currentMonthTotal.DesignerTotal - previousMonthTotal.DesignerTotal) / previousMonthTotal.DesignerTotal) * 100;
            var scrumMasterChurnRate = ((double)(currentMonthTotal.ScrumMasterTotal - previousMonthTotal.ScrumMasterTotal) / previousMonthTotal.ScrumMasterTotal) * 100;
            var businessSupportChurnRate = ((double)(currentMonthTotal.BusinessSupportTotal - previousMonthTotal.BusinessSupportTotal) / previousMonthTotal.BusinessSupportTotal) * 100;

            return new ChurnRateDto
            {
                ChurnRate = Math.Round(churnRate, 0),
                DeveloperChurnRate = Math.Round(devChurnRate, 0),
                DesignerChurnRate = Math.Round(designerChurnRate, 0),
                ScrumMasterChurnRate = Math.Round(scrumMasterChurnRate, 0),
                BusinessSupportChurnRate = Math.Round(businessSupportChurnRate, 0),
                Month = previousMonthTotal.Month,
                Year = previousMonthTotal.Year,
            };
         }
        else
        {
            return new ChurnRateDto 
            {
                ChurnRate = 0,
                DeveloperChurnRate = 0,
                DesignerChurnRate = 0,
                ScrumMasterChurnRate = 0,
                BusinessSupportChurnRate = 0,
                Month = DateTime.Now.AddMonths(-1).ToString("MMMM"),
                Year = DateTime.Now.Year,
            };  
        }
    }

    public async Task<MonthlyEmployeeTotalDto> GetCurrentMonthTotal()
    {
        var currentMonth = DateTime.Now.ToString("MMMM");

        var currentYear = DateTime.Now.Year;

        var currentEmployeeTotal = _db.MonthlyEmployeeTotal
            .Get()
            .Where(e => e.Month == currentMonth && e.Year == currentYear)
            .FirstOrDefault();

        if (currentEmployeeTotal == null)
        {
            var employeeTotalCount = await _db.Employee.GetAll();

            var devsQuery = _db.Employee.Get().Where(e => e.EmployeeTypeId == 2);
            var designersQuery = _db.Employee.Get().Where(e => e.EmployeeTypeId == 3);
            var scrumMastersQuery = _db.Employee.Get().Where(e => e.EmployeeTypeId == 4);
            var businessSupportQuery = _db.Employee.Get().Where(e => e.EmployeeTypeId == 5);

            var devsTotal = await devsQuery.ToListAsync();
            var designersTotal = await designersQuery.ToListAsync();
            var scrumMastersTotal = await scrumMastersQuery.ToListAsync();
            var businessSupportTotal = await businessSupportQuery.ToListAsync();


            MonthlyEmployeeTotalDto monthlyEmployeeTotalDto = new MonthlyEmployeeTotalDto
                (0, employeeTotalCount.Count, devsTotal.Count, designersTotal.Count, scrumMastersTotal.Count, 
                businessSupportTotal.Count, currentMonth, currentYear);

            var newMonthlyEmployeeTotal = new MonthlyEmployeeTotal(monthlyEmployeeTotalDto);

           return await _db.MonthlyEmployeeTotal.Add(newMonthlyEmployeeTotal);
        }

        return currentEmployeeTotal.ToDto();
    }

    public async Task<MonthlyEmployeeTotalDto> GetPreviousMonthTotal()
    {
        var previousMonth = DateTime.Now.AddMonths(-1).ToString("MMMM");

        var previousEmployeeTotal =  _db.MonthlyEmployeeTotal
            .Get()
            .Where(e => e.Month == previousMonth)
            .FirstOrDefault();

        if (previousEmployeeTotal == null)
        {
            return await GetCurrentMonthTotal();
        }

        return previousEmployeeTotal.ToDto();
    }

    public async Task<ChartDto> CreateChart(List<string> dataTypes, List<string> roles, string chartName, string chartType)
    {
        List<EmployeeDto> employees;

        var roleList = roles.SelectMany(item => item.Split(',')).ToList();

        if ( roleList[0] == "All")
        {
            employees = await _employeeService.GetAll();
        }
        else
        {
                employees = await _db.Employee
                .Get(employee => roleList.Contains(employee.EmployeeType.Name))
                .Include(employee => employee.EmployeeType)
                .Select(employee => employee.ToDto())
                .AsNoTracking()
                .ToListAsync();
        }

        var dataTypeList = dataTypes.SelectMany(item => item.Split(',')).ToList();

        var dataDictionary = employees
         .GroupBy(employee =>
         {
             var keyBuilder = new StringBuilder();
             foreach (var dataType in dataTypeList)
             {
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
             }
             if (keyBuilder.Length > 2)
             {
                 keyBuilder.Length -= 2; 
             }
             return keyBuilder.ToString();
         })
         .Where(x => string.IsNullOrWhiteSpace(x.Key) == false)
        .ToDictionary(group => group.Key ?? "Unknown", group => group.Count());

        var labels = dataDictionary.Keys.ToList();
        var data = dataDictionary.Values.ToList();

        var chart = new Chart
        {
            Name = chartName,
            Type = chartType,
            DataTypes = dataTypes,
            Labels = labels,
            Data = data
        };

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

                     if(value == null )
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
        var charts = await _db.Chart.GetAll();
        var chartData = charts
            .Where(chartData => chartData.Id == chartDto.Id)
            .Select(chartData => chartData)
            .FirstOrDefault();
        if (chartData == null) { throw new Exception("No chart data record found"); }
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
                       !p.Name.Equals("SalaryDays"))
            .Select(p => p.Name)
            .ToArray();
        quantifiableColumnNames = quantifiableColumnNames.Concat(new string[] { "Age" }).ToArray();
        return quantifiableColumnNames;
    }

    private bool IsQuantifiableType(Type type)
    {
        Type actualType = Nullable.GetUnderlyingType(type) ?? type;
        bool isQuantifiable = typeof(IConvertible).IsAssignableFrom(actualType) && actualType != typeof(string);
        return isQuantifiable;
    }

    public async Task<byte[]> ExportCsvAsync(List<string> dataTypes)
    {
        var employees = await _db.Employee.GetAll();
        var dataTypeList = dataTypes.SelectMany(item => item.Split(',')).ToList();
        var propertyNames = new List<string>();

        if (dataTypeList.Contains("Age"))
        {
            propertyNames.Add("Age");
        }

        foreach (var typeName in dataTypeList)
        {
            if (typeName == "Age")
            {
                continue;
            }

            var propertyInfo = typeof(EmployeeDto).GetProperty(typeName);
            if (propertyInfo == null)
            {
                throw new ArgumentException($"Invalid property name: {typeName}", nameof(dataTypeList));
            }
            propertyNames.Add(typeName);
        }

        var csvData = new StringBuilder();
        csvData.Append("First Name,Last Name");
        foreach (var propertyName in propertyNames)
        {
            csvData.Append("," + propertyName);
        }
        csvData.AppendLine();
        foreach (var employee in employees)
        {
            var formattedData = $"{employee.Name},{employee.Surname}";
            foreach (var dataType in propertyNames)
            {
                if (BaseDataType.HasCustom(dataType))
                 {
                     var obj = BaseDataType.GetCustom(dataType);
                     var val = obj.GenerateData(employee, _services);
                     if (val == null)
                         continue;
                    formattedData += $",{val.Replace(",", "").Trim()}";
                }
                 else
                 {
                     var propertyInfo = typeof(EmployeeDto).GetProperty(dataType);
                     if (propertyInfo == null)
                         continue;
                     var val = propertyInfo.GetValue(employee);
                     if (val == null)
                         continue;
                    formattedData += $",{val.ToString().Replace(",", "").Trim()}";
                }
            }
            csvData.AppendLine(formattedData);
        }

        var csvContent = Encoding.UTF8.GetBytes(csvData.ToString());
        return csvContent;
    }
}
