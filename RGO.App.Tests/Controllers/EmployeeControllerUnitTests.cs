using Microsoft.AspNetCore.Mvc;
using Moq;
using RGO.App.Controllers;
using RGO.Models;
using RGO.Services.Interfaces;
using Xunit;

namespace RGO.App.Tests.Controllers;

public class EmployeeControllerUnitTests
{
    private readonly Mock<IEmployeeService> _employeeMockService;
    private readonly EmployeeController _controller;
    private readonly EmployeeDto _employee;

    public EmployeeControllerUnitTests()
    {
        _employeeMockService = new Mock<IEmployeeService>();
        _controller = new EmployeeController(_employeeMockService.Object);

        EmployeeTypeDto employeeTypeDto = new(1, "Developer");
        EmployeeAddressDto employeeAddressDto = new(1, "2", "Complex", "2", "Suburb/District", "City", "Country", "Province", "1620");
        _employee = new EmployeeDto(1, "001", "34434434", new DateOnly(), new DateOnly(),
                null, false, "None", 4, employeeTypeDto, "Notes", 1, 28, 128, 100000, "Ms", "Kamo", "K.G.",
                "Smith", new DateOnly(), "South Africa", "South African", "1234457899", " ",
                new DateOnly(), null, Models.Enums.Race.Black, Models.Enums.Gender.Female, null!,
                "ksmith@retrorabbit.co.za", "kmaosmith@gmail.com", "0123456789", null, null, employeeAddressDto, employeeAddressDto, null, null, null);
    }

    [Fact]
    public async Task AddEmployeeSuccessTest()
    {
        _employeeMockService.Setup(service => service.SaveEmployee(_employee))
            .ReturnsAsync(_employee);

        var result = await _controller.AddEmployee(_employee);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("AddEmployee", createdAtActionResult.ActionName);
        Assert.Equal(201, createdAtActionResult.StatusCode);
    }

    [Fact]
    public async Task AddEmployeeEmployeeExistsFailTest()
    {
        _employeeMockService.Setup(service => service.SaveEmployee(_employee))
            .ThrowsAsync(new Exception("Employee exists"));

        var result = await _controller.AddEmployee(_employee);

        var problemDetails = Assert.IsType<ObjectResult>(result);
        Assert.Equal(406, problemDetails.StatusCode);
        Assert.Equal("User Exists", ((ProblemDetails)problemDetails.Value!).Title);
    }

    [Fact]
    public async Task AddEmployeeNotFoundFailTest()
    {
        _employeeMockService.Setup(service => service.SaveEmployee(_employee))
            .ThrowsAsync(new Exception("Not found"));

        var result = await _controller.AddEmployee(_employee);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetEmployeeSuccessTest()
    {
        _employeeMockService.Setup(service => service.GetEmployee(It.IsAny<string>()))
            .ReturnsAsync(_employee);

        var result = await _controller.GetEmployee(_employee.Email);

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(_employee, (EmployeeDto)okObjectResult.Value!);
    }

    [Fact]
    public async Task GetEmployeeFailTest()
    {
        _employeeMockService.Setup(service => service.GetEmployee(It.IsAny<string>()))
            .ThrowsAsync(new Exception("Not found"));

        var result = await _controller.GetEmployee(_employee.Email);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task UpdateEmployeeSuccessTest()
    {
        _employeeMockService.Setup(service => service.UpdateEmployee(_employee, _employee.Email))
            .ReturnsAsync(_employee);

        var result = await _controller.UpdateEmployee(_employee);

        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("UpdateEmployee", createdAtActionResult.ActionName);
        Assert.Equal(201, createdAtActionResult.StatusCode);
    }

    [Fact]
    public async Task UpdateEmployeeFailTest()
    {
        _employeeMockService.Setup(service => service.UpdateEmployee(_employee, _employee.Email))
            .ThrowsAsync(new Exception("Not found"));

        var result = await _controller.UpdateEmployee(_employee);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetAllEmployeesSuccessTest()
    {
        _employeeMockService.Setup(service => service.GetAll())
            .ReturnsAsync(new List<EmployeeDto> { _employee });

        var result = await _controller.GetAllEmployees();

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(new List<EmployeeDto> { _employee }, (List<EmployeeDto>)okObjectResult.Value!);
    }

    [Fact]
    public async Task GetAllEmployeesFailTest()
    {
        _employeeMockService.Setup(service => service.GetAll())
            .ThrowsAsync(new Exception("Not found"));

        var result = await _controller.GetAllEmployees();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task CountAllEmployeesSuccessTest()
    {
        _employeeMockService.Setup(service => service.GetAll())
            .ReturnsAsync(new List<EmployeeDto> { _employee });

        var result = await _controller.CountAllEmployees();

        var okObjectResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okObjectResult.StatusCode);
        Assert.Equal(1, (int)okObjectResult.Value!);
    }

    [Fact]
    public async Task CountAllEmployeesFailTest()
    {
        _employeeMockService.Setup(service => service.GetAll())
            .ThrowsAsync(new Exception("Not found"));

        var result = await _controller.CountAllEmployees();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }
}
