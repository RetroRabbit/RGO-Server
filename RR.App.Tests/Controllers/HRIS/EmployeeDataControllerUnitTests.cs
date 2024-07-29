using HRIS.Models;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.App.Tests.Helper;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;
public class EmployeeDataControllerUnitTests
{
    private readonly Mock<IEmployeeDataService> _employeeDataServiceMock;
    private readonly EmployeeDataController _controller;
    private readonly EmployeeDataDto _employeeDataDto;
    private readonly Mock<AuthorizeIdentityMock> _identityMock;
    public EmployeeDataControllerUnitTests()
    {
        _employeeDataServiceMock = new Mock<IEmployeeDataService>();
        _identityMock = new Mock<AuthorizeIdentityMock>();
        _controller = new EmployeeDataController(_identityMock.Object, _employeeDataServiceMock.Object);
        _employeeDataDto = EmployeeDataTestData.EmployeeDataOne.ToDto();
    }

    [Fact]
    public async Task GetEmployeeDataReturnsOkResult()
    {
        _identityMock.Setup(i => i.Role).Returns("Admin");
        _identityMock.SetupGet(i => i.EmployeeId).Returns(2);

        _employeeDataServiceMock.Setup(x => x.GetEmployeeData(_employeeDataDto.EmployeeId))
                               .ReturnsAsync(_employeeDataDto);

        var result = await _controller.GetEmployeeData(_employeeDataDto.EmployeeId);

        var okResult = Assert.IsType<OkObjectResult>(result);

        _employeeDataServiceMock.Verify(service => service.GetEmployeeData(_employeeDataDto.EmployeeId), Times.Once);
    }

    [Fact]
    public async Task GetEmployeeUnauthorizedAccess()
    {
        _identityMock.Setup(i => i.Role).Returns("Developer");
        _identityMock.SetupGet(i => i.EmployeeId).Returns(5);

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await _controller.GetEmployeeData(2));

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        Assert.Equal("User data being accessed does not match user making the request.", notFoundResult.Value);
    }

    [Fact]
    public async Task CreateEmployeeUnauthorizedAccess()
    {
        _identityMock.Setup(i => i.Role).Returns("Employee");
        _identityMock.SetupGet(i => i.EmployeeId).Returns(5);

        _employeeDataServiceMock.Setup(service => service.CreateEmployeeData(_employeeDataDto))
                              .ThrowsAsync(new CustomException("User data being accessed does not match user making the request."));

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await _controller.CreateEmployeeData(_employeeDataDto));

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("User data being accessed does not match user making the request.", notFoundResult.Value);
    }

    [Fact]
    public async Task CreateEmployeeDataReturnsOkResults()
    {
        _identityMock.Setup(i => i.Role).Returns("SuperAdmin");
        _identityMock.SetupGet(i => i.EmployeeId).Returns(2);

        _employeeDataServiceMock.Setup(x => x.CreateEmployeeData(_employeeDataDto))
                               .ReturnsAsync(_employeeDataDto);

        var result = await _controller.CreateEmployeeData(_employeeDataDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<EmployeeDataDto>(okResult.Value);
        Assert.Equal(_employeeDataDto, returnValue);
        _employeeDataServiceMock.Verify(service => service.CreateEmployeeData(_employeeDataDto), Times.Once);
    }

    [Fact]
    public async Task UpdateEmployeeDataReturnsOkResult()
    {
        _identityMock.Setup(i => i.Role).Returns("SuperAdmin");
        _identityMock.SetupGet(i => i.EmployeeId).Returns(2);

        _employeeDataServiceMock.Setup(x => x.UpdateEmployeeData(_employeeDataDto))
                               .ReturnsAsync(_employeeDataDto);

        var result = await _controller.UpdateEmployeeData(_employeeDataDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<EmployeeDataDto>(okResult.Value);
        Assert.Equal(_employeeDataDto, returnValue);
        _employeeDataServiceMock.Verify(service => service.UpdateEmployeeData(_employeeDataDto), Times.Once);
    }

    [Fact]
    public async Task UpdateUnauthorized()
    {
        _employeeDataServiceMock.Setup(service => service.UpdateEmployeeData(_employeeDataDto))
                              .ThrowsAsync(new CustomException("User data being accessed does not match user making the request."));

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await _controller.UpdateEmployeeData(_employeeDataDto));

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        Assert.Equal("User data being accessed does not match user making the request.", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteEmployeeDataReturnsOkResult()
    {
        _identityMock.Setup(i => i.Role).Returns("SuperAdmin");
        _identityMock.SetupGet(i => i.EmployeeId).Returns(2);

        _employeeDataServiceMock.Setup(service => service.DeleteEmployeeData(_employeeDataDto.Id))
                               .ReturnsAsync(_employeeDataDto);

        var result = await _controller.DeleteEmployeeData(_employeeDataDto.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<EmployeeDataDto>(okResult.Value);
        Assert.Equal(_employeeDataDto, returnValue);
        _employeeDataServiceMock.Verify(service => service.DeleteEmployeeData(_employeeDataDto.Id), Times.Once);
    }

    [Fact]
    public async Task DeleteUnauthorized()
    {
        _employeeDataServiceMock.Setup(service => service.DeleteEmployeeData(_employeeDataDto.Id))
                              .ThrowsAsync(new CustomException("Unauthorized Access."));

        var result = await MiddlewareHelperUnitTests.SimulateHandlingExceptionMiddlewareAsync(async () => await _controller.DeleteEmployeeData(1));

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }
}
