using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
                .Where(p => IsQuantifiableType(p.PropertyType) && !p.Name.Equals("Id") && !p.Name.Equals("EmployeeTypeId"))
                .Select(p => p.Name)
                .ToArray();

            return quantifiableColumnNames;     
    }

    private bool IsQuantifiableType(Type type)
    {
        return typeof(IConvertible).IsAssignableFrom(type) && type != typeof(string);
    }

    public async Task<byte[]> ExportCsvAsync(string dataType)
    {
        var employees = await _db.Employee.GetAll();

        var propertyInfo = typeof(EmployeeDto).GetProperty(dataType);

        if (propertyInfo == null)
        {
            throw new ArgumentException("Invalid property name", nameof(dataType));
        }

        var csvData = new StringBuilder();
        csvData.AppendLine("First Name,Last Name," + dataType); 

        foreach (var employee in employees)
        {
            var propertyValue = propertyInfo.GetValue(employee);

            var formattedData = $"{employee.Name},{employee.Surname},{propertyValue}";
            csvData.AppendLine(formattedData);
        }

        var csvContent = Encoding.UTF8.GetBytes(csvData.ToString());

        return csvContent;
    }

}
