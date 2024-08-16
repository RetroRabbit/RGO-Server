//using System.Data;
//using System.Linq.Expressions;
//using System.Text;
//using HRIS.Models;
//using HRIS.Services.Interfaces;
//using HRIS.Services.Services;
//using Microsoft.EntityFrameworkCore;
//using MockQueryable.Moq;
//using Moq;
//using RR.Tests.Data;
//using RR.Tests.Data.Models.HRIS;
//using RR.UnitOfWork;
//using RR.UnitOfWork.Entities;
//using RR.UnitOfWork.Entities.HRIS;
//using RR.UnitOfWork.Repositories.HRIS;
//using Xunit;

//namespace HRIS.Services.Tests.Services;
//public class ChartServiceUnitTests
//{
//    private readonly ChartService _chartService;
//    private readonly Mock<IEmployeeService> _employeeService;
//    private readonly Mock<IServiceProvider> _services;
//    private readonly Mock<IUnitOfWork> _unitOfWork;
//    private readonly AuthorizeIdentityMock _identity;
//    private readonly Employee _testEmployee;

//    public ChartServiceUnitTests()
//    {
//        _unitOfWork = new Mock<IUnitOfWork>();
//        _employeeService = new Mock<IEmployeeService>();
//        _services = new Mock<IServiceProvider>();
//        _identity = new AuthorizeIdentityMock("test@gmail.com", "test", "Admin", 1);

//        _testEmployee = EmployeeTestData.EmployeeOne;

//        _chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _identity);
//    }

//    [Fact]
//    public async Task CheckIfChatsExists_ShouldReturnFalse_WhenChartsDoesNotExist()
//    {
//        var employeeId = 1;
//        _unitOfWork.Setup(ex => ex.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(false);

//        var result = await _chartService.CheckIfChatsExists(employeeId);

//        Assert.False(result);
//    }

//    [Fact]
//    public async Task CheckIfChatsExists_ShouldReturnTrue_WhenEmployeeExists()
//    {
//        var employeeId = 2;
//        _unitOfWork.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);

//        var result = await _chartService.CheckIfChatsExists(employeeId);

//        Assert.True(result);
//    }

//    [Fact]
//    public async Task GetAllChartsTest()
//    {
//        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _identity);

//        _unitOfWork.Setup(u => u.Chart.Get(It.IsAny<Expression<Func<Chart, bool>>>())).Returns(new List<Chart>().ToMockIQueryable());

//        var result = await chartService.GetAllCharts();

//        Assert.NotNull(result);
//        Assert.IsType<List<ChartDto>>(result);
//        _unitOfWork.Verify(u => u.Chart.Get(null), Times.Once);
//    }

//    [Fact]
//    public async Task GetAllCharts_ShouldReturnEmptyList_WhenNoChartsAvailable()
//    {
//        var Charts = new List<Chart>();

//        _unitOfWork.Setup(x => x.Chart.Get(It.IsAny<Expression<Func<Chart, bool>>>())).Returns(new List<Chart>().ToMockIQueryable());

//        var result = await _chartService.GetAllCharts();

//        Assert.NotNull(result);
//        Assert.Empty(result);
//    }

//    [Fact]
//    public async Task GetAllCharts_ShouldReturnCapitalizedDataTypes_WhenAuthorized()
//    {
//        var employeeId = _identity.EmployeeId;

//        var chart = new Chart
//        {
//            EmployeeId = employeeId,
//            DataTypes = new List<string> { "DataTypeOne", "DataTypeTwo" }
//        };

//        var charts = new List<Chart> { chart };

//        _unitOfWork.Setup(u => u.Chart.Get(It.IsAny<Expression<Func<Chart, bool>>>())).Returns(charts.ToMockIQueryable());

//        var result = await _chartService.GetAllCharts();

//        Assert.NotNull(result);
//        Assert.Single(result);
//        Assert.Equal("Data Type One", result[0].DataTypes[0]);
//        Assert.Equal("Data Type Two", result[0].DataTypes[1]);
//    }

//    [Fact]
//    public async Task GetEmployeeChartsById_ShouldThrowException_WhenChartsDoNotExist()
//    {
//        var employeeId = 1;

