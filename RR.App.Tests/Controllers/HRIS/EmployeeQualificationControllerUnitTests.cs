using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeQualificationControllerUnitTests
{
    private readonly Mock<IEmployeeQualificationService> _mockEmployeeQualificationService;
    private readonly Mock<IEmployeeService> _mockEmployeeService;
    private readonly Mock<IErrorLoggingService> _mockErrorLoggingService;
    private readonly EmployeeQualificationController _employeeQualificationController;

    public EmployeeQualificationControllerUnitTests()
    {
        _mockEmployeeQualificationService = new Mock<IEmployeeQualificationService>();
        _mockEmployeeService = new Mock<IEmployeeService>();
        _mockErrorLoggingService = new Mock<IErrorLoggingService>();
        _employeeQualificationController = new EmployeeQualificationController(
            _mockEmployeeQualificationService.Object,
            _mockEmployeeService.Object,
            _mockErrorLoggingService.Object);
    }

    [Fact]
    public async Task SaveEmployeeQualificationReturnsOkObjectResultWithQualification()
    {
        var dto = new EmployeeQualificationDto { EmployeeId = 1 };
        _mockEmployeeQualificationService.Setup(x => x.SaveEmployeeQualification(dto, dto.EmployeeId))
            .ReturnsAsync(dto);

        var result = await _employeeQualificationController.SaveEmployeeQualification(dto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<EmployeeQualificationDto>(okResult.Value);
        Assert.Equal(dto, returnValue);
    }

    [Fact]
    public async Task SaveEmployeeQualificationReturnsInternalServerErrorOnException()
    {
        var dto = new EmployeeQualificationDto { EmployeeId = 1 };
        var exceptionMessage = "Test Exception";

        _mockEmployeeQualificationService.Setup(x => x.SaveEmployeeQualification(dto, dto.EmployeeId))
            .ThrowsAsync(new Exception(exceptionMessage));

        var result = await _employeeQualificationController.SaveEmployeeQualification(dto);

        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal(exceptionMessage, statusCodeResult.Value);
    }

    [Fact]
    public async Task UpdateEmployeeQualificationReturnsOkObjectResultWithUpdatedQualification()
    {
        var dto = new EmployeeQualificationDto { Id = 1, EmployeeId = 1 };
        _mockEmployeeQualificationService.Setup(x => x.UpdateEmployeeQualification(dto))
            .ReturnsAsync(dto);

        var result = await _employeeQualificationController.UpdateEmployeeQualification(1, dto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<EmployeeQualificationDto>(okResult.Value);
        Assert.Equal(dto, returnValue);
    }

    [Fact]
    public async Task UpdateEmployeeQualificationReturnsNotFoundOnKeyNotFoundException()
    {
        var dto = new EmployeeQualificationDto { Id = 1, EmployeeId = 1 };
        var exceptionMessage = "Qualification not found";

        _mockEmployeeQualificationService.Setup(x => x.UpdateEmployeeQualification(dto))
            .ThrowsAsync(new KeyNotFoundException(exceptionMessage));

        var result = await _employeeQualificationController.UpdateEmployeeQualification(1, dto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task UpdateEmployeeQualificationReturnsInternalServerErrorOnException()
    {
        var dto = new EmployeeQualificationDto { Id = 1, EmployeeId = 1 };
        var exceptionMessage = "Test Exception";

        _mockEmployeeQualificationService.Setup(x => x.UpdateEmployeeQualification(dto))
            .ThrowsAsync(new Exception(exceptionMessage));

        var result = await _employeeQualificationController.UpdateEmployeeQualification(1, dto);

        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal(exceptionMessage, statusCodeResult.Value);
    }
}
