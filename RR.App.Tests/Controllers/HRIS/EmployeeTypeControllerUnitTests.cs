using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeTypeControllerUnitTests
{
    private readonly Mock<IEmployeeTypeService> _employeeTypeServiceMock;
    private readonly EmployeeTypeController _controller;
    public EmployeeTypeControllerUnitTests() 
    { 
        _employeeTypeServiceMock = new Mock<IEmployeeTypeService>();
        _controller = new EmployeeTypeController(_employeeTypeServiceMock.Object);
    }

    [Fact]
    public async Task GetAllEmployeeTypes_ReturnsOkResult()
    {
        var fakeEmployeeTypes = new List<EmployeeTypeDto>
        {
            new EmployeeTypeDto { Id = 1, Name = "Type1" },
            new EmployeeTypeDto { Id = 2, Name = "Type2" }
        };

        _employeeTypeServiceMock.Setup(service => service.GetAllEmployeeType())
                               .ReturnsAsync(fakeEmployeeTypes);

        var result = await _controller.GetAllEmployeeTypes();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedEmployeeTypes = Assert.IsType<List<EmployeeTypeDto>>(okResult.Value);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(fakeEmployeeTypes, returnedEmployeeTypes);
    }

    [Fact]
    public async Task GetAllEmployeeTypes_ReturnsNotFoundResult()
    {
        _employeeTypeServiceMock.Setup(service => service.GetAllEmployeeType())
                               .ThrowsAsync(new Exception("Not found"));

        var result = await _controller.GetAllEmployeeTypes();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal(404, notFoundResult.StatusCode);
        Assert.Equal("Not found", errorMessage);
    }
}