//        _unitOfWork.Setup(x => x.Chart.Get(It.IsAny<Expression<Func<Chart, bool>>>()))
//                   .Returns(new List<Chart>().ToMockIQueryable());

//        _unitOfWork.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
//                   .ReturnsAsync(false);

//        var exception = await Assert.ThrowsAsync<CustomException>(async () =>
//            await _chartService.GetEmployeeChartsById(employeeId)
//        );

//        Assert.Equal("Chat not found", exception.Message);
//    }

//    [Fact]
//    public async Task GetEmployeeChartsById_ShouldReturnChartsWithCapitalizedDataTypes_WhenUserIsAuthorized()
//    {
//        var employeeId = _identity.EmployeeId;
//        var chartDataSet = new ChartDataSet();
//        var chart = new Chart
//        {
//            EmployeeId = employeeId,
//            DataTypes = new List<string> { "DataTypeOne", "DataTypeTwo" },
//            Datasets = new List<ChartDataSet> { chartDataSet }
//        };

//        var charts = new List<Chart> { chart };

//        _unitOfWork.Setup(x => x.Chart.Get(It.IsAny<Expression<Func<Chart, bool>>>()))
//                   .Returns(charts.AsQueryable().BuildMock());

//        _unitOfWork.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
//                   .ReturnsAsync(true);

//        var result = await _chartService.GetEmployeeChartsById(employeeId);

//        Assert.NotNull(result);
//        Assert.Single(result);
//        Assert.Equal("Data Type One", result[0].DataTypes[0]);
//        Assert.Equal("Data Type Two", result[0].DataTypes[1]);
//        Assert.NotNull(result[0].Datasets);
//        Assert.Single(result[0].Datasets);
//    }

//    [Fact]
//    public async Task GetEmployeeChartsById_ShouldThrowUnauthorizedAccess_WhenUserIsNotAuthorized()
//    {
//        var employeeId = 2;

//        var charts = new List<Chart>
//        {
//          new Chart { Id = 1, EmployeeId = employeeId }
//        };

//        _unitOfWork.Setup(x => x.Chart.Get(It.IsAny<Expression<Func<Chart, bool>>>())).Returns(charts.ToMockIQueryable());

//        _unitOfWork.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);

//        var unauthorizedIdentity = new AuthorizeIdentityMock("unauthorized@test.com", "unauthorized", "User", 3);
//        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, unauthorizedIdentity);

//        var exception = await Assert.ThrowsAsync<CustomException>(async () =>
//            await chartService.GetEmployeeChartsById(employeeId)
//        );

//        Assert.Equal("Unauthorized access.", exception.Message);
//    }

//    [Fact]
//    public async Task UpdateChart_ShouldUpdateAndReturnChart()
//    {
//        var chartDto = new ChartDto { Id = 1, EmployeeId = 1 };
//        _unitOfWork.Setup(uow => uow.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);
//        _unitOfWork.Setup(uow => uow.Chart.Update(It.IsAny<Chart>())).ReturnsAsync(new Chart());

//        var result = await _chartService.UpdateChart(chartDto);

//        Assert.NotNull(result);
//    }

//    [Fact]
//    public async Task UpdateChart_ShouldThrowUnauthorizedAccess_WhenUserIsNotSupportAndIdsDoNotMatch()
//    {
//        var chartDto = new ChartDto
//        {
//            Id = 2,
//            EmployeeId = _identity.EmployeeId
//        };

//        _unitOfWork.Setup(u => u.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
//                   .ReturnsAsync(true);

//        _unitOfWork.Setup(u => u.Chart.Update(It.IsAny<Chart>()))
//                   .Throws(new CustomException("Unauthorized access."));

//        await Assert.ThrowsAsync<CustomException>(async () => await _chartService.UpdateChart(chartDto));
//    }

//    //[Fact]
//    //public async Task DeleteChart_ShouldDeleteAndReturnChart()
//    //{
//    //    var chartId = 1;
//    //    _unitOfWork.Setup(uow => uow.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);
//    //    _unitOfWork.Setup(uow => uow.Chart.Delete(chartId)).ReturnsAsync(new Chart());

//    //    var result = await _chartService.DeleteChart(chartId);

