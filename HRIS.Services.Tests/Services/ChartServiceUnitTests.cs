using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using HRIS.Models;
using HRIS.Services.Handler.Charts;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.EntityFrameworkCore;
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

    [Theory]
    [InlineData(1, false)]
    [InlineData(2, true)]
    public async Task CheckIfChartsExists_ShouldReturnExpectedResult(int employeeId, bool expectedResult)
    {
        _unitOfWork.Setup(ex => ex.Chart.Any(It.IsAny<Expression<Func<Chart, bool>>>())).ReturnsAsync(expectedResult);

        var result = await _chartService.CheckIfChartsExists(employeeId);

        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task GetAllCharts_ShouldReturnEmptyList_WhenNoChartsAvailable()
    {
        _unitOfWork.Setup(x => x.Chart.Get(It.IsAny<Expression<Func<Chart, bool>>>())).Returns(new List<Chart>().ToMockIQueryable());

        var result = await _chartService.GetAllCharts();

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetAllCharts_ShouldReturnCapitalizedDataTypes_WhenAuthorized()
    {
        var chart = new Chart
        {
            EmployeeId = _identity.EmployeeId,
            DataTypes = new List<string> { "DataTypeOne", "DataTypeTwo" }
        };

        _unitOfWork.Setup(u => u.Chart.Get(It.IsAny<Expression<Func<Chart, bool>>>())).Returns(new List<Chart> { chart }.ToMockIQueryable());

        var result = await _chartService.GetAllCharts();

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Data Type One", result[0].DataTypes[0]);
        Assert.Equal("Data Type Two", result[0].DataTypes[1]);
    }

    [Fact]
    public async Task GetEmployeeChartsById_ShouldReturnExpectedCharts_WhenUserIsAuthorized()
    {
        var chart = new Chart
        {
            EmployeeId = _identity.EmployeeId,
            DataTypes = new List<string> { "DataTypeOne", "DataTypeTwo" },
            Datasets = new List<ChartDataSet> { new ChartDataSet() }
        };

        _unitOfWork.Setup(x => x.Chart.Get(It.IsAny<Expression<Func<Chart, bool>>>())).Returns(new List<Chart> { chart }.ToMockIQueryable());
        _unitOfWork.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);
        _unitOfWork.Setup(x => x.Chart.Any(It.IsAny<Expression<Func<Chart, bool>>>())).ReturnsAsync(true);

        var result = await _chartService.GetEmployeeChartsById(_identity.EmployeeId);

        Assert.NotNull(result);
        Assert.Single(result);
        Assert.Equal("Data Type One", result[0].DataTypes[0]);
        Assert.Equal("Data Type Two", result[0].DataTypes[1]);
        Assert.Single(result[0].Datasets);
    }

    [Fact]
    public async Task GetEmployeeChartsById_ShouldThrowException_WhenChartsDoNotExist()
    {
        _unitOfWork.Setup(x => x.Chart.Get(It.IsAny<Expression<Func<Chart, bool>>>())).Returns(new List<Chart>().ToMockIQueryable());
        _unitOfWork.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(false);

        var exception = await Assert.ThrowsAsync<CustomException>(async () => await _chartService.GetEmployeeChartsById(1));

        Assert.Equal("Chat not found", exception.Message);
    }

    [Fact]
    public async Task GetEmployeeChartsById_ShouldThrowUnauthorizedAccess_WhenUserIsNotAuthorized()
    {
        var unauthorizedEmployeeId = 2;
        var unauthorizedIdentity = new AuthorizeIdentityMock("unauthorized@test.com", "password", "User", 3);
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, unauthorizedIdentity, _dataTypeProvider.Object);
        _unitOfWork.Setup(x => x.Chart.Any(It.IsAny<Expression<Func<Chart, bool>>>())).ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<CustomException>(() => chartService.GetEmployeeChartsById(unauthorizedEmployeeId));

        Assert.Equal("Unauthorized access.", exception.Message);
    }

    [Fact]
    public async Task UpdateChart_ShouldThrowException_WhenUnauthorized()
    {
        var chartDto = new ChartDto { Id = 1, EmployeeId = 1 };
        var unauthorizedIdentity = new AuthorizeIdentityMock("test@gmail.com", "test", "User", 2);
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, unauthorizedIdentity, _dataTypeProvider.Object);

        _unitOfWork.Setup(uow => uow.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(false);
        _unitOfWork.Setup(x => x.Chart.GetById(It.IsAny<int>())).ReturnsAsync((Chart?)null);
        _unitOfWork.Setup(x => x.Chart.Any(It.IsAny<Expression<Func<Chart, bool>>>())).ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<CustomException>(() => chartService.UpdateChart(chartDto));
        Assert.Equal("Unauthorized access.", exception.Message);
    }

    [Fact]
    public async Task UpdateChart_ShouldThrowUnauthorizedAccess_WhenUserIsNotSupportAndIdsDoNotMatch()
    {
        var chartDto = new ChartDto { Id = 2, EmployeeId = _identity.EmployeeId };

        _unitOfWork.Setup(u => u.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);
        _unitOfWork.Setup(u => u.Chart.Update(It.IsAny<Chart>())).Throws(new CustomException("Unauthorized access."));

        await Assert.ThrowsAsync<CustomException>(async () => await _chartService.UpdateChart(chartDto));
    }

    [Fact]
    public async Task UpdateChart_ShouldThrowException_WhenChartDoesNotExist()
    {
        var chartDto = new ChartDto { Id = 1, EmployeeId = 1 };

        _unitOfWork.Setup(uow => uow.Chart.GetById(chartDto.Id)).ReturnsAsync((Chart?)null);
        _unitOfWork.Setup(uow => uow.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<CustomException>(() => _chartService.UpdateChart(chartDto));
        Assert.Equal("No chart data record found", exception.Message);
    }

    [Fact]
    public async Task UpdateChart_ShouldUpdateChart_WhenChartExistsAndUserIsAuthorized()
    {
        var chartDto = new ChartDto { Id = 1, EmployeeId = 1 };
        var authorizedIdentity = new AuthorizeIdentityMock("admin@test.com", "password", "Admin", 1);
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, authorizedIdentity, _dataTypeProvider.Object);

        var existingChart = new Chart { Id = chartDto.Id, EmployeeId = chartDto.EmployeeId };
        _unitOfWork.Setup(x => x.Chart.Any(It.IsAny<Expression<Func<Chart, bool>>>())).ReturnsAsync(true);
        _unitOfWork.Setup(uow => uow.Chart.GetById(chartDto.Id)).ReturnsAsync(existingChart);
        _unitOfWork.Setup(uow => uow.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(true);
        _unitOfWork.Setup(uow => uow.Chart.Update(It.IsAny<Chart>())).ReturnsAsync(existingChart);

        var result = await chartService.UpdateChart(chartDto);

        Assert.NotNull(result);
        Assert.Equal(chartDto.Id, result.Id);
    }

    [Fact]
    public async Task DeleteChart_ShouldThrowUnauthorizedAccess_WhenUserIsNotAuthorized()
    {
        var chartId = 1;
        var unauthorizedIdentity = new AuthorizeIdentityMock("test@gmail.com", "test", "User", 2);
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, unauthorizedIdentity, _dataTypeProvider.Object);
        _unitOfWork.Setup(x => x.Chart.Any(It.IsAny<Expression<Func<Chart, bool>>>())).ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<CustomException>(() => chartService.DeleteChart(chartId));

        Assert.Equal("Unauthorized access.", exception.Message);
    }

    [Fact]
    public async Task DeleteChart_ShouldThrowException_WhenChartDoesNotExist()
    {
        var chartId = 1;
        _unitOfWork.Setup(x => x.Employee.Any(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(false);
        _unitOfWork.Setup(u => u.Chart.GetById(chartId)).ReturnsAsync((Chart?)null);
        var exception = new CustomException("Chart not found");
        _unitOfWork.Setup(x => x.Chart.Delete(It.IsAny<int>())).Throws(exception);

        var result = await Assert.ThrowsAsync<CustomException>(() => _chartService.DeleteChart(chartId));
        Assert.Equal("Chart not found", result.Message);
    }

    [Fact]
    public async Task DeleteChart_ShouldReturnChartDto_WhenChartExistsAndUserIsAuthorized()
    {
        var chartId = 1;
        var authorizedIdentity = new AuthorizeIdentityMock("admin@test.com", "password", "Admin", 1);
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, authorizedIdentity, _dataTypeProvider.Object);

        _unitOfWork.Setup(x => x.Chart.Any(It.IsAny<Expression<Func<Chart, bool>>>())).ReturnsAsync(true);

        var chart = new Chart { Id = chartId };
        _unitOfWork.Setup(u => u.Chart.GetById(chartId)).ReturnsAsync(chart);
        _unitOfWork.Setup(x => x.Chart.Delete(chartId)).ReturnsAsync(chart);

        var result = await chartService.DeleteChart(chartId);

        Assert.NotNull(result);
        Assert.Equal(chart.Id, result.Id);
    }

    [Fact]
    public async Task CreateChart_ShouldThrowException_WhenChartDoesNotExistOrUnauthorized()
    {
        var employeeId = 1;
        var dataTypes = new List<string> { "DataTypeOne", "DataTypeTwo" };
        var roles = new List<string> { "RoleOne", "RoleTwo" };
        var chartName = "TestChart";
        var chartType = "standard";

        _unitOfWork.Setup(x => x.Chart.Any(It.IsAny<Expression<Func<Chart, bool>>>())).ReturnsAsync(false);

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

        _unitOfWork.Setup(x => x.Chart.Any(It.IsAny<Expression<Func<Chart, bool>>>())).ReturnsAsync(true);

        var unauthorizedIdentity = new AuthorizeIdentityMock("unauthorized@test.com", "unauthorized", "User", 3);
        var chartService = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, unauthorizedIdentity, _dataTypeProvider.Object);

        var exception = await Assert.ThrowsAsync<CustomException>(async () =>
            await chartService.CreateChart(dataTypes, roles, chartName, chartType, employeeId)
        );

        Assert.Equal("Unauthorized access.", exception.Message);
    }

    //[Fact]
    //public async Task CreateChart_ShouldCreateStackedChart_WhenChartTypeIsStacked()
    //{
    //    var employeeId = 1;
    //    var dataTypes = new List<string> { "DataTypeOne", "DataTypeTwo" };
    //    var roles = new List<string> { "RoleOne", "RoleTwo" };
    //    var chartName = "TestStackedChart";
    //    var chartType = "stacked";

    //    _unitOfWork.Setup(x => x.Chart.Any(It.IsAny<Expression<Func<Chart, bool>>>())).ReturnsAsync(true);
    //    _unitOfWork.Setup(u => u.Chart.Add(It.IsAny<Chart>())).ReturnsAsync(new Chart { Id = 1 });
    //    var employees = new List<Employee>
    //{
    //    new Employee
    //    {
    //        Name = "John",
    //        EmployeeType = new EmployeeType { Name = "Admin" }
    //    },
    //    new Employee
    //    {
    //        Name = "Jane",
    //        EmployeeType = new EmployeeType { Name = "User" }
    //    }
    //};

    //    // Mock GetAll to return a List<Employee>
    //    _unitOfWork.Setup(u => u.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>()))
    //               .ReturnsAsync(employees);
    //    var result = await _chartService.CreateChart(dataTypes, roles, chartName, chartType, employeeId);

    //    Assert.NotNull(result);
    //    Assert.Equal(1, result.Id);
    //    Assert.Equal("TestStackedChart", result.Name);
    //    Assert.Equal("stacked", result.Type);
    //}


    //[Fact]
    //public async Task CreateChart_ShouldCreateStandardChart_WhenChartTypeIsStandard()
    //{
    //    var employeeId = 1;
    //    var dataTypes = new List<string> { "DataTypeOne", "DataTypeTwo" };
    //    var roles = new List<string> { "RoleOne", "RoleTwo" };
    //    var chartName = "TestStandardChart";
    //    var chartType = "standard";

    //    _unitOfWork.Setup(x => x.Chart.Any(It.IsAny<Expression<Func<Chart, bool>>>())).ReturnsAsync(true);
    //    _unitOfWork.Setup(u => u.Chart.Add(It.IsAny<Chart>())).ReturnsAsync(new Chart { Id = 2 });

    //    var result = await _chartService.CreateChart(dataTypes, roles, chartName, chartType, employeeId);

    //    Assert.NotNull(result);
    //    Assert.Equal(2, result.Id);
    //    Assert.Equal("TestStandardChart", result.Name);
    //    Assert.Equal("standard", result.Type);
    //}

    //[Fact]
    //public async Task CreateChart_ShouldHandleRolesWithSpacesAndCommas()
    //{
    //    var employeeId = 1;
    //    var dataTypes = new List<string> { "DataTypeOne", "DataTypeTwo" };
    //    var roles = new List<string> { "Role One, Role Two" };
    //    var chartName = "ChartWithComplexRoles";
    //    var chartType = "standard";

    //    _unitOfWork.Setup(x => x.Chart.Any(It.IsAny<Expression<Func<Chart, bool>>>())).ReturnsAsync(true);
    //    _unitOfWork.Setup(u => u.Chart.Add(It.IsAny<Chart>())).ReturnsAsync(new Chart { Id = 3 });

    //    var result = await _chartService.CreateChart(dataTypes, roles, chartName, chartType, employeeId);

    //    Assert.NotNull(result);
    //    Assert.Equal(3, result.Id);
    //    Assert.Equal("ChartWithComplexRoles", result.Name);
    //    Assert.Contains("Role One", result.Roles);
    //    Assert.Contains("Role Two", result.Roles);
    //}

    //[Fact]
    //public async Task CreateChart_ShouldCreateChartWithAllRoles_WhenRoleIsAll()
    //{
    //    var employeeId = 1;
    //    var dataTypes = new List<string> { "DataTypeOne" };
    //    var roles = new List<string> { "All" };
    //    var chartName = "ChartWithAllRoles";
    //    var chartType = "standard";

    //    _unitOfWork.Setup(x => x.Chart.Any(It.IsAny<Expression<Func<Chart, bool>>>())).ReturnsAsync(true);
    //    _unitOfWork.Setup(u => u.Chart.Add(It.IsAny<Chart>())).ReturnsAsync(new Chart { Id = 4 });

    //    var result = await _chartService.CreateChart(dataTypes, roles, chartName, chartType, employeeId);

    //    Assert.NotNull(result);
    //    Assert.Equal(4, result.Id);
    //    Assert.Equal("ChartWithAllRoles", result.Name);
    //    Assert.Contains("All", result.Roles);
    //}

    //[Fact]
    //public async Task CreateChart_ShouldHandleEmptyDataTypesList()
    //{
    //    var employeeId = 1;
    //    var dataTypes = new List<string>();
    //    var roles = new List<string> { "RoleOne" };
    //    var chartName = "ChartWithNoDataTypes";
    //    var chartType = "standard";

    //    _unitOfWork.Setup(x => x.Chart.Any(It.IsAny<Expression<Func<Chart, bool>>>())).ReturnsAsync(true);
    //    _unitOfWork.Setup(u => u.Chart.Add(It.IsAny<Chart>())).ReturnsAsync(new Chart { Id = 5 });

    //    var result = await _chartService.CreateChart(dataTypes, roles, chartName, chartType, employeeId);

    //    Assert.NotNull(result);
    //    Assert.Equal(5, result.Id);
    //    Assert.Equal("ChartWithNoDataTypes", result.Name);
    //    Assert.Empty(result.DataTypes);
    //}

    //[Fact]
    //public async Task CreateChart_ShouldHandleUnknownPropertiesInDataTypes()
    //{
    //    var employeeId = 1;
    //    var dataTypes = new List<string> { "UnknownProperty" };
    //    var roles = new List<string> { "RoleOne" };
    //    var chartName = "ChartWithUnknownProperties";
    //    var chartType = "standard";

    //    _unitOfWork.Setup(x => x.Chart.Any(It.IsAny<Expression<Func<Chart, bool>>>())).ReturnsAsync(true);
    //    _unitOfWork.Setup(u => u.Chart.Add(It.IsAny<Chart>())).ReturnsAsync(new Chart { Id = 6 });

    //    var result = await _chartService.CreateChart(dataTypes, roles, chartName, chartType, employeeId);

    //    Assert.NotNull(result);
    //    Assert.Equal(6, result.Id);
    //    Assert.Equal("ChartWithUnknownProperties", result.Name);
    //    Assert.Contains("UnknownProperty", result.DataTypes);
    //}

    //[Fact]
    //public async Task CreateChart_ShouldHandleAllRolesWhenRoleListIsEmpty()
    //{
    //    var employeeId = 1;
    //    var dataTypes = new List<string> { "DataTypeOne" };
    //    var roles = new List<string>();
    //    var chartName = "ChartWithEmptyRoles";
    //    var chartType = "standard";

    //    _unitOfWork.Setup(x => x.Chart.Any(It.IsAny<Expression<Func<Chart, bool>>>())).ReturnsAsync(true);
    //    _unitOfWork.Setup(u => u.Chart.Add(It.IsAny<Chart>())).ReturnsAsync(new Chart { Id = 7 });

    //    var result = await _chartService.CreateChart(dataTypes, roles, chartName, chartType, employeeId);

    //    Assert.NotNull(result);
    //    Assert.Equal(7, result.Id);
    //    Assert.Equal("ChartWithEmptyRoles", result.Name);
    //    Assert.Contains("All", result.Roles);
    //}


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
        var dataTypes = new List<string>(); // Empty data types list
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

    [Fact]
    public async Task GetChartData_ShouldThrowUnauthorizedAccess_WhenUserIsNotSupport()
    {
        var dataTypes = new List<string> { "Age" };
        var unauthorizedIdentity = new AuthorizeIdentityMock("user@test.com", "password", "User", 1);
        var service = new ChartService(_unitOfWork.Object, _employeeService.Object, _services.Object, unauthorizedIdentity, _dataTypeProvider.Object);

        var exception = await Assert.ThrowsAsync<CustomException>(async () => await service.GetChartData(dataTypes));
        Assert.Equal("Unauthorized access.", exception.Message);
    }

    [Fact]
    public async Task GetChartData_ShouldReturnEmptyChartData_WhenNoEmployees()
    {
        var dataTypes = new List<string> { "Age" };

        _employeeService.Setup(es => es.GetAll("")).ReturnsAsync(new List<EmployeeDto>());

        var result = await _chartService.GetChartData(dataTypes);

        Assert.NotNull(result);
        Assert.Empty(result.Labels);
        Assert.Empty(result.Data);
    }

    [Fact]
    public async Task GetChartData_ShouldReturnCorrectChartData_WhenSingleEmployeeAndSingleDataType()
    {
        var dataTypes = new List<string> { "Age" };
        var employees = new List<EmployeeDto>
    {
        new EmployeeDto
        {
            Name = "John",
            Surname = "Doe",
            DateOfBirth = new DateTime(1994, 1, 1),
        }
    };

        _employeeService.Setup(es => es.GetAll("")).ReturnsAsync(employees);

        var result = await _chartService.GetChartData(dataTypes);

        Assert.NotNull(result);
        Assert.Single(result.Labels);
        Assert.Single(result.Data);

        Assert.Equal("30", result.Labels[0]);
        Assert.Equal(1, result.Data[0]);
    }

    [Fact]
    public async Task GetChartData_ShouldCountOccurrencesCorrectly_WhenMultipleEmployeesHaveSameData()
    {
        var dataTypes = new List<string> { "Age" };
        var employees = new List<EmployeeDto>
    {
        new EmployeeDto { DateOfBirth = new DateTime(1994, 1, 1) },
        new EmployeeDto { DateOfBirth = new DateTime(1994, 1, 1) },
    };

        _employeeService.Setup(es => es.GetAll("")).ReturnsAsync(employees);

        var result = await _chartService.GetChartData(dataTypes);

        Assert.NotNull(result);
        Assert.Single(result.Labels);
        Assert.Single(result.Data);
        Assert.Equal("30", result.Labels[0]);
        Assert.Equal(2, result.Data[0]);
    }

    [Fact]
    public async Task GetChartData_ShouldProcessMultipleDataTypesCorrectly()
    {
        var dataTypes = new List<string> { "Age", "Name" };
        var employees = new List<EmployeeDto>
    {
        new EmployeeDto { DateOfBirth = new DateTime(1994, 1, 1), Name = "John" },
        new EmployeeDto { DateOfBirth = new DateTime(1994, 1, 1), Name = "Jane" }
    };

        _employeeService.Setup(es => es.GetAll("")).ReturnsAsync(employees);

        var result = await _chartService.GetChartData(dataTypes);

        Assert.NotNull(result);
        Assert.Equal(2, result.Labels.Count);
        Assert.Equal(2, result.Data.Count);

        var expectedLabel1 = "30John";
        var expectedLabel2 = "30Jane";

        Assert.Contains(expectedLabel1, result.Labels);
        Assert.Contains(expectedLabel2, result.Labels);

        Assert.Equal(1, result.Data[result.Labels.IndexOf(expectedLabel1)]);
        Assert.Equal(1, result.Data[result.Labels.IndexOf(expectedLabel2)]);
    }

    [Fact]
    public async Task GetChartData_ShouldIgnoreInvalidDataTypes()
    {
        var dataTypes = new List<string> { "InvalidDataType" };
        var employees = new List<EmployeeDto>
    {
        new EmployeeDto { DateOfBirth = new DateTime(1994, 1, 1) },
    };

        _employeeService.Setup(es => es.GetAll("")).ReturnsAsync(employees);

        var result = await _chartService.GetChartData(dataTypes);

        Assert.NotNull(result);
        Assert.Empty(result.Labels);
        Assert.Empty(result.Data);
    }
}