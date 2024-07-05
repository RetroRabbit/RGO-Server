using HRIS.Models;
using RR.Tests.Data.Models.HRIS;
using RR.Tests.Data;
using RR.UnitOfWork.Entities.HRIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }

    [Fact]
    public async Task CalculateChurnRateTest()
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

        var churnRateDto = new ChurnRateDataCardDto
        {
            ChurnRate = Math.Round(0.0, 0),
            DeveloperChurnRate = Math.Round(0.0, 0),
            DesignerChurnRate = Math.Round(0.0, 0),
            ScrumMasterChurnRate = Math.Round(0.0, 0),
            BusinessSupportChurnRate = Math.Round(0.0, 0),
            Month = "January",
            Year = 2024
        };

        _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
               .Returns(montlhyEmployeeTotalList.ToMockIQueryable());

        _dbMock.Setup(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()))
               .ReturnsAsync(monthlyEmployeeTotalDto);

        var result = await _dashboardService.CalculateEmployeeChurnRate();

        Assert.NotNull(result);
        Assert.Equal(churnRateDto.ChurnRate, result.ChurnRate);
    }

    [Fact]
    public async Task CalculateChurnRateIfStatementTest()
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

        var churnRateDto = new ChurnRateDataCardDto
        {
            ChurnRate = Math.Round(0.0, 0),
            DeveloperChurnRate = Math.Round(0.0, 0),
            DesignerChurnRate = Math.Round(0.0, 0),
            ScrumMasterChurnRate = Math.Round(0.0, 0),
            BusinessSupportChurnRate = Math.Round(0.0, 0),
            Month = "January",
            Year = 2024
        };

        _dbMock.Setup(e => e.MonthlyEmployeeTotal.Get(It.IsAny<Expression<Func<MonthlyEmployeeTotal, bool>>>()))
               .Returns(montlhyEmployeeTotalList.ToMockIQueryable());

        _dbMock.Setup(u => u.MonthlyEmployeeTotal.Add(It.IsAny<MonthlyEmployeeTotal>()))
               .ReturnsAsync(monthlyEmployeeTotalDto);

        var result = await _dashboardService.CalculateEmployeeChurnRate();

        Assert.NotNull(result);
        Assert.Equal(churnRateDto.ChurnRate, result.ChurnRate);
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