//    //    Assert.NotNull(result);
//    //}

//    [Fact]
//    public async Task DeleteChart_DeletesChart_WhenAuthorized()
//    {
//        //    var chartId = 1;
//        //    var authorizedEmployeeId = 1;

//        //var chartMock = new Mock<IChartRepository>();
//        //chartMock.Setup(c => c.Delete(It.IsAny<int>())).ReturnsAsync(new Chart { Id = chartId, EmployeeId = authorizedEmployeeId });

//        _unitOfWork.Setup(m => m.Chart.Any(It.IsAny<Expression<Func<Chart, bool>>>())).ReturnsAsync(true);


//        var result = await _chartService.DeleteChart(1);

//        Assert.NotNull(result);
//        Assert.Equivalent(ChartDataSetTestData.ChartDataOne.ToDto(), result);


//        _unitOfWork.Verify(x => x.Chart.Delete(It.IsAny<int>()), Times.Once);
//    }

//    [Fact]
//    public async Task DeleteChart_ShouldThrowChatNotFound_WhenChatDoesNotExist()
//    {
//        var chartId = 1;

//        _unitOfWork.Setup(u => u.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
//                   .ReturnsAsync(true);

//        var exception = await Assert.ThrowsAsync<CustomException>(
//            async () => await _chartService.DeleteChart(chartId)
//        );

//        Assert.Equal("Chart not found", exception.Message);
//    }

//    [Fact]
//    public async Task DeleteChart_unauthorized()
//    {

//        //_unitOfWork.Setup(u => u.Chart).Returns(chartMock.Object);
//        //_unitOfWork.Setup(m => m.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);

//    }

//    [Fact]
//    public async Task GetChartDataTest()
//    {
//        var dataType = new List<string> { "Gender", "Race" };

//        var employeeOne = EmployeeTestData.EmployeeOne;

//        var employees = new List<Employee>
//        {
//            employeeOne
//        };

//        _employeeService.Setup(e => e.GetAll("")).ReturnsAsync(employees.Select(x => x.ToDto()).ToList());

//        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _identity);

//        var result = await chartService.GetChartData(dataType);

//        Assert.NotNull(result);
//        Assert.IsType<ChartDataDto>(result);
//    }

//    [Fact(Skip = "temp")]
//    public async Task UpdateChartTestFail()
//    {
//        var expectedChart = new Chart
//        {
//            Id = 1,
//            Name = "Update",
//            Type = "Pie",
//            DataTypes = new List<string> { "Gender", "Race" },
//            Labels = new List<string> { "Male", "Female" },
//            Roles = new List<string> { "All" },
//            Datasets = ChartDataSetTestData.ChartDataSetList
//        };

//        var existingCharts = new List<Chart>
//        {
//            new()
//            {
//                Id = 1,
//                Name = "Existing Chart",
//                Type = "Existing Type",
//                DataTypes = expectedChart.DataTypes,
//                Labels = expectedChart.Labels,
//                Roles = new List<string> { "All" },
//                Datasets = ChartDataSetTestData.ChartDataSetList
//            }
//        };

//        _unitOfWork.Setup(x => x.Chart.GetAll(null)).ReturnsAsync(existingCharts);

//        _unitOfWork.Setup(x => x.Chart.Update(It.IsAny<Chart>())).ReturnsAsync(expectedChart);

//        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _identity);

//        var result = await chartService.UpdateChart(expectedChart.ToDto());

//        Assert.NotNull(result);
//        Assert.Equivalent(expectedChart.ToDto(), result);
//        _unitOfWork.Verify(x => x.Chart.Update(It.IsAny<Chart>()), Times.Once);
//    }

//    [Fact]
//    public void GetColumnsFromTableTest()
//    {
//        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _identity);

//        var columnNames = chartService.GetColumnsFromTable();

//        Assert.NotNull(columnNames);
//        Assert.NotEmpty(columnNames);
//    }

//    [Fact(Skip = "Needs Work")]
//    public async Task ExportCsvAsyncTest()
//    {
//        var employeeSix = EmployeeTestData.EmployeeSix;

//        var employees = new List<Employee>
//        {
//            employeeSix
//        };

