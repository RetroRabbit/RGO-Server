using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    public async Task SaveEmployeeQualification_ReturnsOkObjectResult_WithQualification()
    {
        var dto = new EmployeeQualificationDto { EmployeeId = 1 };
        _mockEmployeeQualificationService.Setup(x => x.SaveEmployeeQualification(dto, dto.EmployeeId))
            .ReturnsAsync(dto);

        var result = await _employeeQualificationController.SaveEmployeeQualification(dto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<EmployeeQualificationDto>(okResult.Value);
        Assert.Equal(dto, returnValue);
    }

}
