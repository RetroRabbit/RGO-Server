using System.Linq.Expressions;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
using Moq;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork;
using RR.UnitOfWork.Entities;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;

namespace RGO.Tests.Services;

public class ChartServiceUnitTests
{
    private readonly Mock<IEmployeeService> _employeeService;
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private readonly Mock<IServiceProvider> _services;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly EmployeeType employeeType1;
    private readonly EmployeeType employeeType2;
    private readonly EmployeeTypeDto employeeTypeDto1;
    private readonly EmployeeTypeDto employeeTypeDto2;
    private readonly IErrorLoggingService _errorLoggingService;
    EmployeeDto testEmployee = EmployeeTestData.EmployeeDto;

    public ChartServiceUnitTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _employeeService = new Mock<IEmployeeService>();
        _services = new Mock<IServiceProvider>();
        _errorLoggingService = new ErrorLoggingService(_unitOfWork.Object);

        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        employeeTypeDto1 = EmployeeTypeTestData.DeveloperType;
        employeeTypeDto2 = EmployeeTypeTestData.PeopleChampionType;
        employeeType1 = new EmployeeType(employeeTypeDto1);
        employeeType2 = new EmployeeType(employeeTypeDto2);
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType1.Name!))
                                .Returns(Task.FromResult(employeeTypeDto1));
        _employeeTypeServiceMock.Setup(r => r.GetEmployeeType(employeeType2.Name!))
                                .Returns(Task.FromResult(employeeTypeDto2));
        
    }

    [Fact]
    public async Task GetAllChartsTest()
    {
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _errorLoggingService);

        _unitOfWork.Setup(u => u.Chart.Get(It.IsAny<Expression<Func<Chart, bool>>>())).Returns(new List<Chart>().AsQueryable().BuildMock());

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


        EmployeeTypeDto developerEmployeeTypeDto =EmployeeTypeTestData.DeveloperType;
        EmployeeTypeDto designerEmployeeTypeDto = EmployeeTypeTestData.DesignerType;
        EmployeeTypeDto scrumMasterEmployeeTypeDto = EmployeeTypeTestData.ScrumType;
        EmployeeTypeDto otherEmployeeTypeDto = EmployeeTypeTestData.OtherType;

        EmployeeDto developerEmployeeDto = EmployeeTestData.EmployeeDto;
        EmployeeDto developerEmployeeDto1 = EmployeeTestData.EmployeeDto2;
        EmployeeDto designerEmployeeDto = EmployeeTestData.EmployeeDto3;
      
        var employeeList = new List<Employee>
        {
            new(developerEmployeeDto, developerEmployeeTypeDto),
            new(designerEmployeeDto, designerEmployeeTypeDto),
            new(developerEmployeeDto, scrumMasterEmployeeTypeDto),
            new(designerEmployeeDto, otherEmployeeTypeDto)
        };


        _unitOfWork.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                 .Returns(employeeList.AsQueryable().BuildMock()); 
        
        var employeeDtoList = new List<EmployeeDto>
        {
            developerEmployeeDto,
            designerEmployeeDto
        };

        var chartDto = new ChartDto
        {
            Id = 1,
            Name = chartName,
            Type = chartType,
            DataTypes = dataTypes,
            Labels = new List<string> { "Male", "Female" },
            Roles = roles,
            Datasets = ChartDataSetTestData.chartDataSetDtoList
        };

        _employeeService.Setup(e => e.GetAll("")).ReturnsAsync(employeeDtoList);

        _unitOfWork.Setup(u => u.Chart.Add(It.IsAny<Chart>()))
                   .ReturnsAsync(chartDto);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _errorLoggingService);

        var result = await chartService.CreateChart(dataTypes, roles, chartName, chartType, testEmployee.Id);

        Assert.NotNull(result);
        Assert.Equal(chartName, result.Name);
        Assert.Equal(chartType, result.Type);

        chartDto.Type = "stacked";

        _unitOfWork.Setup(u => u.Chart.Add(It.IsAny<Chart>()))
                   .ReturnsAsync(chartDto);
        
        chartType = "stacked";

        employeeList[0].EmployeeType = new EmployeeType(developerEmployeeTypeDto);
        employeeList[1].EmployeeType = new EmployeeType(designerEmployeeTypeDto);
        employeeList[2].EmployeeType = new EmployeeType(scrumMasterEmployeeTypeDto);
        employeeList[3].EmployeeType = new EmployeeType(otherEmployeeTypeDto);

        _unitOfWork.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                 .Returns(employeeList.AsQueryable().BuildMock());

        result = await chartService.CreateChart(dataTypes, roles, chartName, chartType, testEmployee.Id);

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

        EmployeeTypeDto devEmployeeTypeDto = EmployeeTypeTestData.DeveloperType;
        EmployeeTypeDto desEmployeeTypeDto = EmployeeTypeTestData.DesignerType;

        var employeeAddressDto = EmployeeAddressTestData.EmployeeAddressDto;
           
        EmployeeDto employeeDto = EmployeeTestData.EmployeeDto;

        EmployeeDto desEmployeeDto = EmployeeTestData.EmployeeDto2;

        var employeeList = new List<Employee>
        {
            new(employeeDto, devEmployeeTypeDto),
            new(desEmployeeDto, desEmployeeTypeDto)
        };

        var employeeDtoList = new List<EmployeeDto>
        {
            employeeDto,
            desEmployeeDto
        };

        var chartDto = new ChartDto
        {
            Id = 1,
            Name = chartName,
            Type = chartType,
            DataTypes = dataTypes,
            Labels = new List<string> { "Male", "Female" },
            Roles = roles,
            Datasets = ChartDataSetTestData.chartDataSetDtoList
        };

        _employeeService.Setup(e => e.GetAll("")).ReturnsAsync(employeeDtoList);

        _unitOfWork.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(employeeList.AsQueryable().BuildMock());

        _unitOfWork.Setup(u => u.Chart.Add(It.IsAny<Chart>()))
                   .ReturnsAsync(chartDto);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _errorLoggingService);

        var result = await chartService.CreateChart(dataTypes, roles, chartName, chartType, testEmployee.Id);

        Assert.NotNull(result);
        Assert.Equal(chartName, result.Name);
        Assert.Equal(chartType, result.Type);
    }

    [Fact]
    public async Task GetChartDataTest()
    {
        var dataType = new List<string> { "Gender", "Race" };

        EmployeeTypeDto employeeTypeDto = EmployeeTypeTestData.DeveloperType;
        EmployeeType employeeType = new(employeeTypeDto);
        var employeeAddressDto = EmployeeAddressTestData.EmployeeAddressDto;

        EmployeeDto employeeDto = EmployeeTestData.EmployeeDto;

        var employeeList = new List<EmployeeDto>
        {
            employeeDto
        };

        _employeeService.Setup(e => e.GetAll("")).ReturnsAsync(employeeList);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _errorLoggingService);

        var result = await chartService.GetChartData(dataType);

        Assert.NotNull(result);
        Assert.IsType<ChartDataDto>(result);
    }

    [Fact]
    public async Task DeleteChartTest()
    {
        var chartId = 1;
        var expectedChartDto = new ChartDto
        {
            Id = chartId,
            Name = "Test",
            Type = "Pie",
            DataTypes = new List<string> { "Gender", "Race" },
            Labels = new List<string> { "Male", "Female" },
            Roles = new List<string>{ "All" },
            Datasets = ChartDataSetTestData.chartDataSetDtoList
        };

        _unitOfWork.Setup(u => u.Chart.Delete(chartId)).ReturnsAsync(expectedChartDto);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _errorLoggingService);

        var result = await chartService.DeleteChart(chartId);

        Assert.NotNull(result);
        Assert.IsType<ChartDto>(result);
        Assert.Equal(expectedChartDto.Id, result.Id);
        _unitOfWork.Verify(x => x.Chart.Delete(It.IsAny<int>()), Times.Once);
    }

    [Fact]
    public async Task UpdateChartTest()
    {
        var chartDtoToUpdate = new ChartDto
        {
            Id = 1,
            Name = "Update",
            Type = "Pie",
            DataTypes = new List<string> { "Gender", "Race" },
            Labels = new List<string> { "Male", "Female" },
            Roles = new List<string> { "All" },
            Datasets = ChartDataSetTestData.chartDataSetDtoList
        };

        var existingCharts = new List<ChartDto>
        {
            new ChartDto
            {
                Id = 1,
                Name = "Existing Chart",
                Type = "Existing Type",
                DataTypes = chartDtoToUpdate.DataTypes,
                Labels = chartDtoToUpdate.Labels,
                Roles = new List<string> { "All" },
                Datasets = ChartDataSetTestData.chartDataSetDtoList
            }
        };

        _unitOfWork.Setup(x => x.Chart.GetAll(null)).Returns(Task.FromResult(existingCharts));

        _unitOfWork.Setup(x => x.Chart.Update(It.IsAny<Chart>()))
                   .Returns(Task.FromResult(chartDtoToUpdate));

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _errorLoggingService);

        var result = await chartService.UpdateChart(chartDtoToUpdate);

        Assert.NotNull(result);
        Assert.Equal(chartDtoToUpdate, result);
        _unitOfWork.Verify(x => x.Chart.Update(It.IsAny<Chart>()), Times.Once);
    }

    [Fact]
    public async Task UpdateChartTestFail()
    {
        var existingCharts = new List<ChartDto>
        {
            new ChartDto
            {
                Id = 1,
                Name = "Existing Chart",
                Type = "Existing Type",
                DataTypes = new List<string> { "Gender", "Race" },
                Labels = new List<string> { "Male", "Female" },
                Roles = new List<string> { "All" },
                Datasets = ChartDataSetTestData.chartDataSetDtoList
            }
        };

        var nonExistingCharts = new ChartDto
        {
            Id = 2,
            Name = "Non Existing Chart",
            Type = "Non Existing Type",
            DataTypes = new List<string> { "Gender", "Race" },
            Labels = new List<string> { "Male", "Female" },
            Roles = new List<string> { "All" },
            Datasets = ChartDataSetTestData.chartDataSetDtoList
        };

        _unitOfWork.SetupSequence(a => a.Chart.GetAll(null)).Returns(Task.FromResult(existingCharts));
        _unitOfWork.Setup(a => a.Chart.Any(It.IsAny<Expression<Func<Chart, bool>>>())).ReturnsAsync(true);
        _unitOfWork.Setup(a => a.Chart.Update(It.IsAny<Chart>())).Throws(new Exception());
        _unitOfWork.Setup(x => x.ErrorLogging.Add(It.IsAny<ErrorLogging>()));

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _errorLoggingService);

        var exception = await Assert.ThrowsAsync<Exception>(async () => await chartService.UpdateChart(nonExistingCharts));
        Assert.Equal("No chart data record found", exception.Message);
    }

    [Fact]
    public void GetColumnsFromTableTest()
    {
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _errorLoggingService);

        var columnNames = chartService.GetColumnsFromTable();

        Assert.NotNull(columnNames);
        Assert.NotEmpty(columnNames);
    }

    [Fact]
    public async Task ExportCsvAsyncTest()
    {
        var employeeDto = EmployeeTestData.EmployeeDto6;

        var employeeDtoList = new List<EmployeeDto>
        {
            employeeDto
        };

        var dataTypeList = new List<string> { "Gender", "Race", "Age" };
        var propertyNames = new List<string>();

        _unitOfWork.Setup(e => e.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(Task.FromResult(employeeDtoList));

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _errorLoggingService);
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

    [Fact]
    public async Task ExportCsvAsyncTestFail()
    {
        var dataTypeList = new List<string> { "", "" };

        var employeeDto = EmployeeTestData.EmployeeDto;

        var employeeDtoList = new List<EmployeeDto>
        {
            employeeDto
        };

        _unitOfWork.Setup(e => e.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(Task.FromResult(employeeDtoList));

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _errorLoggingService);
        _unitOfWork.Setup(x => x.ErrorLogging.Add(It.IsAny<ErrorLogging>()));
        var exception = await Assert.ThrowsAsync<Exception>( async () => await chartService.ExportCsvAsync(dataTypeList));
                                                           
        Assert.Equal("Invalid property name: ", exception.Message);
    }
}