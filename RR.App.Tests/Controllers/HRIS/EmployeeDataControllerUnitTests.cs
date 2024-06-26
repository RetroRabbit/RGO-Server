using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeDataControllerUnitTests
{
    private readonly Mock<IEmployeeDataService> _employeeDataServiceMock;
    private readonly EmployeeDataController _controller;
    private readonly List<EmployeeDataDto> _employeeDataDtoList;
    private readonly EmployeeDataDto _employeeDataDto;
    public EmployeeDataControllerUnitTests()
    {
        _employeeDataServiceMock = new Mock<IEmployeeDataService>();
        _controller = new EmployeeDataController(new AuthorizeIdentityMock(), _employeeDataServiceMock.Object);

        _employeeDataDtoList = new List<EmployeeDataDto>
        {
            new EmployeeDataDto { Id = 1, EmployeeId = 1, FieldCodeId = 1, Value = "example 1" },
            new EmployeeDataDto { Id = 2, EmployeeId = 2, FieldCodeId = 1, Value = "example 1" }
        };

        _employeeDataDto = new EmployeeDataDto { Id = 1, EmployeeId = 1, FieldCodeId = 1, Value = "example 1" };
    }

    [Fact(Skip = "Current user needs to be set for validations on endpoint")]
    public async Task GetEmployeeDataReturnsOkResult()
    {
        _employeeDataServiceMock.Setup(service => service.GetAllEmployeeData(1))
                               .ReturnsAsync(_employeeDataDtoList);

        var result = await _controller.GetEmployeeData(1);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsType<List<EmployeeDataDto>>(okResult.Value);
        _employeeDataServiceMock.Verify(service => service.GetAllEmployeeData(1), Times.Once);
    }

    [Fact(Skip = "Current user needs to be set for validations on endpoint")]
    public async Task GetEmployeeDataReturnsNotFoundResult()
    {
        _employeeDataServiceMock.Setup(service => service.GetAllEmployeeData(1))
                               .ReturnsAsync((List<EmployeeDataDto>?)null);

        var result = await _controller.GetEmployeeData(1);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Employee data not found", errorMessage);
        _employeeDataServiceMock.Verify(service => service.GetAllEmployeeData(1), Times.Once);
    }

    [Fact(Skip = "Current user needs to be set for validations on endpoint")]
    public async Task SaveEmployeeDataReturnsOkResult()
    {
        _employeeDataServiceMock.Setup(service => service.SaveEmployeeData(_employeeDataDto))
                               .ReturnsAsync(_employeeDataDto);

        var result = await _controller.SaveEmployeeData(_employeeDataDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsType<EmployeeDataDto>(okResult.Value);
        _employeeDataServiceMock.Verify(service => service.SaveEmployeeData(_employeeDataDto), Times.Once);
    }

    [Fact(Skip = "Current user needs to be set for validations on endpoint")]
    public async Task SaveEmployeeDataReturnsNotFoundResultOnException()
    {
        _employeeDataServiceMock.Setup(service => service.SaveEmployeeData(_employeeDataDto))
                               .ThrowsAsync(new Exception("Error saving employee data."));

        var result = await _controller.SaveEmployeeData(_employeeDataDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Error saving employee data.", errorMessage);
        _employeeDataServiceMock.Verify(service => service.SaveEmployeeData(_employeeDataDto), Times.Once);
    }

    [Fact(Skip = "Current user needs to be set for validations on endpoint")]
    public async Task UpdateEmployeeDataReturnsOkResult()
    {
        _employeeDataServiceMock.Setup(service => service.UpdateEmployeeData(_employeeDataDto))
                               .ReturnsAsync(_employeeDataDto);

        var result = await _controller.UpdateEmployeeData(_employeeDataDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsType<EmployeeDataDto>(okResult.Value);
        _employeeDataServiceMock.Verify(service => service.UpdateEmployeeData(_employeeDataDto), Times.Once);
    }

    [Fact(Skip = "Current user needs to be set for validations on endpoint")]
    public async Task UpdateEmployeeDataReturnsNotFoundResultOnException()
    {
        _employeeDataServiceMock.Setup(service => service.UpdateEmployeeData(_employeeDataDto))
                               .ThrowsAsync(new Exception("Error updating employee data."));

        var result = await _controller.UpdateEmployeeData(_employeeDataDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Error updating employee data.", errorMessage);
        _employeeDataServiceMock.Verify(service => service.UpdateEmployeeData(_employeeDataDto), Times.Once);
    }


    [Fact]
    public async Task DeleteEmployeeDataReturnsOkResult()
    {
        _employeeDataServiceMock.Setup(service => service.DeleteEmployeeData(_employeeDataDto.Id))
                               .ReturnsAsync(_employeeDataDto);

        var result = await _controller.DeleteEmployeeData(_employeeDataDto.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsType<EmployeeDataDto>(okResult.Value);
        _employeeDataServiceMock.Verify(service => service.DeleteEmployeeData(_employeeDataDto.Id), Times.Once);
    }

    [Fact]
    public async Task DeleteEmployeeDataReturnsNotFoundResultOnException()
    {
        _employeeDataServiceMock.Setup(service => service.DeleteEmployeeData(_employeeDataDto.Id))
                               .ThrowsAsync(new Exception("Error deleting employee data."));

        var result = await _controller.DeleteEmployeeData(_employeeDataDto.Id);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Error deleting employee data.", errorMessage);
        _employeeDataServiceMock.Verify(service => service.DeleteEmployeeData(_employeeDataDto.Id), Times.Once);
    }
}