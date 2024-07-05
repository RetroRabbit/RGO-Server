using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data;
using RR.UnitOfWork;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class DashboardControllerUnitTest
{
    private readonly DasboardController _dashboardcontroller;
    private readonly Mock<IUnitOfWork> _dbMock;
    private readonly Mock<IDashboardService> _dashboardMockService;

    public DashboardControllerUnitTest()
    {
        _dbMock = new Mock<IUnitOfWork>();
        _dashboardMockService = new Mock<IDashboardService>();
        _dashboardcontroller = new DasboardController(new AuthorizeIdentityMock(), _dashboardMockService.Object);
    }

    [Fact]
    public async Task GetEmployeeCountSuccessTest()
    {
        var expectedCount = new EmployeeCountDataCard { EmployeeTotalDifference = 42 };
        _dashboardMockService.Setup(service => service.GenerateDataCardInformation())
                            .ReturnsAsync(expectedCount);

        var result = await _dashboardcontroller.GetEmployeesCount();

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(expectedCount.EmployeeTotalDifference, ((EmployeeCountDataCard)okObjectResult.Value!).EmployeeTotalDifference);
    }

    [Fact]
    public async Task GetEmployeesCountFailTest()
    {
        _dashboardMockService.Setup(service => service.GenerateDataCardInformation())
                            .ThrowsAsync(new Exception("Failed to generate data card"));

        var result = await _dashboardcontroller.GetEmployeesCount();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.Equal("Failed to generate data card", notFoundResult.Value);
    }

    [Fact]
    public async Task GetChurnRateSuccessTest()
    {
        var expectedChurnRate = new ChurnRateDataCardDto { ChurnRate = 0.15 };
        _dashboardMockService.Setup(service => service.CalculateEmployeeChurnRate())
                            .ReturnsAsync(expectedChurnRate);

        var result = await _dashboardcontroller.GetChurnRate();

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(expectedChurnRate.ChurnRate, ((ChurnRateDataCardDto)okObjectResult.Value!).ChurnRate);
    }

    [Fact]
    public async Task GetChurnRateFailTest()
    {
        _dashboardMockService.Setup(service => service.CalculateEmployeeChurnRate())
                            .ThrowsAsync(new Exception("Failed to calculate churn rate"));

        var result = await _dashboardcontroller.GetChurnRate();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.Equal("Failed to calculate churn rate", notFoundResult.Value);
    }
}


