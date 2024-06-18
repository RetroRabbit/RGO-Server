using HRIS.Models;
using HRIS.Models.Enums;
using HRIS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using RR.App.Controllers.HRIS;
using System.Security.Claims;
using Xunit;

namespace RR.App.Tests.Controllers.HRIS;

public class PropertyAccessControllerUnitTests
{

    private readonly Mock<IPropertyAccessService> _propertyAccessMockService;
    private readonly Mock<IEmployeeService> _employeeMockService;
    private readonly PropertyAccessController _propertyAccessController;

    public PropertyAccessControllerUnitTests() {
        _propertyAccessMockService = new Mock<IPropertyAccessService>();
        _employeeMockService = new Mock<IEmployeeService>();
        _propertyAccessController = new PropertyAccessController(_propertyAccessMockService.Object, _employeeMockService.Object);
    }

    [Fact]
    public async Task GeAllPropertyWithAccessReturnsOkResult()
    {
        _propertyAccessMockService.Setup(service => service.GetAll())
                                 .ReturnsAsync(new List<PropertyAccessDto>());

        var result = await _propertyAccessController.GetAllPropertyAccess();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var model = Assert.IsAssignableFrom<List<PropertyAccessDto>>(okResult.Value);

        _propertyAccessMockService.Verify(service => service.GetAll(), Times.Once);
    }

    [Fact]
    public async Task GeAllPropertyWithAccessNotFoundResult()
    {
        _propertyAccessMockService.Setup(service => service.GetAll())
                                 .ThrowsAsync(new
                                                  Exception("Error retrieving properties with access for the specified user."));

        var result = await _propertyAccessController.GetAllPropertyAccess();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Error retrieving properties with access for the specified user.", errorMessage);

    }

    //[Fact]
    //public async Task GetPropertyWithAccessReturnsOkResult()
    //{
    //    _propertyAccessMockService.Setup(service => service.GetAccessListByEmployeeId(0))
    //                             .ReturnsAsync(new List<PropertyAccessDto>());

    //    var result = await _propertyAccessController.GetPropertiesWithAccess(0);

    //    var okResult = Assert.IsType<OkObjectResult>(result);
    //    var model = Assert.IsAssignableFrom<List<PropertyAccessDto>>(okResult.Value);
    //    _propertyAccessMockService.Verify(service => service.GetAccessListByEmployeeId(0), Times.Once);
    //}

    //[Fact]
    //public async Task GetPropertyWithAccessReturnsNotFoundResult()
    //{
    //    _propertyAccessMockService.Setup(service => service.GetAccessListByEmployeeId(0))
    //                             .ThrowsAsync(new Exception("Error retrieving properties with access for the specified user."));

    //    var result = await _propertyAccessController.GetPropertiesWithAccess(0);

    //    var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
    //    var errorMessage = Assert.IsType<string>(notFoundResult.Value);
    //    Assert.Equal("Error retrieving properties with access for the specified user.", errorMessage);
    //    _propertyAccessMockService.Verify(service => service.GetAccessListByEmployeeId(0), Times.Once);
    //}

    [Fact]
    public async Task SeedPropertiesSuccess()
    {
        var result = await _propertyAccessController.Seed();

        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task SeedPropertiesFail()
    {
        _propertyAccessMockService.Setup(service => service.CreatePropertyAccessEntries())
                                .ThrowsAsync(new
                                                 Exception("Error retrieving properties with access for the specified user."));
        var result = await _propertyAccessController.Seed();

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorMessage = Assert.IsType<string>(notFoundResult.Value);
    }

    [Fact]
    public async Task UpdatePropertyRoleAccessReturnsSuccess()
    {
        _propertyAccessMockService.Setup(service => service.UpdatePropertyAccess(0, PropertyAccessLevel.read));

        var result = await _propertyAccessController.UpdatePropertyRoleAccess(0, PropertyAccessLevel.read);

        Assert.IsType<OkResult>(result);

        _propertyAccessMockService.Verify(service => service.UpdatePropertyAccess(0, PropertyAccessLevel.read), Times.Once);
    }

    [Fact]
    public async Task UpdatePropertyRoleAccessReturnsFail()
    {
        _propertyAccessMockService.Setup(service => service.UpdatePropertyAccess(0, PropertyAccessLevel.read))
                                 .ThrowsAsync(new
                                                  Exception("Error updating properties with access for the specified user."));

        var result = await _propertyAccessController.UpdatePropertyRoleAccess(0, PropertyAccessLevel.read);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        var errorMessage = Assert.IsType<string>(notFoundResult.Value);
        Assert.Equal("Error updating properties with access for the specified user.", errorMessage);

        _propertyAccessMockService.Verify(service => service.UpdatePropertyAccess(0, PropertyAccessLevel.read), Times.Once);
    }

    //[Fact]
    //public async Task GetUserIdReturnsOkResult()
    //{
    //    _employeeMockService.Setup(service => service.GetEmployee("test@retrorabbit.co.za")).ReturnsAsync(EmployeeTestData.EmployeeDto);

    //    var result = await _propertyAccessController.GetUserId("test@retrorabbit.co.za");

    //    var okResult = Assert.IsType<OkObjectResult>(result);
    //    Assert.Equal(1, okResult.Value!);
    //    _employeeMockService.Verify(service => service.GetEmployee("test@retrorabbit.co.za"), Times.Once);
    //}

    //[Fact]
    //public async Task GetUserIdReturnsNotFoundResult()
    //{
    //    _employeeMockService.Setup(service => service.GetEmployee("test@retrorabbit.co.za")).ThrowsAsync(new Exception("Error retrieving properties with access for the specified user."));

    //    var result = await _propertyAccessController.GetUserId("test@retrorabbit.co.za");

    //    var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
    //    var errorMessage = Assert.IsType<string>(notFoundResult.Value);
    //    Assert.Equal("Error retrieving properties with access for the specified user.", errorMessage);
    //    _employeeMockService.Verify(service => service.GetEmployee("test@retrorabbit.co.za"), Times.Once);
    //}
}