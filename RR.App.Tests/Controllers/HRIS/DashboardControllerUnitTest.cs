using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.UnitOfWork;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class DashboardControllerUnitTest
{
    private readonly DashboardController _dashboardController;
    private readonly Mock<IDashboardService> _dashboardMockService;

    public DashboardControllerUnitTest()
    {
        _dashboardMockService = new Mock<IDashboardService>();
        _dashboardController = new DashboardController(_dashboardMockService.Object);
    }

    [Fact]
    public async Task GetEmployeeCountSuccessTest()
    {
        var expectedCount = new EmployeeCountDataCard { EmployeeTotalDifference = 42 };
        _dashboardMockService.Setup(service => service.GenerateDataCardInformation())
                            .ReturnsAsync(expectedCount);

        var result = await _dashboardController.GetEmployeesCount();

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(expectedCount.EmployeeTotalDifference, ((EmployeeCountDataCard)okObjectResult.Value!).EmployeeTotalDifference);
    }

    [Fact]
    public async Task CalculateEmployeeGrowthRate_ReturnsOkResult()
    {
        var expectedGrowthRate = 5.5;
        _dashboardMockService.Setup(service => service.CalculateEmployeeGrowthRate())
                             .ReturnsAsync(expectedGrowthRate);

        var result = await _dashboardController.CalculateEmployeeGrowthRate();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedGrowthRate, okResult.Value);
    }

    [Fact]
    public async Task CalculateEmployeeGrowthRate_Returns500_WhenExceptionIsThrown()
    {
        _dashboardMockService.Setup(service => service.CalculateEmployeeGrowthRate())
                             .ThrowsAsync(new CustomException("Some error occurred"));

        var exception = await Assert.ThrowsAsync<CustomException>(() =>
             _dashboardController.CalculateEmployeeGrowthRate());

        Assert.Equal("Some error occurred", exception.Message);
    }

    [Fact]
    public async Task GetEmployeesCountFailTest()
    {
        _dashboardMockService.Setup(service => service.GenerateDataCardInformation())
                            .ThrowsAsync(new CustomException("Failed to generate data card"));

        var exception = await Assert.ThrowsAsync<CustomException>( () =>
             _dashboardController.GetEmployeesCount());

        Assert.Equal("Failed to generate data card", exception.Message);
    }

    [Fact]
    public async Task GetChurnRateSuccessTest()
    {
        var expectedChurnRate = new ChurnRateDataCardDto { ChurnRate = 0.15 };
        _dashboardMockService.Setup(service => service.CalculateEmployeeChurnRate())
                            .ReturnsAsync(expectedChurnRate);

        var result = await _dashboardController.GetChurnRate();

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(expectedChurnRate.ChurnRate, ((ChurnRateDataCardDto)okObjectResult.Value!).ChurnRate);
    }

    [Fact]
    public async Task GetChurnRateFailTest()
    {
        _dashboardMockService.Setup(service => service.CalculateEmployeeChurnRate())
                            .ThrowsAsync(new CustomException("Failed to calculate churn rate"));

        var exception = await Assert.ThrowsAsync<CustomException>(async () =>
            await _dashboardController.GetChurnRate());

        Assert.Equal("Failed to calculate churn rate", exception.Message);
    }
}