//        var dataTypeList = new List<string> { "Gender", "Race", "Age" };

//        _unitOfWork.Setup(e => e.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>()))
//                   .ReturnsAsync(employees);

//        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _identity);
//        var result = await chartService.ExportCsvAsync(dataTypeList);
//        var expectedResult = new byte[]
//        {
//            70, 105, 114, 115, 116, 32, 78, 97, 109, 101, 44, 76, 97, 115, 116, 32, 78, 97, 109, 101,
//            44, 65, 103, 101, 44, 71, 101, 110, 100, 101, 114, 44, 82, 97, 99, 101, 13, 10, 69, 115, 116, 105, 97, 97, 110,
//            44, 66, 114, 105, 116, 122, 44, 65, 103, 101, 32, 48, 44, 77, 97, 108, 101, 44, 66, 108, 97, 99, 107,
//            13, 10
//        };

//        Assert.NotNull(result);
//        Assert.IsType<byte[]>(result);
//        Assert.Equal(expectedResult, result);
//    }

//    [Fact(Skip = "temp")]
//    public async Task ExportCsvAsyncTestFail()
//    {
//        var dataTypeList = new List<string> { "", "" };

//        var employeeOne = EmployeeTestData.EmployeeOne;

//        var employees = new List<Employee>
//        {
//            employeeOne
//        };

//        _unitOfWork.Setup(e => e.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>()))
//                   .ReturnsAsync(employees);

//        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _identity);
//        _unitOfWork.Setup(x => x.ErrorLogging.Add(It.IsAny<ErrorLogging>()));
//        var exception = await Assert.ThrowsAsync<Exception>(async () => await chartService.ExportCsvAsync(dataTypeList));

//        Assert.Equal("Invalid property name: ", exception.Message);
//    }

//    [Fact]
//    public async Task CreateChart_ShouldThrowException_WhenChartDoesNotExist()
//    {
//        var employeeId = 1;
//        var dataTypes = new List<string> { "DataTypeOne", "DataTypeTwo" };
//        var roles = new List<string> { "RoleOne", "RoleTwo" };
//        var chartName = "TestChart";
//        var chartType = "standard";

//        _unitOfWork.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
//                   .ReturnsAsync(false);

//        var exception = await Assert.ThrowsAsync<CustomException>(async () =>
//            await _chartService.CreateChart(dataTypes, roles, chartName, chartType, employeeId)
//        );

//        Assert.Equal("Chat not found", exception.Message);
//    }

//    [Fact]
//    public async Task CreateChart_ShouldThrowUnauthorizedAccess_WhenUserIsNotAuthorized()
//    {
//        var employeeId = 2;
//        var dataTypes = new List<string> { "DataTypeOne", "DataTypeTwo" };
//        var roles = new List<string> { "RoleOne", "RoleTwo" };
//        var chartName = "TestChart";
//        var chartType = "standard";

//        _unitOfWork.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
//                   .ReturnsAsync(true);

//        var unauthorizedIdentity = new AuthorizeIdentityMock("unauthorized@test.com", "unauthorized", "User", 3);
//        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, unauthorizedIdentity);

//        var exception = await Assert.ThrowsAsync<CustomException>(async () =>
//            await chartService.CreateChart(dataTypes, roles, chartName, chartType, employeeId)
//        );

//        Assert.Equal("Unauthorized access.", exception.Message);
//    }

//    [Fact]
//    public async Task CreateChart_ShouldCreateStackedChart_WhenChartTypeIsStacked()
//    {
//        var employeeId = _identity.EmployeeId;
//        var dataTypes = new List<string> { "DataTypeOne" };
//        var roles = new List<string> { "Developer", "Designer" };
//        var chartName = "TestChart";
//        var chartType = "stacked";

//        _unitOfWork.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
//                   .ReturnsAsync(true);

//        _unitOfWork.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
//                   .Returns(new List<Employee>().AsQueryable().BuildMock());

//        _unitOfWork.Setup(x => x.Chart.Add(It.IsAny<Chart>()))
//                   .Returns(Task.FromResult(new Chart { Name = chartName, EmployeeId = employeeId }));

//        var result = await _chartService.CreateChart(dataTypes, roles, chartName, chartType, employeeId);

