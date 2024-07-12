using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Moq;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace HRIS.Services.Tests.Services;

public class ChartServiceUnitTests
{
    private readonly Mock<IEmployeeService> _employeeService;
    private readonly Mock<IServiceProvider> _services;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly Employee _testEmployee = EmployeeTestData.EmployeeOne;

    public ChartServiceUnitTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _employeeService = new Mock<IEmployeeService>();
        _services = new Mock<IServiceProvider>();
    }

    [Fact]
    public async Task GetAllChartsTest()
    {
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object);

        _unitOfWork.Setup(u => u.Chart.Get(It.IsAny<Expression<Func<Chart, bool>>>())).Returns(new List<Chart>().ToMockIQueryable());

        var result = await chartService.GetAllCharts();

        Assert.NotNull(result);
        Assert.IsType<List<ChartDto>>(result);
        _unitOfWork.Verify(u => u.Chart.Get(null), Times.Once);
    }

    [Fact]
    public async Task CreateChartTest()
    {
        var roles = new List<string> { "Developer, Designer, Scrum Master, Support Staff" };
        var dataTypes = new List<string> { "Gender, Race, Age" };
        var chartName = "TestChart";
        var chartType = "Pie";


        var developerType =EmployeeTypeTestData.DeveloperType;
        var designerType = EmployeeTypeTestData.DesignerType;
        var scrumType = EmployeeTypeTestData.ScrumType;
        var otherType = EmployeeTypeTestData.OtherType;

        var employeeOne = EmployeeTestData.EmployeeOne;
        var employeeTwo = EmployeeTestData.EmployeeTwo;
        var employeeThree = EmployeeTestData.EmployeeThree;
      
        var employeeList = new List<Employee>
        {
            new(employeeOne.ToDto(), developerType.ToDto()),
            new(employeeThree.ToDto(), designerType.ToDto()),
            new(employeeTwo.ToDto(), scrumType.ToDto()),
            new(employeeThree.ToDto(), otherType.ToDto())
        };

        _unitOfWork.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                 .Returns(employeeList.ToMockIQueryable()); 
        
        var employees = new List<Employee>
        {
            employeeOne,
            employeeThree
        };

        var chart = new Chart
        {
            Id = 1,
            Name = chartName,
            Type = chartType,
            DataTypes = dataTypes,
            Labels = new List<string> { "Male", "Female" },
            Roles = roles,
            Datasets = ChartDataSetTestData.ChartDataSetList
        };

        _employeeService.Setup(e => e.GetAll("")).ReturnsAsync(employees.Select(x => x.ToDto()).ToList());

        _unitOfWork.Setup(u => u.Chart.Add(It.IsAny<Chart>()))
                   .ReturnsAsync(chart);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object);

        var result = await chartService.CreateChart(dataTypes, roles, chartName, chartType, _testEmployee.Id);

        Assert.NotNull(result);
        Assert.Equal(chartName, result.Name);
        Assert.Equal(chartType, result.Type);

        chart.Type = "stacked";

        _unitOfWork.Setup(u => u.Chart.Add(It.IsAny<Chart>()))
                   .ReturnsAsync(chart);
        
        chartType = "stacked";

        employeeList[0].EmployeeType = developerType;
        employeeList[1].EmployeeType = designerType;
        employeeList[2].EmployeeType = scrumType;
        employeeList[3].EmployeeType = otherType;

        _unitOfWork.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                 .Returns(employeeList.ToMockIQueryable());

        result = await chartService.CreateChart(dataTypes, roles, chartName, chartType, _testEmployee.Id);

        Assert.NotNull(result);
        Assert.Equal(chartName, result.Name);
        Assert.Equal(chartType, result.Type);
    }

    [Fact]
    public async Task CreateChartTestAll()
    {
        var roles = new List<string> { "All" };
        var dataTypes = new List<string> { "Gender, Race, Age" };
        var chartName = "TestChart";
        var chartType = "Pie";

        var developerType = EmployeeTypeTestData.DeveloperType;
        var designerType = EmployeeTypeTestData.DesignerType;

        var employeeOne = EmployeeTestData.EmployeeOne;

        var employeeTwo = EmployeeTestData.EmployeeTwo;

        var employeeList = new List<Employee>
        {
            new(employeeOne.ToDto(), developerType.ToDto()),
            new(employeeTwo.ToDto(), designerType.ToDto())
        };

        var employees = new List<Employee>
        {
            employeeOne,
            employeeTwo
        };

        var chart = new Chart
        {
            Id = 1,
            Name = chartName,
            Type = chartType,
            DataTypes = dataTypes,
            Labels = new List<string> { "Male", "Female" },
            Roles = roles,
            Datasets = ChartDataSetTestData.ChartDataSetList
        };

        _employeeService.Setup(e => e.GetAll("")).ReturnsAsync(employees.Select(x => x.ToDto()).ToList);

        _unitOfWork.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(employeeList.ToMockIQueryable());

        _unitOfWork.Setup(u => u.Chart.Add(It.IsAny<Chart>()))
                   .ReturnsAsync(chart);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object);

        var result = await chartService.CreateChart(dataTypes, roles, chartName, chartType, _testEmployee.Id);

        Assert.NotNull(result);
        Assert.Equal(chartName, result.Name);
        Assert.Equal(chartType, result.Type);
    }

    [Fact]
    public async Task GetChartDataTest()
    {
        var dataType = new List<string> { "Gender", "Race" };

        var employeeOne = EmployeeTestData.EmployeeOne;

        var employees = new List<Employee>
        {
            employeeOne
        };

        _employeeService.Setup(e => e.GetAll("")).ReturnsAsync(employees.Select(x => x.ToDto()).ToList());

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object);

        var result = await chartService.GetChartData(dataType);

        Assert.NotNull(result);
        Assert.IsType<ChartDataDto>(result);
    }

    [Fact]
    public async Task DeleteChartTest()
    {
        var chartId = 1;
        var expectedChart = new Chart
        {
            Id = chartId,
            Name = "Test",
            Type = "Pie",
            DataTypes = new List<string> { "Gender", "Race" },
            Labels = new List<string> { "Male", "Female" },
            Roles = new List<string>{ "All" },
            Datasets = ChartDataSetTestData.ChartDataSetList
        };

        _unitOfWork.Setup(u => u.Chart.Delete(chartId)).ReturnsAsync(expectedChart);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object);

        var result = await chartService.DeleteChart(chartId);

        Assert.NotNull(result);
        Assert.IsType<ChartDto>(result);
        Assert.Equal(expectedChart.Id, result.Id);
        _unitOfWork.Verify(x => x.Chart.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task UpdateChartTest()
    {
        var expectedChart = new Chart
        {
            Id = 1,
            Name = "Update",
            Type = "Pie",
            DataTypes = new List<string> { "Gender", "Race" },
            Labels = new List<string> { "Male", "Female" },
            Roles = new List<string> { "All" },
            Datasets = ChartDataSetTestData.ChartDataSetList
        };

        var existingCharts = new List<Chart>
        {
            new()
            {
                Id = 1,
                Name = "Existing Chart",
                Type = "Existing Type",
                DataTypes = expectedChart.DataTypes,
                Labels = expectedChart.Labels,
                Roles = new List<string> { "All" },
                Datasets = ChartDataSetTestData.ChartDataSetList
            }
        };

        _unitOfWork.Setup(x => x.Chart.GetAll(null)).ReturnsAsync(existingCharts);

        _unitOfWork.Setup(x => x.Chart.Update(It.IsAny<Chart>()))
                   .ReturnsAsync(expectedChart);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object);

        var result = await chartService.UpdateChart(expectedChart.ToDto());

        Assert.NotNull(result);
        Assert.Equivalent(expectedChart.ToDto(), result);
        _unitOfWork.Verify(x => x.Chart.Update(It.IsAny<Chart>()), Times.Once);
    }

    [Fact(Skip ="temp")]
    public async Task UpdateChartTestFail()
    {
        var existingCharts = new List<Chart>
        {
            new()
            {
                Id = 1,
                Name = "Existing Chart",
                Type = "Existing Type",
                DataTypes = new List<string> { "Gender", "Race" },
                Labels = new List<string> { "Male", "Female" },
                Roles = new List<string> { "All" },
                Datasets = ChartDataSetTestData.ChartDataSetList
            }
        };

        var nonExistingCharts = new Chart
        {
            Id = 2,
            Name = "Non Existing Chart",
            Type = "Non Existing Type",
            DataTypes = new List<string> { "Gender", "Race" },
            Labels = new List<string> { "Male", "Female" },
            Roles = new List<string> { "All" },
            Datasets = ChartDataSetTestData.ChartDataSetList
        };

        _unitOfWork.SetupSequence(a => a.Chart.GetAll(null)).ReturnsAsync(existingCharts);
        _unitOfWork.Setup(a => a.Chart.Any(It.IsAny<Expression<Func<Chart, bool>>>())).ReturnsAsync(true);
        _unitOfWork.Setup(a => a.Chart.Update(It.IsAny<Chart>())).Throws(new Exception());
        _unitOfWork.Setup(x => x.ErrorLogging.Add(It.IsAny<ErrorLogging>()));

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object);

        var exception = await Assert.ThrowsAsync<Exception>(async () => await chartService.UpdateChart(nonExistingCharts.ToDto()));
        Assert.Equal("No chart data record found", exception.Message);
    }

    [Fact]
    public void GetColumnsFromTableTest()
    {
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object);

        var columnNames = chartService.GetColumnsFromTable();

        Assert.NotNull(columnNames);
        Assert.NotEmpty(columnNames);
    }

    [Fact(Skip = "Needs Work")]
    public async Task ExportCsvAsyncTest()
    {
        var employeeSix = EmployeeTestData.EmployeeSix;

        var employees = new List<Employee>
        {
            employeeSix
        };

        var dataTypeList = new List<string> { "Gender", "Race", "Age" };

        _unitOfWork.Setup(e => e.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(employees);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object);
        var result = await chartService.ExportCsvAsync(dataTypeList);
        var expectedResult = new byte[]
        {
            70, 105, 114, 115, 116, 32, 78, 97, 109, 101, 44, 76, 97, 115, 116, 32, 78, 97, 109, 101,
            44, 65, 103, 101, 44, 71, 101, 110, 100, 101, 114, 44, 82, 97, 99, 101, 13, 10, 69, 115, 116, 105, 97, 97, 110,
            44, 66, 114, 105, 116, 122, 44, 65, 103, 101, 32, 48, 44, 77, 97, 108, 101, 44, 66, 108, 97, 99, 107,
            13, 10
        };

        Assert.NotNull(result);
        Assert.IsType<byte[]>(result);
        Assert.Equal(expectedResult, result);
    }

    [Fact(Skip = "temp")]
    public async Task ExportCsvAsyncTestFail()
    {
        var dataTypeList = new List<string> { "", "" };

        var employeeOne = EmployeeTestData.EmployeeOne;

        var employees = new List<Employee>
        {
            employeeOne
        };

        _unitOfWork.Setup(e => e.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(employees);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object);
        _unitOfWork.Setup(x => x.ErrorLogging.Add(It.IsAny<ErrorLogging>()));
        var exception = await Assert.ThrowsAsync<Exception>( async () => await chartService.ExportCsvAsync(dataTypeList));
                                                           
        Assert.Equal("Invalid property name: ", exception.Message);
    }
}
