using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeTypeControllerUnitTests
{
    [Fact]
    public async Task GetAllEmployeeTypes_ReturnsOkResult()
    {
        var employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        var controller = new EmployeeTypeController(employeeTypeServiceMock.Object);

        var fakeEmployeeTypes = new List<EmployeeTypeDto>
        {
            new(1, "Type1"),
            new(2, "Type2")
        };

        employeeTypeServiceMock.Setup(service => service.GetAllEmployeeType())
                               .ReturnsAsync(fakeEmployeeTypes);

        var result = await controller.GetAllEmployeeTypes();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);

        var returnedEmployeeTypes = Assert.IsType<List<EmployeeTypeDto>>(okResult.Value);
        Assert.Equal(fakeEmployeeTypes, returnedEmployeeTypes);
    }

    [Fact]
    public async Task GetAllEmployeeTypes_ReturnsNotFoundResult()
    {
        var employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        var controller = new EmployeeTypeController(employeeTypeServiceMock.Object);

        employeeTypeServiceMock.Setup(service => service.GetAllEmployeeType())
                               .ThrowsAsync(new Exception("Not found"));

        var result = await controller.GetAllEmployeeTypes();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);

        var errorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Not found", errorMessage);
    }
}