//        Assert.NotNull(result);
//        Assert.Equal(chartName, result.Name);
//        Assert.Equal("stacked", result.Subtype);
//        Assert.Equal("bar", result.Type);
//        Assert.True(result.Datasets.Count > 1);
//    }

//    [Fact]
//    public async Task CreateChart_ShouldCreateStandardChart_WhenChartTypeIsStandard()
//    {
//        var employeeId = _identity.EmployeeId;
//        var dataTypes = new List<string> { "DataTypeOne" };
//        var roles = new List<string> { "RoleOne" };
//        var chartName = "TestChart";
//        var chartType = "standard";

//        _unitOfWork.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
//                   .ReturnsAsync(true);

        
//        _unitOfWork.Setup(x => x.Chart.Add(It.IsAny<Chart>()))
//                   .Returns(Task.FromResult(new Chart { Name = chartName, EmployeeId = employeeId }));

//        var result = await _chartService.CreateChart(dataTypes, roles, chartName, chartType, employeeId);

//        Assert.NotNull(result);
//        Assert.Equal(chartName, result.Name);
//        Assert.Equal("standard", result.Subtype);
//        Assert.Single(result.Datasets);
//        Assert.Equal(dataTypes[0], result.Datasets[0].Label);
//    }

//    [Fact]
//    public async Task ExportCsvAsync_ShouldThrowUnauthorizedAccess_WhenUserIsNotSupport()
//    {
//        var nonSupportIdentity = new AuthorizeIdentityMock("user@test.com", "Regular User", "User", 1);
//        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, nonSupportIdentity);

//        var exception = await Assert.ThrowsAsync<CustomException>(async () =>
//            await chartService.ExportCsvAsync(new List<string> { "Name", "Surname" })
//        );

//        Assert.Equal("Unauthorized access.", exception.Message);
//    }

//    [Fact]
//    public async Task ExportCsvAsync_ShouldReturnCsvContent_WhenDataTypesAreValid()
//    {
//        var employees = new List<Employee>
//        {
//           new Employee { Name = "John", Surname = "Doe"},
//           new Employee { Name = "Jane", Surname = "Smith"}
//        };

//        _unitOfWork.Setup(x => x.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(employees);

//        var dataTypes = new List<string> { "Age" };

//        var result = await _chartService.ExportCsvAsync(dataTypes);

//        Assert.NotNull(result);
//        var csvContent = Encoding.UTF8.GetString(result);
//        var expectedCsv = "First Name,Last Name,Age\r\nJohn,Doe,30\r\nJane,Smith,25\r\n";
//        Assert.Equal(expectedCsv, csvContent);
//    }

//    //[Fact]
//    //public async Task ExportCsvAsync_ShouldThrowCustomException_WhenInvalidPropertyNameIsProvided()
//    //{
//    //    _unitOfWork.Setup(x => x.Employee.GetAll())
//    //               .ReturnsAsync(new List<Employee> { new Employee { Name = "John", Surname = "Doe" } });

//    //    var exception = await Assert.ThrowsAsync<CustomException>(async () =>
//    //        await _chartService.ExportCsvAsync(new List<string> { "InvalidProperty" })
//    //    );

//    //    Assert.Equal("Invalid property name: InvalidProperty", exception.Message);
//    //}

//    [Fact]
//    public async Task GetAllCharts_ShouldThrowCustomException_WhenUserIsUnauthorized()
//    {
//        _unitOfWork.Setup(x => x.Chart.Get(It.IsAny<Expression<Func<Chart, bool>>>()))
//                   .Returns(new List<Chart>().ToMockIQueryable());

//        var exception = await Assert.ThrowsAsync<CustomException>(async () =>
//            await _chartService.GetAllCharts()
//        );

//        Assert.Equal("Unauthorized access.", exception.Message);
//    }

//    [Fact]
//    public void GetColumnsFromTable_ShouldThrowCustomException_WhenUserIsNotSupport()
//    {
//        var employeeId = _identity.EmployeeId;

//        var exception = Assert.Throws<CustomException>(() =>
//            _chartService.GetColumnsFromTable()
//        );

//        Assert.Equal("Unauthorized access.", exception.Message);
//    }
//}
