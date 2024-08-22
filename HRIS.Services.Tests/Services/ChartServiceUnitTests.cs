using System.Data;
using System.Linq.Expressions;
using System.Text;
using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using MockQueryable.Moq;
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
    private readonly ChartService _chartService;
    private readonly Mock<IEmployeeService> _employeeService;
    private readonly Mock<IServiceProvider> _services;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly AuthorizeIdentityMock _identity;
    private readonly Employee _testEmployee;
    private readonly Mock<IDataTypeProvider> _dataTypeProvider;

    public ChartServiceUnitTests()
    {
        _unitOfWork = new Mock<IUnitOfWork>();
        _employeeService = new Mock<IEmployeeService>();
        _services = new Mock<IServiceProvider>();
        _identity = new AuthorizeIdentityMock("test@gmail.com", "test", "Admin", 1);
        _dataTypeProvider = new Mock<IDataTypeProvider>();

        _testEmployee = EmployeeTestData.EmployeeOne;

        _chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _identity, _dataTypeProvider.Object);
    }

    [Fact]
    public async Task CheckIfChatsExists_ShouldReturnFalse_WhenChartsDoesNotExist()
    {
        var employeeId = 1;
        _unitOfWork.Setup(ex => ex.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(false);

        var result = await _chartService.CheckIfChatsExists(employeeId);

        Assert.False(result);
    }

    [Fact]
    public async Task CheckIfChatsExists_ShouldReturnTrue_WhenEmployeeExists()
    {
        var employeeId = 2;
        _unitOfWork.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);

        var result = await _chartService.CheckIfChatsExists(employeeId);

        Assert.True(result);
    }

    [Fact]
    public async Task GetAllChartsTest()
    {
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _identity, _dataTypeProvider.Object);

        _unitOfWork.Setup(u => u.Chart.Get(It.IsAny<Expression<Func<Chart, bool>>>())).Returns(new List<Chart>().ToMockIQueryable());

        var result = await chartService.GetAllCharts();

        Assert.NotNull(result);
        Assert.IsType<List<ChartDto>>(result);
        _unitOfWork.Verify(u => u.Chart.Get(null), Times.Once);
    }

    [Fact]
    public async Task GetAllCharts_ShouldReturnEmptyList_WhenNoChartsAvailable()
    {
        var Charts = new List<Chart>();

        _unitOfWork.Setup(x => x.Chart.Get(It.IsAny<Expression<Func<Chart, bool>>>())).Returns(new List<Chart>().ToMockIQueryable());

        var result = await _chartService.GetAllCharts();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllCharts_ShouldReturnCapitalizedDataTypes_WhenAuthorized()
    {
        var employeeId = _identity.EmployeeId;

        var chart = new Chart
        {
            EmployeeId = employeeId,
            DataTypes = new List<string> { "DataTypeOne", "DataTypeTwo" }
        };

        var charts = new List<Chart> { chart };

        _unitOfWork.Setup(u => u.Chart.Get(It.IsAny<Expression<Func<Chart, bool>>>())).Returns(charts.ToMockIQueryable());

        var result = await _chartService.GetAllCharts();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Data Type One", result[0].DataTypes[0]);
        Assert.Equal("Data Type Two", result[0].DataTypes[1]);
    }

    [Fact]
    public async Task GetEmployeeChartsById_ShouldThrowException_WhenChartsDoNotExist()
    {
        var employeeId = 1;

        _unitOfWork.Setup(x => x.Chart.Get(It.IsAny<Expression<Func<Chart, bool>>>()))
                   .Returns(new List<Chart>().ToMockIQueryable());

        _unitOfWork.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<CustomException>(async () =>
            await _chartService.GetEmployeeChartsById(employeeId)
        );

        Assert.Equal("Chat not found", exception.Message);
    }

    [Fact]
    public async Task GetEmployeeChartsById_ShouldReturnChartsWithCapitalizedDataTypes_WhenUserIsAuthorized()
    {
        var employeeId = _identity.EmployeeId;
        var chartDataSet = new ChartDataSet();
        var chart = new Chart
        {
            EmployeeId = employeeId,
            DataTypes = new List<string> { "DataTypeOne", "DataTypeTwo" },
            Datasets = new List<ChartDataSet> { chartDataSet }
        };

        var charts = new List<Chart> { chart };

        _unitOfWork.Setup(x => x.Chart.Get(It.IsAny<Expression<Func<Chart, bool>>>()))
                   .Returns(charts.AsQueryable().BuildMock());

        _unitOfWork.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(true);

        var result = await _chartService.GetEmployeeChartsById(employeeId);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Data Type One", result[0].DataTypes[0]);
        Assert.Equal("Data Type Two", result[0].DataTypes[1]);
        Assert.NotNull(result[0].Datasets);
        Assert.Single(result[0].Datasets);
    }

    [Fact]
    public async Task GetEmployeeChartsById_ShouldThrowUnauthorizedAccess_WhenUserIsNotAuthorized()
    {
        var employeeId = 2;

        var charts = new List<Chart>
        {
          new Chart { Id = 1, EmployeeId = employeeId }
        };

        _unitOfWork.Setup(x => x.Chart.Get(It.IsAny<Expression<Func<Chart, bool>>>())).Returns(charts.ToMockIQueryable());

        _unitOfWork.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);

        var unauthorizedIdentity = new AuthorizeIdentityMock("unauthorized@test.com", "unauthorized", "User", 3);
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, unauthorizedIdentity, _dataTypeProvider.Object);

        var exception = await Assert.ThrowsAsync<CustomException>(async () =>
            await chartService.GetEmployeeChartsById(employeeId)
        );

        Assert.Equal("Unauthorized access.", exception.Message);
    }

    [Fact]
    public async Task UpdateFailTest()
    {
        var chartDto = new ChartDto { Id = 1, EmployeeId = 1 };
        _unitOfWork.Setup(uow => uow.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(false);

        _unitOfWork.Setup(x => x.Chart.GetById(It.IsAny<int>())).ReturnsAsync((Chart?)null);

        await Assert.ThrowsAsync<CustomException>(() => _chartService.UpdateChart(chartDto));
    }

    [Fact]
    public async Task UpdateChart_ShouldUpdateAndReturnChart()
    {
        var chartDto = new ChartDto { Id = 1, EmployeeId = 1 };
        _unitOfWork.Setup(uow => uow.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);
        _unitOfWork.Setup(uow => uow.Chart.Update(It.IsAny<Chart>())).ReturnsAsync(new Chart());

        var result = await _chartService.UpdateChart(chartDto);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateUnauthorizedTest()
    {
        var chartDto = new ChartDto { Id = 1, EmployeeId = 1 };
        _unitOfWork.Setup(uow => uow.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);
        _unitOfWork.Setup(uow => uow.Chart.Update(It.IsAny<Chart>())).ReturnsAsync(new Chart());


        var unauthorizedIdentity = new AuthorizeIdentityMock("test@gmail.com", "test", "User", 2);
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, unauthorizedIdentity, _dataTypeProvider.Object);

        var exception = await Assert.ThrowsAsync<CustomException>(() => chartService.UpdateChart(chartDto));

        Assert.Equal("Unauthorized access.", exception.Message);
    }

    [Fact]
    public async Task UpdateChart_ShouldThrowUnauthorizedAccess_WhenUserIsNotSupportAndIdsDoNotMatch()
    {
        var chartDto = new ChartDto
        {
            Id = 2,
            EmployeeId = _identity.EmployeeId
        };

        _unitOfWork.Setup(u => u.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(true);

        _unitOfWork.Setup(u => u.Chart.Update(It.IsAny<Chart>()))
                   .Throws(new CustomException("Unauthorized access."));

        await Assert.ThrowsAsync<CustomException>(async () => await _chartService.UpdateChart(chartDto));
    }

    [Fact]
    public async Task DeleteChart_ShouldDeleteAndReturnChart()
    {
        var chartId = 1;
        _unitOfWork.Setup(uow => uow.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);
        _unitOfWork.Setup(uow => uow.Chart.Delete(chartId)).ReturnsAsync(new Chart());

        var result = await _chartService.DeleteChart(chartId);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeleteFailTest()
    {
        var chartId = 1;
        _unitOfWork.Setup(uow => uow.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(false);
        _unitOfWork.Setup(ex => ex.Chart.Delete(It.IsAny<int>())).ReturnsAsync((Chart?)null);

        await Assert.ThrowsAsync<CustomException>(() => _chartService.DeleteChart(chartId));
    }

    [Fact]
    public async Task DeleteUnauthorizedTest()
    {
        var chartId = 1;
        var chart = new Chart { Id = chartId, EmployeeId = 2 };
        _unitOfWork.Setup(uow => uow.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);

        _unitOfWork.Setup(x => x.Chart.Any(It.IsAny<Expression<Func<Chart, bool>>>())).ReturnsAsync(true);

        _unitOfWork.Setup(x => x.Chart.Get(It.IsAny<Expression<Func<Chart, bool>>>())).Returns(new List<Chart> { chart }.ToMockIQueryable());

        _unitOfWork.Setup(x => x.Chart.Delete(chartId)).ReturnsAsync(chart);

        var unauthorizedIdentity = new AuthorizeIdentityMock("test@gmail.com", "test", "User", 2);
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, unauthorizedIdentity, _dataTypeProvider.Object);

        var exception = await Assert.ThrowsAsync<CustomException>(() => chartService.DeleteChart(chartId));

        Assert.Equal("Unauthorized access.", exception.Message);
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

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _identity, _dataTypeProvider.Object);

        var result = await chartService.GetChartData(dataType);

        Assert.NotNull(result);
        Assert.IsType<ChartDataDto>(result);
    }

    [Fact]
    public async Task GetChartDataUnauthorized()
    {
        var dataType = new List<string> { "Gender", "Race" };

        var employeeOne = EmployeeTestData.EmployeeOne;

        var employees = new List<Employee>
        {
            employeeOne
        };


        var unauthorizedIdentity = new AuthorizeIdentityMock("test@gmail.com", "test", "User", 2);
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, unauthorizedIdentity, _dataTypeProvider.Object);

        var exception = await Assert.ThrowsAsync<CustomException>(() => chartService.GetChartData(dataType));

        Assert.Equal("Unauthorized access.", exception.Message);
    }

    [Fact]
    public void GetColumnsFromTableTest()
    {
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, _identity, _dataTypeProvider.Object);

        var columnNames = chartService.GetColumnsFromTable();

        Assert.NotNull(columnNames);
        Assert.NotEmpty(columnNames);
    }

    [Fact]
    public void GetColumnsFromTable_ShouldThrowUnauthorizedAccess()
    {
        var unauthorizedIdentity = new AuthorizeIdentityMock("test@gmail.com", "test", "User", 2);
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, unauthorizedIdentity, _dataTypeProvider.Object);

        var exception = Assert.Throws<CustomException>(() => chartService.GetColumnsFromTable());

        Assert.Equal("Unauthorized access.", exception.Message);
    }

    [Fact]
    public async Task CreateChart_ShouldThrowException_WhenChartDoesNotExist()
    {
        var employeeId = 1;
        var dataTypes = new List<string> { "DataTypeOne", "DataTypeTwo" };
        var roles = new List<string> { "RoleOne", "RoleTwo" };
        var chartName = "TestChart";
        var chartType = "standard";

        _unitOfWork.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<CustomException>(async () =>
            await _chartService.CreateChart(dataTypes, roles, chartName, chartType, employeeId)
        );

        Assert.Equal("Chat not found", exception.Message);
    }

    [Fact]
    public async Task CreateChart_ShouldThrowUnauthorizedAccess_WhenUserIsNotAuthorized()
    {
        var employeeId = 2;
        var dataTypes = new List<string> { "DataTypeOne", "DataTypeTwo" };
        var roles = new List<string> { "RoleOne", "RoleTwo" };
        var chartName = "TestChart";
        var chartType = "standard";

        _unitOfWork.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(true);

        var unauthorizedIdentity = new AuthorizeIdentityMock("unauthorized@test.com", "unauthorized", "User", 3);
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, unauthorizedIdentity, _dataTypeProvider.Object);

        var exception = await Assert.ThrowsAsync<CustomException>(async () =>
            await chartService.CreateChart(dataTypes, roles, chartName, chartType, employeeId)
        );

        Assert.Equal("Unauthorized access.", exception.Message);
    }

    [Fact]
    public async Task CreateChart_ShouldCreateStackedChart_WhenChartTypeIsStacked()
    {
        var employeeId = _identity.EmployeeId;
        var dataTypes = new List<string> { "DataTypeOne" };
        var roles = new List<string> { "Developer", "Designer" };
        var chartName = "TestChart";
        var chartType = "stacked";

        _unitOfWork.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(true);

        _unitOfWork.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(new List<Employee>().AsQueryable().BuildMock());

        _unitOfWork.Setup(x => x.Chart.Add(It.IsAny<Chart>()))
                   .Returns(Task.FromResult(new Chart { Name = chartName, EmployeeId = employeeId, Subtype = chartType, Type = "bar" }));

        var result = await _chartService.CreateChart(dataTypes, roles, chartName, chartType, employeeId);

        Assert.NotNull(result);
        Assert.Equal(chartName, result.Name);
        Assert.Equal("stacked", result.Subtype);
        Assert.Equal("bar", result.Type);
    }

    [Fact]
    public async Task CreateChart_ShouldCreateStandardChart_WhenChartTypeIsStandard()
    {
        var employeeId = _identity.EmployeeId;
        var dataTypes = new List<string> { "DataTypeOne" };
        var roles = new List<string> { "RoleOne" };
        var chartName = "TestChart";
        var chartType = "standard";

        _unitOfWork.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(true);

        var mockEmployeesQueryable = new List<Employee>().AsQueryable().BuildMock();
        _unitOfWork.Setup(x => x.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .Returns(mockEmployeesQueryable);

        _unitOfWork.Setup(x => x.Chart.Add(It.IsAny<Chart>()))
                   .ReturnsAsync(new Chart { Name = chartName, EmployeeId = employeeId, Subtype = chartType});

        var result = await _chartService.CreateChart(dataTypes, roles, chartName, chartType, employeeId);

        Assert.NotNull(result);
        Assert.Equal(chartName, result.Name);
        Assert.Equal("standard", result.Subtype);
    }

    [Fact]
    public async Task ExportCsvAsync_ShouldThrowCustomException_WhenPropertyNameIsInvalid()
    {
        var dataTypes = new List<string> { "InvalidProperty" };
        var supportIdentity = new AuthorizeIdentityMock("admin@test.com", "password", "Admin", 1);
        var employees = new List<Employee>();

        var mockDataTypeProvider = new Mock<IDataTypeProvider>();
        mockDataTypeProvider.Setup(p => p.GetDataTypes()).Returns(new List<BaseDataType>());

        _unitOfWork.Setup(u => u.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(employees);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, supportIdentity, mockDataTypeProvider.Object);

        var exception = await Assert.ThrowsAsync<CustomException>(() => chartService.ExportCsvAsync(dataTypes));
        Assert.Equal("Invalid property name: InvalidProperty", exception.Message);
    }

    [Fact]
    public async Task ExportCsvAsync_ShouldThrowCustomException_WhenDataTypesListIsEmpty()
    {
        var dataTypes = new List<string>();
        var supportIdentity = new AuthorizeIdentityMock("admin@test.com", "password", "Admin", 1);
        var employees = new List<Employee>
    {
        new Employee
        {
            Name = "John",
            Surname = "Doe",
            DateOfBirth = new DateTime(1994, 1, 1),
        }
    };

        _unitOfWork.Setup(u => u.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(employees);

        var exception = await Assert.ThrowsAsync<CustomException>(() => _chartService.ExportCsvAsync(dataTypes));
        Assert.Equal("Data types list is empty or null.", exception.Message);
    }

    [Fact]
    public async Task ExportCsvAsync_ShouldHandleNoEmployees()
    {
        var dataTypes = new List<string> { "Age" };
        var supportIdentity = new AuthorizeIdentityMock("admin@test.com", "password", "Admin", 1);
        var employees = new List<Employee>();

        _unitOfWork.Setup(u => u.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(employees);

        var csvResult = await _chartService.ExportCsvAsync(dataTypes);

        var expectedCsv = new StringBuilder();
        expectedCsv.AppendLine("First Name,Last Name,Age");

        Assert.NotNull(csvResult);
        var expectedResult = expectedCsv.ToString();
        var actualResult = Encoding.UTF8.GetString(csvResult);
        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public async Task ExportCsvAsync_ShouldThrowUnauthorizedAccess_WhenUserIsNotSupport()
    {
        var dataTypes = new List<string> { "Age", "Department" };
        var nonSupportIdentity = new AuthorizeIdentityMock("user@test.com", "Regular User", "User", 1);
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, nonSupportIdentity, _dataTypeProvider.Object);

        var exception = await Assert.ThrowsAsync<CustomException>(async () => await chartService.ExportCsvAsync(dataTypes));
        Assert.Equal("Unauthorized access.", exception.Message);
    }

    [Fact]
    public async Task ExportCsvAsync_ShouldExportValidCsvContent_WhenUserIsSupport()
    {
        var dataTypes = new List<string> { "Age" };
        var supportIdentity = new AuthorizeIdentityMock("admin@test.com", "password", "Admin", 1);
        var employees = new List<Employee>
    {
        new Employee
        {
            Name = "John",
            Surname = "Doe",
            DateOfBirth = new DateTime(1994, 1, 1)
        }
    };

        var mockAgeType = new Mock<BaseDataType>();
        mockAgeType.Setup(x => x.Name).Returns("Age");
        mockAgeType.Setup(x => x.GenerateData(It.IsAny<EmployeeDto>(), It.IsAny<IServiceProvider>()))
                   .Returns("Age 30");

        var mockDataTypeProvider = new Mock<IDataTypeProvider>();
        mockDataTypeProvider.Setup(x => x.GetDataTypes())
                            .Returns(new List<BaseDataType> { mockAgeType.Object });

        _unitOfWork.Setup(u => u.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(employees);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, supportIdentity, mockDataTypeProvider.Object);

        var csvResult = await chartService.ExportCsvAsync(dataTypes);

        var expectedCsv = new StringBuilder();
        expectedCsv.AppendLine("First Name,Last Name,Age");
        expectedCsv.AppendLine("John,Doe,Age 30");

        Assert.NotNull(csvResult);
        var expectedResult = expectedCsv.ToString();
        var actualResult = Encoding.UTF8.GetString(csvResult);

        expectedResult = expectedResult.Replace("\r\n", "\n");
        actualResult = actualResult.Replace("\r\n", "\n");

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public async Task ExportCsvAsync_ShouldHandleCustomDataTypes()
    {
        var dataTypes = new List<string> { "CustomDataType" };
        var supportIdentity = new AuthorizeIdentityMock("admin@test.com", "password", "Admin", 1);
        var employees = new List<Employee>
    {
        new Employee
        {
            Name = "Jane",
            Surname = "Smith"
        }
    };

        var mockBaseDataType = new Mock<BaseDataType>();
        mockBaseDataType.Setup(x => x.Name).Returns("CustomDataType");
        mockBaseDataType.Setup(x => x.GenerateData(It.IsAny<EmployeeDto>(), It.IsAny<IServiceProvider>()))
                         .Returns("Custom Value");

        var mockDataTypeProvider = new Mock<IDataTypeProvider>();
        mockDataTypeProvider.Setup(x => x.GetDataTypes())
                            .Returns(new List<BaseDataType> { mockBaseDataType.Object });

        _unitOfWork.Setup(u => u.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(employees);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, supportIdentity, mockDataTypeProvider.Object);

        var csvResult = await chartService.ExportCsvAsync(dataTypes);

        var expectedCsv = new StringBuilder();
        expectedCsv.AppendLine("First Name,Last Name,CustomDataType");
        expectedCsv.AppendLine("Jane,Smith,Custom Value");

        Assert.NotNull(csvResult);
        var expectedResult = expectedCsv.ToString();
        var actualResult = Encoding.UTF8.GetString(csvResult);

        expectedResult = expectedResult.Replace("\r\n", "\n");
        actualResult = actualResult.Replace("\r\n", "\n");

        Assert.Equal(expectedResult, actualResult);
    }

    [Fact]
    public async Task ExportCsvAsync_ShouldHandleNonCustomPropertiesCorrectly()
    {
        var dataTypes = new List<string> { "Name", "Surname" };
        var supportIdentity = new AuthorizeIdentityMock("admin@test.com", "password", "Admin", 1);
        var employees = new List<Employee>
    {
        new Employee
        {
            Name = "Alice",
            Surname = "Johnson"
        }
    };

        var mockDataTypeProvider = new Mock<IDataTypeProvider>();
        mockDataTypeProvider.Setup(x => x.GetDataTypes())
            .Returns(new List<BaseDataType>());

        _unitOfWork.Setup(u => u.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>()))
                   .ReturnsAsync(employees);

        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, supportIdentity, mockDataTypeProvider.Object);

        var csvResult = await chartService.ExportCsvAsync(dataTypes);

        var expectedCsv = new StringBuilder();
        expectedCsv.AppendLine("First Name,Last Name,Name,Surname");
        expectedCsv.AppendLine("Alice,Johnson,Alice,Johnson");

        Assert.NotNull(csvResult);
        var expectedResult = expectedCsv.ToString();
        var actualResult = Encoding.UTF8.GetString(csvResult);
        expectedResult = expectedResult.Replace("\r\n", "\n");
        actualResult = actualResult.Replace("\r\n", "\n");
        Assert.Equal(expectedResult, actualResult);
    }
}