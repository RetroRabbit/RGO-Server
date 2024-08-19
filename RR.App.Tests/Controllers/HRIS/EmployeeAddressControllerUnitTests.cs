using HRIS.Models.Employee.Commons;
using HRIS.Services.Interfaces;
using HRIS.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using RR.Tests.Data;
using RR.Tests.Data.Models.HRIS;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class EmployeeAddressControllerUnitTests
{
    private readonly Mock<IEmployeeAddressService> _employeeAddressServiceMock;
    private readonly AuthorizeIdentityMock _authorizeIdentityMock;
    private readonly EmployeeAddressController _controller;
    private readonly EmployeeAddressDto _employeeAddressDto;
    private readonly List<EmployeeAddressDto> _employeeAddressDtoList;

    public EmployeeAddressControllerUnitTests() 
    {
        _employeeAddressServiceMock = new Mock<IEmployeeAddressService>();
        _controller = new EmployeeAddressController(new AuthorizeIdentityMock("test@example.com", "TestUser", "Employee", 1), _employeeAddressServiceMock.Object);

        _employeeAddressDto = EmployeeAddressTestData.EmployeeAddressOne.ToDto();

        _employeeAddressDtoList = new List<EmployeeAddressDto>
        {
            EmployeeAddressTestData.EmployeeAddressTwo.ToDto(),
            EmployeeAddressTestData.EmployeeAddressThree.ToDto()
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
    public async Task SaveEmployeeAddressReturnsOkResultWithSavedAddress()
    {
        _employeeAddressServiceMock.Setup(s => s.Create(_employeeAddressDto))
            .ReturnsAsync(_employeeAddressDto);

        var result = await _controller.SaveEmployeeAddress(_employeeAddressDto);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualAddress = Assert.IsAssignableFrom<EmployeeAddressDto>(okResult.Value);
        Assert.Equal(_employeeAddressDto, actualAddress);
    }

    [Fact]
    public async Task SaveEmployeeUnauthorized()
    {
        var unauthorizedIdentity = new AuthorizeIdentityMock("unauthorized@example.com", "UnauthorizedUser", "User", 2);
        var controller = new EmployeeAddressController(unauthorizedIdentity, _employeeAddressServiceMock.Object);

        var exception = await Assert.ThrowsAsync<CustomException>(() => controller.SaveEmployeeAddress(_employeeAddressDto));
        _employeeAddressServiceMock.Setup(x => x.Create(_employeeAddressDto))
            .ThrowsAsync(new CustomException("Unauthorized Access."));
      
        Assert.Equal("Unauthorized Access.", exception.Message);
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
    public async Task UpdateEmployeeAddress_ThrowsCustomException()
    {
        var unauthorizedIdentity = new AuthorizeIdentityMock("unauthorized@example.com", "UnauthorizedUser", "User", 2);
        var controller = new EmployeeAddressController(unauthorizedIdentity, _employeeAddressServiceMock.Object);

        var exception = await Assert.ThrowsAsync<CustomException>(() => controller.UpdateEmployeeAddress(_employeeAddressDto));
        Assert.Equal("Unauthorized Access.", exception.Message);
    }

    [Fact]
    public async Task DeleteEmployeeAddressReturnsOkResultWithDeletedAddress()
    {
        _employeeAddressServiceMock.Setup(s => s.Delete(_employeeAddressDto.EmployeeId))
            .ReturnsAsync(_employeeAddressDto);

        var result = await _controller.DeleteEmployeeAddress(_employeeAddressDto.EmployeeId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualAddress = Assert.IsAssignableFrom<EmployeeAddressDto>(okResult.Value);
        Assert.Equal(_employeeAddressDto, actualAddress);
    }

    [Fact]
    public async Task GetEmployeeAddressByIdSuccessReturnsOkResultWithAddress()
    {
        _employeeAddressServiceMock.Setup(x => x.GetById(_employeeAddressDto.EmployeeId)).ReturnsAsync(_employeeAddressDto);

        var result = await _controller.GetEmployeeAddressById(_employeeAddressDto.EmployeeId);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var actualAddress = Assert.IsType<EmployeeAddressDto>(okResult.Value);

        Assert.Equal(_employeeAddressDto, actualAddress);
    }

    [Fact]
    public async Task GetEmployeeAddressById_ThrowsCustomException()
    {
        var unauthorizedIdentity = new AuthorizeIdentityMock("unauthorized@example.com", "UnauthorizedUser", "User", 2);
        var controller = new EmployeeAddressController(unauthorizedIdentity, _employeeAddressServiceMock.Object);

        var exception = await Assert.ThrowsAsync<CustomException>(() => controller.GetEmployeeAddressById(_employeeAddressDto.EmployeeId));
        Assert.Equal("Unauthorized Access.", exception.Message);
    }

}