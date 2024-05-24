using HRIS.Models;
using HRIS.Models.Enums.QualificationEnums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RGO.Tests.Data.Models;
using RR.App.Controllers.HRIS;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork.Entities.HRIS;
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

    [Fact]
    public async Task GetEmployeeQualificationByEmployeeIdReturnsOkObjectResultWithQualifications()
    {
        var expectedQualificationDto = EmployeeQualificationTestData.EmployeeQualification;

        _mockEmployeeQualificationService.Setup(x => x.GetAllEmployeeQualificationsByEmployeeId(EmployeeQualificationTestData.EmployeeQualification.Id))
            .ReturnsAsync(EmployeeQualificationTestData.EmployeeQualification);

        var result = await _employeeQualificationController.GetEmployeeQualificationByEmployeeId(EmployeeQualificationTestData.EmployeeQualification.Id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<EmployeeQualificationDto>(okResult.Value);
        Assert.Equal(expectedQualificationDto, returnValue);
    }

    [Fact]
    public async Task GetEmployeeQualificationByEmployeeIdReturnsNotFoundWhenQualificationsNotFound()
    {
        var employeeId = 1;
        var exceptionMessage = "Qualifications not found";

        _mockEmployeeQualificationService.Setup(x => x.GetAllEmployeeQualificationsByEmployeeId(employeeId))
            .ThrowsAsync(new KeyNotFoundException(exceptionMessage));

        var result = await _employeeQualificationController.GetEmployeeQualificationByEmployeeId(employeeId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task GetEmployeeQualificationByEmployeeIdReturnsBadRequestOnException()
    {
        var employeeId = 1;
        var exceptionMessage = "An error occurred while retrieving qualifications";

        _mockEmployeeQualificationService.Setup(x => x.GetAllEmployeeQualificationsByEmployeeId(employeeId))
            .ThrowsAsync(new Exception(exceptionMessage));

        var result = await _employeeQualificationController.GetEmployeeQualificationByEmployeeId(employeeId);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.StartsWith("An error occurred while retrieving qualifications", (string)badRequestResult.Value);
    }

    [Fact]
    public async Task DeleteEmployeeQualificationReturnsOkObjectResultWithDeletedQualification()
    {
        var EmployeeQualificationDto = EmployeeQualificationTestData.EmployeeQualification; 

        _mockEmployeeQualificationService.Setup(x => x.DeleteEmployeeQualification(EmployeeQualificationDto.Id))
            .ReturnsAsync(EmployeeQualificationDto);

        var result = await _employeeQualificationController.DeleteEmployeeQualification(EmployeeQualificationDto.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<EmployeeQualificationDto>(okResult.Value);
        Assert.Equal(EmployeeQualificationDto, returnValue);
    }

    [Fact]
    public async Task DeleteEmployeeQualificationReturnsNotFoundWhenKeyNotFoundExceptionThrown()
    {
        var id = 1;
        var exceptionMessage = "Qualification not found";

        _mockEmployeeQualificationService.Setup(x => x.DeleteEmployeeQualification(id))
            .ThrowsAsync(new KeyNotFoundException(exceptionMessage));

        var result = await _employeeQualificationController.DeleteEmployeeQualification(id);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(exceptionMessage, notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteEmployeeQualificationReturnsStatusCode500OnException()
    {
        var id = 1;
        var expectedExceptionMessage = "An unexpected error occurred";

        _mockEmployeeQualificationService.Setup(x => x.DeleteEmployeeQualification(id))
            .ThrowsAsync(new Exception(expectedExceptionMessage));

        var result = await _employeeQualificationController.DeleteEmployeeQualification(id);

        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal($"An error occurred while deleting the qualification: {expectedExceptionMessage}", statusCodeResult.Value);
    }

    [Fact]
    public async Task GetAllEmployeeQualifications()
    {
        var listOfEmployeeQualificationDto = new List<EmployeeQualificationDto>()
            {
                EmployeeQualificationTestData.EmployeeQualification
            };

        _mockEmployeeQualificationService.Setup(x => x.GetAllEmployeeQualifications())
            .ReturnsAsync(listOfEmployeeQualificationDto);

        var result = await _employeeQualificationController.GetAllEmployeeQualifications();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<List<EmployeeQualificationDto>>(okResult.Value);
        Assert.Equal(listOfEmployeeQualificationDto, returnValue);
    }

    [Fact]
    public async Task GetAllEmployeeQualificationsReturnsBadRequestOnException()
    {
        var exceptionMessage = "An error occurred while retrieving qualifications";

        _mockEmployeeQualificationService.Setup(x => x.GetAllEmployeeQualifications())
            .ThrowsAsync(new Exception(exceptionMessage));

        var result = await _employeeQualificationController.GetAllEmployeeQualifications();

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.StartsWith("An error occurred while retrieving qualifications", (string)badRequestResult.Value);
    }
}