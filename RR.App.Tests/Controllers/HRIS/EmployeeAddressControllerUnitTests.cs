using HRIS.Models;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data.Models.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeAddressControllerUnitTests
{
    private readonly Mock<IEmployeeAddressService> _employeeAddressServiceMock;
    private readonly EmployeeAddressController _controller;
    private readonly EmployeeAddressDto _employeeAddressDto;
    private readonly List<EmployeeAddressDto> _employeeAddressDtoList;

    public EmployeeAddressControllerUnitTests() 
    { 
        _employeeAddressServiceMock = new Mock<IEmployeeAddressService>();
        _controller = new EmployeeAddressController(_employeeAddressServiceMock.Object);
        _employeeAddressDto = EmployeeAddressTestData.EmployeeAddressDto;

        _employeeAddressDtoList = new List<EmployeeAddressDto>
        {
            EmployeeAddressTestData.EmployeeAddressDto2,
            EmployeeAddressTestData.EmployeeAddressDto3
        };
    }

    [Fact]
    public async Task GetAllReturnsOkResultWithAddresses()
    {
        _employeeAddressServiceMock.Setup(s => s.GetAll())
            .ReturnsAsync(_employeeAddressDtoList);

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualAddresses = Assert.IsAssignableFrom<List<EmployeeAddressDto>>(okResult.Value);
        Assert.Equal(_employeeAddressDtoList, actualAddresses);
    }

    [Fact]
    public async Task GetAllReturnsNotFoundResultWhenExceptionThrown()
    {
        _employeeAddressServiceMock.Setup(s => s.GetAll())
            .ThrowsAsync(new Exception("An error occurred while retrieving addresses"));

        var result = await _controller.GetAll();

        var noFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while retrieving addresses", noFoundResult.Value);
    }

    [Fact]
    public async Task SaveEmployeeAddressReturnsOkResultWithSavedAddress()
    {

        _employeeAddressServiceMock.Setup(s => s.Save(_employeeAddressDto))
            .ReturnsAsync(_employeeAddressDto);

        var result = await _controller.SaveEmployeeAddress(_employeeAddressDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualAddress = Assert.IsAssignableFrom<EmployeeAddressDto>(okResult.Value);
        Assert.Equal(_employeeAddressDto, actualAddress);
    }

    [Fact]
    public async Task SaveEmployeeAddressThrowsExceptionReturnsNotFoundResult()
    {
        _employeeAddressServiceMock.Setup(s => s.Save(_employeeAddressDto))
            .ThrowsAsync(new Exception("An error occurred while saving the address."));

        var result = await _controller.SaveEmployeeAddress(_employeeAddressDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while saving the address.", notFoundResult.Value);
    }

    [Fact]
    public async Task UpdateEmployeeAddressReturnsOkResultWithUpdatedAddress()
    {
        _employeeAddressServiceMock.Setup(s => s.Update(_employeeAddressDto))
            .ReturnsAsync(_employeeAddressDto);

        var result = await _controller.UpdateEmployeeAddress(_employeeAddressDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualAddress = Assert.IsAssignableFrom<EmployeeAddressDto>(okResult.Value);
        Assert.Equal(_employeeAddressDto, actualAddress);
    }

    [Fact]
    public async Task UpdateEmployeeAddressReturnsNotFoundResultWhenExceptionThrown()
    {
        _employeeAddressServiceMock.Setup(s => s.Update(_employeeAddressDto))
            .ThrowsAsync(new Exception("An error occurred while updating the address."));

        var result = await _controller.UpdateEmployeeAddress(_employeeAddressDto);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while updating the address.", notFoundResult.Value);
    }

    [Fact]
    public async Task DeleteEmployeeAddressReturnsOkResultWithDeletedAddress()
    {
        _employeeAddressServiceMock.Setup(s => s.Delete(_employeeAddressDto.Id))
            .ReturnsAsync(_employeeAddressDto);

        var result = await _controller.DeleteEmployeeAddress(_employeeAddressDto.Id);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualAddress = Assert.IsAssignableFrom<EmployeeAddressDto>(okResult.Value);
        Assert.Equal(_employeeAddressDto, actualAddress);
    }

    [Fact]
    public async Task DeleteEmployeeAddressReturnsNotFoundResultWhenExceptionThrown()
    {
        _employeeAddressServiceMock.Setup(s => s.Delete(_employeeAddressDto.Id))
                                  .ThrowsAsync(new Exception("An error occurred while deleting the address."));

        var result = await _controller.DeleteEmployeeAddress(_employeeAddressDto.Id);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal("An error occurred while deleting the address.", notFoundResult.Value);
    }
}