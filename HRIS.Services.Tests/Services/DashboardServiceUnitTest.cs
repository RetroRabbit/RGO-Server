using HRIS.Models;
using RR.Tests.Data.Models.HRIS;
using RR.Tests.Data;
using RR.UnitOfWork.Entities.HRIS;
using Xunit;
using HRIS.Services.Interfaces;
using Moq;
using RR.UnitOfWork;
using HRIS.Services.Services;
using System.Linq.Expressions;

namespace HRIS.Services.Tests.Services;

public class DashboardServiceUnitTest
{
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IDashboardService> _dashboardMockService;
    private readonly DashboardService _dashboardService;
    private readonly List<Employee> _employees;
    private readonly MonthlyEmployeeTotalDto _monthTotalDto;

    public DashboardServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _dashboardMockService = new Mock<IDashboardService>();
        _dashboardService = new DashboardService(_dbMock.Object);

        _employees = new List<Employee>
        {
            EmployeeTestData.EmployeeOne,
            EmployeeTestData.EmployeeTwo,
            EmployeeTestData.EmployeeThree,
            EmployeeTestData.EmployeeFour,
        };

        _monthTotalDto = new MonthlyEmployeeTotalDto
        {
            Id = 1,
            EmployeeTotal = 40,
            DeveloperTotal = 20,
            DesignerTotal = 10,
            ScrumMasterTotal = 5,
            BusinessSupportTotal = 5
        };
    }

    [Fact]
    public async Task CalculateChurnRateTest()
    {
        _dbMock.Setup(u => u.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(_employees);

        var result = await _dashboardService.CalculateEmployeeChurnRate();
        Assert.NotNull(result);
        Assert.Equal("July", result.Month);
        Assert.Equal(0, result.ChurnRate);
        Assert.Equal(0, result.DeveloperChurnRate);
        Assert.Equal(0, result.DesignerChurnRate);
        Assert.Equal(0, result.ScrumMasterChurnRate);
        Assert.Equal(0, result.BusinessSupportChurnRate);
    }

    [Fact]
    public async Task GenerateDataCardInformation_ReturnsExpectedDataCard()
    {
        var employeeList = new List<Employee>
        {
            new Employee(EmployeeTestData.EmployeeOne.ToDto(), EmployeeTypeTestData.DeveloperType.ToDto())
        };

        _dashboardMockService.Setup(x => x.GetEmployeeCountTotalByRole()).ReturnsAsync(new EmployeeCountByRoleDataCard());
        _dashboardMockService.Setup(x => x.GetTotalNumberOfEmployeesOnBench()).ReturnsAsync(new EmployeeOnBenchDataCard());
        _dashboardMockService.Setup(x => x.GetTotalNumberOfEmployeesOnClients()).ReturnsAsync(0);

        _dashboardMockService.Setup(x => x.GetEmployeeCurrentMonthTotal()).ReturnsAsync(_monthTotalDto);
        _dashboardMockService.Setup(x => x.GetEmployeePreviousMonthTotal()).ReturnsAsync(_monthTotalDto);

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(employeeList.ToMockIQueryable());
        _dbMock.Setup(mt => mt.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>())).Returns(new MonthlyEmployeeTotal(_monthTotalDto).ToMockIQueryable());
        _dbMock.Setup(e => e.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(_employees);
        
         var newMonthlyEmployeeTotal = new MonthlyEmployeeTotal(_monthTotalDto);

        _dbMock.Setup(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()))
               .ReturnsAsync(newMonthlyEmployeeTotal);
        var result = await _dashboardService.GenerateDataCardInformation();

        Assert.NotNull(result);
        Assert.Equal(0, result.DevsCount);
        Assert.Equal(0, result.DesignersCount);
        Assert.Equal(0, result.ScrumMastersCount);
        Assert.Equal(0,result.BusinessSupportCount);
        Assert.Equal(0, result.DevsOnBenchCount);
        Assert.Equal(0, result.DesignersOnBenchCount);
        Assert.Equal(0, result.ScrumMastersOnBenchCount);
        Assert.Equal(0, result.TotalNumberOfEmployeesOnClients);
        Assert.Equal(0, result.TotalNumberOfEmployeesOnBench);
        Assert.Equal(0, result.BillableEmployeesPercentage);
        Assert.Equal(0, result.EmployeeTotalDifference);
        Assert.False(result.isIncrease);
    }

    [Fact]
    public async Task GetCurrentMonthTotalReturnsExistingTotalTest()
    {
        _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(_employees);

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(_employees.ToMockIQueryable());

        var monthlyEmployeeTotalDto = MonthlyEmployeeTotalTestData.MonthlyEmployeeTotal_CurrentYear_CurrentMonth;

        var monthlyEmployeeTotalList = new List<MonthlyEmployeeTotal>
        {
            monthlyEmployeeTotalDto
        };

        _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
               .Returns(montlhyEmployeeTotalList.ToMockIQueryable());

        var result = await _dashboardService.GetEmployeeCurrentMonthTotal();

        Assert.NotNull(result);
        Assert.Equal(monthlyEmployeeTotalDto.Month, result.Month);
    }

    [Fact]
    public async Task GetCurrentMonthTotalCreateNewTotalTest()
    {
        _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(_employees);

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(_employees.ToMockIQueryable());

        var monthlyEmployeeTotalDto = MonthlyEmployeeTotalTestData.MonthlyEmployeeTotal_PreviuosMonth_CurrentYear;

        var monthlyEmployeeTotalList = new List<MonthlyEmployeeTotal>
        {
            monthlyEmployeeTotalDto
        };

        _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
               .Returns(montlhyEmployeeTotalList.ToMockIQueryable());

        _dbMock.Setup(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()))
               .ReturnsAsync(monthlyEmployeeTotalDto);

        var result = await _dashboardService.GetEmployeeCurrentMonthTotal();

        _dbMock.Verify(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(1, result.EmployeeTotal);
        Assert.Equal(1, result.DeveloperTotal);
    }

    [Fact]
    public async Task GetPreviousMonthTotalReturnsExistingTotalTest()
    {
        var monthlyEmployeeTotalDto = MonthlyEmployeeTotalTestData.MonthlyEmployeeTotal_PreviuosMonth_CurrentYear;

        var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
        {
            monthlyEmployeeTotalDto
        };

        _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
               .Returns(montlhyEmployeeTotalList.ToMockIQueryable());

        var result = await _dashboardService.GetEmployeePreviousMonthTotal();

        Assert.NotNull(result);
        Assert.Equal(monthlyEmployeeTotalDto.EmployeeTotal, result.EmployeeTotal);
    }


    [Fact]
    public async Task GetPreviousMonthTotalCreateNewTotalTest()
    {

        _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(_employees);

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(_employees.ToMockIQueryable());

        var monthlyEmployeeTotalDto = MonthlyEmployeeTotalTestData.MonthlyEmployeeTotal_MonthNov_CurrentYear;

        var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
        {
            monthlyEmployeeTotalDto
        };

        _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
               .Returns(montlhyEmployeeTotalList.ToMockIQueryable());

        _dbMock.Setup(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()))
               .ReturnsAsync(monthlyEmployeeTotalDto);

        var result = await _dashboardService.GetEmployeePreviousMonthTotal();

        Assert.NotNull(result);
        Assert.Equal(1, result.EmployeeTotal);
    }

    [Fact]
    public async Task GetTotalNumberOfEmployeesOnClients_ReturnsExpectedCount()
    {
        var employees = new List<Employee>
        {
            new Employee { EmployeeTypeId = 2, ClientAllocated = 2 },
            new Employee { EmployeeTypeId = 3, ClientAllocated = 3 },
            new Employee { EmployeeTypeId = 4, ClientAllocated = 4 },
            new Employee { EmployeeTypeId = 2, ClientAllocated = 1 },
            new Employee { EmployeeTypeId = 1, ClientAllocated = 2 },
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
             .Returns(employees.ToMockIQueryable());

        var result = await _dashboardService.GetTotalNumberOfEmployeesOnClients();

        Assert.Equal(3, result);
    }

    [Fact]
    public async Task GetTotalNumberOfEmployeesOnBench_ReturnsExpectedCountsAsync()
    {
        var employees = new List<Employee>
        {
            new Employee { EmployeeTypeId = 2, ClientAllocated = null },
            new Employee { EmployeeTypeId = 3, ClientAllocated = null },
            new Employee { EmployeeTypeId = 4, ClientAllocated = null },
            new Employee { EmployeeTypeId = 2, ClientAllocated = 1 },
            new Employee { EmployeeTypeId = 1, ClientAllocated = null },
        };

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
              .Returns(employees.ToMockIQueryable());

        var result = await _dashboardService.GetTotalNumberOfEmployeesOnBench();

        Assert.NotNull(result);
        Assert.Equal(1, result.DevsOnBenchCount);
        Assert.Equal(1, result.DesignersOnBenchCount);
        Assert.Equal(1, result.ScrumMastersOnBenchCount);
        Assert.Equal(3, result.TotalNumberOfEmployeesOnBench);
    }
}

