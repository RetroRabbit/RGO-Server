using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Primitives;
using RGO.Models;
using RGO.Services.Interfaces;
using RGO.UnitOfWork;
using RGO.UnitOfWork.Entities;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RGO.Services.Services;

public class ChartService : IChartService
{
    private readonly IUnitOfWork _db;
    private readonly IEmployeeService _employeeService;
    public ChartService(IUnitOfWork db, IEmployeeService employeeService) 
    {
        _db = db;
        _employeeService = employeeService;
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

    public async Task<ChartDto> CreateChart(List<string> dataTypes, string chartName, string chartType)
    {
        var employees = await _employeeService.GetAll();

        var dataTypeList = dataTypes.SelectMany(item => item.Split(',')).ToList();

        var dataDictionary = employees
         .GroupBy(employee =>
         {
             var keyBuilder = new StringBuilder();
             foreach (var dataType in dataTypeList)
             {
                 if (dataType == "Age")
                 {
                     var dobPropertyInfo = typeof(EmployeeDto).GetProperty("DateOfBirth");
                     if (dobPropertyInfo == null)
                     {
                         throw new ArgumentException($"EmployeeDto does not have a DateOfBirth property.");
                     }

                     var dob = (DateOnly)dobPropertyInfo.GetValue(employee);
                     var age = CalculateAge(dob);
                     keyBuilder.Append(age.ToString() + ", ");
                 }
                 else
                 {
                     var propertyInfo = typeof(EmployeeDto).GetProperty(dataType);
                     if (propertyInfo == null)
                     {
                         throw new ArgumentException($"Invalid dataType: {dataType}");
                     }
                     keyBuilder.Append(propertyInfo.GetValue(employee).ToString() + ", ");
                 }
             }
             if (keyBuilder.Length > 2)
             {
                 keyBuilder.Length -= 2; 
             }
             return keyBuilder.ToString();
         })
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

    private int CalculateAge(DateOnly dob)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - dob.Year;

        if (today.DayOfYear < dob.DayOfYear)
            age--;

        return age;
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
                     {
                         throw new ArgumentException($"Invalid dataType: {dataType}");
                     }

                     keyBuilder.Append(propertyInfo.GetValue(employee));
                 }
                 return keyBuilder.ToString();
             })
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
                       !p.Name.Equals("PostalAddressId"))
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

        foreach (var typeName in dataTypeList)
        {
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

            foreach (var propertyName in propertyNames)
            {
                var propertyInfo = typeof(EmployeeDto).GetProperty(propertyName);
                var propertyValue = propertyInfo.GetValue(employee);

                formattedData += $",{propertyValue}";
            }

            csvData.AppendLine(formattedData);
        }

        var csvContent = Encoding.UTF8.GetBytes(csvData.ToString());

        return csvContent;
    }

}
