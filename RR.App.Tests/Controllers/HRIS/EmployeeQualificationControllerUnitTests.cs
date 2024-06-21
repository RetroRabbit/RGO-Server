using HRIS.Models;
using HRIS.Models.Enums.QualificationEnums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Transaction;
using Moq;
using RGO.Tests.Data.Models;
using RR.App.Controllers.HRIS;
using RR.Tests.Data.Models.HRIS;
using RR.UnitOfWork.Entities.HRIS;
using System.Security.Claims;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeQualificationControllerUnitTests
{
    private readonly Mock<IEmployeeQualificationService> _mockEmployeeQualificationService;
    private readonly Mock<IEmployeeService> _mockEmployeeService;
    private readonly Mock<IErrorLoggingService> _mockErrorLoggingService;
    private readonly EmployeeQualificationController _employeeQualificationController;
    private readonly EmployeeQualificationDto _employeeQualificationDto;

    public EmployeeQualificationControllerUnitTests()
    {
        _mockEmployeeQualificationService = new Mock<IEmployeeQualificationService>();
        _mockEmployeeService = new Mock<IEmployeeService>();
        _mockErrorLoggingService = new Mock<IErrorLoggingService>();

        _employeeQualificationController = new EmployeeQualificationController(
            _mockEmployeeQualificationService.Object,
            _mockEmployeeService.Object,
            _mockErrorLoggingService.Object);

        _employeeQualificationDto = EmployeeQualificationTestData.EmployeeQualification;
    }

    [Fact]
    public async Task SaveEmployeeQualificationReturnsOkObjectResultWithQualification()
    {
        _mockEmployeeQualificationService.Setup(x => x.SaveEmployeeQualification(_employeeQualificationDto, _employeeQualificationDto.EmployeeId))
            .ReturnsAsync(_employeeQualificationDto);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, "TestUser"),
                                        new Claim(ClaimTypes.Email, "test@example.com"),
                                        new Claim(ClaimTypes.Role, "SuperAdmin")
                                   }, "TestAuthentication"));
        _employeeQualificationController.ControllerContext = new ControllerContext();
        _employeeQualificationController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        var result = await _employeeQualificationController.SaveEmployeeQualification(_employeeQualificationDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<EmployeeQualificationDto>(okResult.Value);
        Assert.Equal(_employeeQualificationDto, returnValue);
    }

    [Fact]
    public async Task SaveEmployeeQualificationReturnsInternalServerErrorOnException()
    {
        _mockEmployeeQualificationService.Setup(x => x.SaveEmployeeQualification(   _employeeQualificationDto, _employeeQualificationDto.EmployeeId))
            .ThrowsAsync(new Exception("Test Exception"));

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, "TestUser"),
                                        new Claim(ClaimTypes.Email, "test@example.com"),
                                        new Claim(ClaimTypes.Role, "SuperAdmin")
                                   }, "TestAuthentication"));
        _employeeQualificationController.ControllerContext = new ControllerContext();
        _employeeQualificationController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        var result = await _employeeQualificationController.SaveEmployeeQualification(_employeeQualificationDto);

        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("Test Exception", statusCodeResult.Value);
    }

    [Fact]
    public async Task UpdateEmployeeQualificationReturnsOkObjectResultWithUpdatedQualification()
    {
        _mockEmployeeQualificationService.Setup(x => x.UpdateEmployeeQualification(_employeeQualificationDto))
            .ReturnsAsync(_employeeQualificationDto);

        var result = await _employeeQualificationController.UpdateEmployeeQualification(1, _employeeQualificationDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<EmployeeQualificationDto>(okResult.Value);
        Assert.Equal(_employeeQualificationDto, returnValue);
    }

    [Fact]
    public async Task UpdateEmployeeQualificationReturnsNotFoundOnKeyNotFoundException()
    {
        _mockEmployeeQualificationService.Setup(x => x.UpdateEmployeeQualification(_employeeQualificationDto))
            .ThrowsAsync(new KeyNotFoundException("Qualification not found"));

        var result = await _employeeQualificationController.UpdateEmployeeQualification(1, _employeeQualificationDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Qualification not found", notFoundResult.Value);
    }

    [Fact]
    public async Task UpdateEmployeeQualificationReturnsInternalServerErrorOnException()
    {
        _mockEmployeeQualificationService.Setup(x => x.UpdateEmployeeQualification(_employeeQualificationDto))
            .ThrowsAsync(new Exception("Test Exception"));

        var result = await _employeeQualificationController.UpdateEmployeeQualification(1, _employeeQualificationDto);

        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal("Test Exception", statusCodeResult.Value);
    }

    [Fact]
    public async Task GetEmployeeQualificationByEmployeeIdReturnsOkObjectResultWithQualifications()
    {
        _mockEmployeeQualificationService.Setup(x => x.GetAllEmployeeQualificationsByEmployeeId(_employeeQualificationDto.Id))
            .ReturnsAsync(EmployeeQualificationTestData.EmployeeQualification);

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, "TestUser"),
                                        new Claim(ClaimTypes.Email, "test@example.com"),
                                        new Claim(ClaimTypes.Role, "SuperAdmin")
                                   }, "TestAuthentication"));
        _employeeQualificationController.ControllerContext = new ControllerContext();
        _employeeQualificationController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        var result = await _employeeQualificationController.GetEmployeeQualificationByEmployeeId(_employeeQualificationDto.Id);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnValue = Assert.IsType<EmployeeQualificationDto>(okResult.Value);
        Assert.Equal(_employeeQualificationDto, returnValue);
    }

    [Fact]
    public async Task GetEmployeeQualificationByEmployeeIdReturnsNotFoundWhenQualificationsNotFound()
    {
        _mockEmployeeQualificationService.Setup(x => x.GetAllEmployeeQualificationsByEmployeeId(1))
            .ThrowsAsync(new KeyNotFoundException("Qualifications not found"));

        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.NameIdentifier, "TestUser"),
                                        new Claim(ClaimTypes.Email, "test@example.com"),
                                        new Claim(ClaimTypes.Role, "SuperAdmin")
                                   }, "TestAuthentication"));
        _employeeQualificationController.ControllerContext = new ControllerContext();
        _employeeQualificationController.ControllerContext.HttpContext = new DefaultHttpContext { User = user };

        var result = await _employeeQualificationController.GetEmployeeQualificationByEmployeeId(1);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("Qualifications not found", notFoundResult.Value);
    }

    [Fact]
    public async Task GetEmployeeQualificationByEmployeeIdReturnsBadRequestOnException()
    {
        _mockEmployeeQualificationService.Setup(x => x.GetAllEmployeeQualificationsByEmployeeId(1))
            .ThrowsAsync(new Exception("An error occurred while retrieving qualifications"));

        var result = await _employeeQualificationController.GetEmployeeQualificationByEmployeeId(1);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.StartsWith("An error occurred while retrieving qualifications", (string)badRequestResult.Value);
    }

    [Fact]
    public async Task DeleteEmployeeQualificationReturnsOkObjectResultWithDeletedQualification()
    {
        _mockEmployeeQualificationService.Setup(x => x.DeleteEmployeeQualification(_employeeQualificationDto.Id))
            .ReturnsAsync(_employeeQualificationDto);

        var result = await _employeeQualificationController.DeleteEmployeeQualification(_employeeQualificationDto.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<EmployeeQualificationDto>(okResult.Value);
        Assert.Equal(_employeeQualificationDto, returnValue);
    }

    [Fact]
    public async Task DeleteEmployeeQualificationReturnsNotFoundWhenKeyNotFoundExceptionThrown()
    {
        _mockEmployeeQualificationService.Setup(x => x.DeleteEmployeeQualification(1))
            .ThrowsAsync(new KeyNotFoundException("Qualification not found"));

        var result = await _employeeQualificationController.DeleteEmployeeQualification(1);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("Qualification not found", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteEmployeeQualificationReturnsStatusCode500OnException()
    {
        _mockEmployeeQualificationService.Setup(x => x.DeleteEmployeeQualification(1))
            .ThrowsAsync(new Exception("An unexpected error occurred"));

        var result = await _employeeQualificationController.DeleteEmployeeQualification(1);

        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Equal($"An error occurred while deleting the qualification: {"An unexpected error occurred"}", statusCodeResult.Value);
    }

    [Fact]
    public async Task GetAllEmployeeQualifications()
    {
        var listOfEmployeeQualificationDto = new List<EmployeeQualificationDto>()
            {
               _employeeQualificationDto,
               _employeeQualificationDto
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
        _mockEmployeeQualificationService.Setup(x => x.GetAllEmployeeQualifications())
            .ThrowsAsync(new Exception("An error occurred while retrieving qualifications"));

        var result = await _employeeQualificationController.GetAllEmployeeQualifications();

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.StartsWith("An error occurred while retrieving qualifications", (string)badRequestResult.Value);
    }
}