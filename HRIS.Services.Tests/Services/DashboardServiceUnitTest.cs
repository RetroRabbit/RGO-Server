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


    public DashboardServiceUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _dashboardMockService = new Mock<IDashboardService>();
        _dashboardService = new DashboardService(_dbMock.Object);
    }

    [Fact]
    public async Task CalculateChurnRateTest()
    {
        var today = DateTime.Today;
        var twelveMonthsAgo = today.AddMonths(-12);

        var employeeList = new List<Employee>
        {
            EmployeeTestData.EmployeeOne,
            EmployeeTestData.EmployeeTwo,
            EmployeeTestData.EmployeeThree,
            EmployeeTestData.EmployeeFour,
        };

        _dbMock.Setup(u => u.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(employeeList);

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
    public async Task CalculateEmployeeGrowthRate_ReturnsExpectedRate()
    {
        var currentMonthTotalDto = new MonthlyEmployeeTotalDto
        {
            Id = 1,
            EmployeeTotal = 120,
            DeveloperTotal = 20,
            DesignerTotal = 30,
            ScrumMasterTotal = 15,
            BusinessSupportTotal = 10
        };

        var previousMonthTotalDto = new MonthlyEmployeeTotalDto
        {
            Id = 2,
            EmployeeTotal = 100,
            DeveloperTotal = 15,
            DesignerTotal = 25,
            ScrumMasterTotal = 10,
            BusinessSupportTotal = 5
        };

        var employeeList = new List<Employee>
        {
            EmployeeTestData.EmployeeOne,
            EmployeeTestData.EmployeeTwo,
            EmployeeTestData.EmployeeThree,
            EmployeeTestData.EmployeeFour,
        };

        _dbMock.Setup(u => u.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()));
        _dbMock.Setup(u => u.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(employeeList);

        var newMonthlyEmployeeTotal = new MonthlyEmployeeTotal(currentMonthTotalDto);

        _dbMock.Setup(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()))
               .ReturnsAsync(newMonthlyEmployeeTotal);

        _dashboardMockService.Setup(x => x.GetEmployeeCurrentMonthTotal())
            .ReturnsAsync(currentMonthTotalDto);

        _dashboardMockService.Setup(x => x.GetEmployeePreviousMonthTotal())
            .ReturnsAsync(currentMonthTotalDto);

        var result = await _dashboardService.CalculateEmployeeGrowthRate();

        Assert.NotNull(result);
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task CalculateEmployeeGrowthRate_ThrowsCustomException_WhenTotalsAreNotAvailable()
    {
        _dashboardMockService.Setup(x => x.GetEmployeeCurrentMonthTotal())
                             .ReturnsAsync((MonthlyEmployeeTotalDto)null);

        _dashboardMockService.Setup(x => x.GetEmployeePreviousMonthTotal())
                             .ReturnsAsync((MonthlyEmployeeTotalDto)null);


        _dbMock.Setup(u => u.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()));
        var exception = await Assert.ThrowsAsync<Exception>(() => _dashboardService.CalculateEmployeeGrowthRate());

        Assert.Equal("Employee totals for current or previous month are not available", exception.Message);
    }


    [Fact]
    public async Task GenerateDataCardInformation_ReturnsExpectedDataCard()
    {
        var employeeCountTotalsByRole = new EmployeeCountByRoleDataCard
        {
            DevsCount = 20,
            DesignersCount = 10,
            ScrumMastersCount = 5,
            BusinessSupportCount = 15
        };

        var totalNumberOfEmployeesOnBench = new EmployeeOnBenchDataCard
        {
            DevsOnBenchCount = 2,
            DesignersOnBenchCount = 1,
            ScrumMastersOnBenchCount = 1,
            TotalNumberOfEmployeesOnBench = 4
        };

        int totalNumberOfEmployeesOnClients = 36;

        var currentMonthTotalDto = new MonthlyEmployeeTotalDto
        {
            Id = 1,
            EmployeeTotal = 40,
            DeveloperTotal = 20,
            DesignerTotal = 10,
            ScrumMasterTotal = 5,
            BusinessSupportTotal = 5
        };

        var previousMonthTotalDto = new MonthlyEmployeeTotalDto
        {
            Id = 2,
            EmployeeTotal = 35,
            DeveloperTotal = 18,
            DesignerTotal = 9,
            ScrumMasterTotal = 4,
            BusinessSupportTotal = 4
        };

        var employeeList = new List<Employee>
        {
            new Employee(EmployeeTestData.EmployeeOne.ToDto(), EmployeeTypeTestData.DeveloperType.ToDto())
        };

        _dashboardMockService.Setup(x => x.GetEmployeeCountTotalByRole()).Returns(new EmployeeCountByRoleDataCard());
        _dashboardMockService.Setup(x => x.GetTotalNumberOfEmployeesOnBench()).Returns(new EmployeeOnBenchDataCard());
        _dashboardMockService.Setup(x => x.GetTotalNumberOfEmployeesOnClients()).Returns(0);

        _dashboardMockService.Setup(x => x.GetEmployeeCurrentMonthTotal()).ReturnsAsync(currentMonthTotalDto);
        _dashboardMockService.Setup(x => x.GetEmployeePreviousMonthTotal()).ReturnsAsync(previousMonthTotalDto);

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>())).Returns(employeeList.ToMockIQueryable());
        _dbMock.Setup(mt => mt.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>())).Returns(new MonthlyEmployeeTotal(previousMonthTotalDto).ToMockIQueryable());
        _dbMock.Setup(e => e.Employee.GetAll(It.IsAny<Expression<Func<Employee, bool>>>())).ReturnsAsync(employeeList);
        
         var newMonthlyEmployeeTotal = new MonthlyEmployeeTotal(currentMonthTotalDto);

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

        var employeeList = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        var employee = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(employeeList);

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employee.ToMockIQueryable());

        var monthlyEmployeeTotalDto = MonthlyEmployeeTotalTestData.MonthlyEmployeeTotal_CurrentYear_CurrentMonth;

        var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
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

        var employeeList = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        var employee = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(employeeList);

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employee.ToMockIQueryable());

        var monthlyEmployeeTotalDto = MonthlyEmployeeTotalTestData.MonthlyEmployeeTotal_PreviuosMonth_CurrentYear;

        var montlhyEmployeeTotalList = new List<MonthlyEmployeeTotal>
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
        var employeeList = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        var employee = new List<Employee>
        {
            EmployeeTestData.EmployeeOne
        };

        _dbMock.Setup(u => u.Employee.GetAll(null)).ReturnsAsync(employeeList);

        _dbMock.Setup(e => e.Employee.Get(It.IsAny<Expression<Func<Employee, bool>>>()))
               .Returns(employee.ToMockIQueryable());

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
    public void GetTotalNumberOfEmployeesOnClients_ReturnsExpectedCount()
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

        var result = _dashboardService.GetTotalNumberOfEmployeesOnClients();

        Assert.Equal(3, result);
    }

    [Fact]
    public void GetTotalNumberOfEmployeesOnBench_ReturnsExpectedCounts()
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

        var result = _dashboardService.GetTotalNumberOfEmployeesOnBench();

        Assert.NotNull(result);
        Assert.Equal(1, result.DevsOnBenchCount);
        Assert.Equal(1, result.DesignersOnBenchCount);
        Assert.Equal(1, result.ScrumMastersOnBenchCount);
        Assert.Equal(3, result.TotalNumberOfEmployeesOnBench);
    }
}

