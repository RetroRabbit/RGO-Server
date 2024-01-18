﻿using Microsoft.EntityFrameworkCore;
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

    public async Task<DevsAndDesignersCountDto> GetDevsAndDesignersCount()
    {
        var employees = await _employeeService.GetAll();
        var devsQuery = _db.Employee.Get().Where(e => e.EmployeeTypeId == 2);
        var designersQuery = _db.Employee.Get().Where(e => e.EmployeeTypeId == 3);
        var scrumMastersQuery = _db.Employee.Get().Where(e => e.EmployeeTypeId == 4);
        var businessSupportQuery = _db.Employee.Get().Where(e => e.EmployeeTypeId == 5);


        var totalnumberOfDevs = await devsQuery.ToListAsync();
        var totalnumberOfDesigners = await designersQuery.ToListAsync();
        var totalnumbersOfScrumMasters = await scrumMastersQuery.ToListAsync();
        var totalnumberOfBusinessSupport = await businessSupportQuery.ToListAsync();

        var totalnumberOfDevsOnBench = await devsQuery.Where(c => c.ClientAllocated == 7).ToListAsync();
        var totalnumberOfDesignersOnBench = await designersQuery.Where(c => c.ClientAllocated == 7).ToListAsync();
        var totalnumbersOfScrumMastersOnBench = await designersQuery.Where(c => c.ClientAllocated == 7).ToListAsync();

        var totalnumberOfEmployeesOnBench = totalnumberOfDevsOnBench.Count +
           totalnumberOfDesignersOnBench.Count +
           totalnumbersOfScrumMastersOnBench.Count;

        var totalNumberOfDevsDesignersAndScrumsOnClients = await _db.Employee
           .Get()
           .Where(e => (e.EmployeeTypeId == 2 || e.EmployeeTypeId == 3 || e.EmployeeTypeId == 4) && e.ClientAllocated != 7)
           .ToListAsync();


        var totalNumberOfEmployeesDevsScrumsAndDevs = totalnumberOfEmployeesOnBench + totalNumberOfDevsDesignersAndScrumsOnClients.Count;
        var billableEmployees = (double)totalNumberOfDevsDesignersAndScrumsOnClients.Count / totalNumberOfEmployeesDevsScrumsAndDevs * 100;

        return new DevsAndDesignersCountDto
        {
            DevsCount = totalnumberOfDevs.Count,
            DesignersCount = totalnumberOfDesigners.Count,
            ScrumMastersCount = totalnumbersOfScrumMasters.Count,
            BusinessSupportCount = totalnumberOfBusinessSupport.Count,
            DevsOnBenchCount = totalnumberOfDevsOnBench.Count,
            DesignersOnBenchCount = totalnumberOfDesignersOnBench.Count,
            ScrumMastersOnBenchCount = totalnumbersOfScrumMastersOnBench.Count,
            TotalNumberOfEmployeesOnBench = totalnumberOfEmployeesOnBench,
            BillableEmployeesPercentage = Math.Round(billableEmployees,0),
        }; 
    }

    public async Task<ChurnRateDto> CalculateChurnRate()
    {
        var currentMonthTotal = await GetCurrentMonthTotal();
        var previousMonthTotal = await GetPreviousMonthTotal();

        if (previousMonthTotal != null && previousMonthTotal.EmployeeTotal > 0)
        {
            var churnRate = ((double)(currentMonthTotal.EmployeeTotal - previousMonthTotal.EmployeeTotal) / previousMonthTotal.EmployeeTotal) * 100;

            return new ChurnRateDto
            {
                ChurnRate = Math.Round(churnRate,0),
                Month = previousMonthTotal.Month,
                Year = previousMonthTotal.Year,
            };
        }
        else
        {
            
            return new ChurnRateDto
            {
                ChurnRate = 0,  
                Month = "N/A",  
                Year = 0,      
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

            MonthlyEmployeeTotalDto monthlyEmployeeTotalDto = new MonthlyEmployeeTotalDto(0, employeeTotalCount.Count, currentMonth, currentYear);

            var newMonthlyEmployeeTotal = new MonthlyEmployeeTotal(monthlyEmployeeTotalDto);

           return await _db.MonthlyEmployeeTotal.Add(newMonthlyEmployeeTotal);
        }

        return currentEmployeeTotal.ToDto();
    }

    private async Task<MonthlyEmployeeTotalDto> GetPreviousMonthTotal()
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
