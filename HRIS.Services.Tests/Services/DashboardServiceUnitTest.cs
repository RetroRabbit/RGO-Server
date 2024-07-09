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
}

